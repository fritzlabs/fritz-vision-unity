//
//  FritzVisionUnity.swift
//  FritzVisionUnity
//
//  Created by Christopher Kelly on 7/9/19.
//  Copyright © 2019 Fritz Labs Incorporated. All rights reserved.
//

import Foundation

import AVFoundation
import UIKit
import FritzVisionPoseModel



@available(iOS 11.0, *)
@objc public class FritzVisionUnity: NSObject, AVCaptureVideoDataOutputSampleBufferDelegate {

  @objc class func configure() {
    FritzCore.configure()
  }
}


@available(iOS 11.0, *)
@objc public class CameraBase: NSObject, AVCaptureVideoDataOutputSampleBufferDelegate {
  private lazy var captureSession: AVCaptureSession = {
    let session = AVCaptureSession()

    guard
      let backCamera = AVCaptureDevice.default(
        .builtInWideAngleCamera,
        for: .video,
        position: .back),
      let input = try? AVCaptureDeviceInput(device: backCamera)
      else { return session }
    session.addInput(input)

    // The style transfer takes a 640x480 image as input and outputs an image of the same size.
    session.sessionPreset = AVCaptureSession.Preset.vga640x480
    return session
  }()

  var queue = DispatchQueue(label: "ai.fritz.fritzvisionunity.posequeue")

  public override init() {
    super.init()
    let videoOutput = AVCaptureVideoDataOutput()
    videoOutput.videoSettings = [kCVPixelBufferPixelFormatTypeKey as String: kCVPixelFormatType_32BGRA as UInt32]
    videoOutput.setSampleBufferDelegate(self, queue: queue)
    self.captureSession.addOutput(videoOutput)
  }

  @objc func startCamera() {
    queue.async {
      self.captureSession.startRunning()
    }
  }

  @objc func stopCamera() {
    queue.async {
      self.captureSession.stopRunning()
    }
  }

  open func captureOutput(_ output: AVCaptureOutput, didOutput sampleBuffer: CMSampleBuffer, from connection: AVCaptureConnection) { }
}


@available(iOS 11.0, *)
@objc public class FritzVisionUnityPoseModel: NSObject {

  var latestPose: FritzVisionPoseResult<HumanSkeleton>?
  lazy var poseModel = FritzVisionHumanPoseModel()

  @objc public var callbackTarget = "FritzPoseController"
  @objc public var callbackFunctionTarget = "UpdatePose"

  private let modelQueue = DispatchQueue(label: "ai.fritz.Unity")

  @objc static let shared = FritzVisionUnityPoseModel()

  @objc var minPartThreshold = 0.6
  @objc var minPoseThreshold = 0.3
  @objc var numPoses = 5

  @objc public private(set) var processing = false

  private var retainedBuffer: CVPixelBuffer?

  @objc func getLatestPoseImage() -> CVPixelBuffer? {
    guard let latestPose = latestPose else { return nil }
    return latestPose.image.toPixelBuffer()
  }

  func encodedPoses(for poses: [Pose<HumanSkeleton>]) -> String {
    let convertedPoses = poses.map { $0.keypoints.map { [Double($0.part.rawValue), Double($0.position.x), Double($0.position.y), Double($0.score)] }}
    let jsonEncoder = JSONEncoder()
    if let data = try? jsonEncoder.encode(convertedPoses) {
      return String(data: data, encoding: .utf8) ?? ""
    }
    return ""
  }

  @objc(processFrameWithBuffer:)
  func processFrame(buffer: CVPixelBuffer) -> String {
    let image = FritzVisionImage(imageBuffer: buffer)
    image.metadata = FritzVisionImageMetadata()
    image.metadata?.orientation = .right

    let options = FritzVisionPoseModelOptions()
    options.minPartThreshold = minPartThreshold
    options.minPoseThreshold = minPoseThreshold

    guard let result = try? poseModel.predict(image, options: options) else { return "" }
    let poses = result.poses(limit: numPoses)
    return encodedPoses(for: poses)
  }

  @objc(processFrameAsyncWithBuffer:)
  func processFrameAsync(buffer: CVPixelBuffer) {
    if processing {
      return
    }
    modelQueue.async {
      defer { self.retainedBuffer = nil }
      self.processing = true
      let result = self.processFrame(buffer: buffer)
      self.processing = false
      DispatchQueue.main.async {
        print(self.callbackTarget)
        UnitySendMessage(
          self.callbackTarget,
          self.callbackFunctionTarget,
          result
        )
      }
    }
  }
}
