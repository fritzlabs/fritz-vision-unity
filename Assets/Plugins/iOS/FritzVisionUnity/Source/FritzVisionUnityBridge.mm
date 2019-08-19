//
//  FritzVisionUnityBridge.mm
//  FritzVisionUnity
//
//  Created by Christopher Kelly on 7/9/19.
//  Copyright © 2019 Fritz Labs Incorporated. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <AVFoundation/AVFoundation.h>
#import "UnityXRNativePtrs.h"
#import <ARKit/ARKit.h>
#include "FritzVisionUnity-Swift.h"
#pragma mark - C interface

extern "C" {

  void _configure() {
    [FritzVisionUnity configure];
  }

  char* _processPose(intptr_t ptr) {

    // In case of invalid buffer ref
    if (!ptr) return 0;

    UnityXRNativeFrame_1* unityXRFrame = (UnityXRNativeFrame_1*) ptr;
    ARFrame* frame = (__bridge ARFrame*)unityXRFrame->framePtr;

    CVPixelBufferRef buffer = frame.capturedImage;
    // Forward message to the swift api
    NSString *returnString = [[FritzVisionUnityPoseModel shared] processFrameWithBuffer: buffer];
    char* cStringCopy(const char* string);
    return cStringCopy([returnString UTF8String]);
  }

  void _setMinPartThreshold(float threshold) {
    [FritzVisionUnityPoseModel shared].minPartThreshold = threshold;
  }

  void _setMinPoseThreshold(float threshold) {
    [FritzVisionUnityPoseModel shared].minPoseThreshold = threshold;
  }

  void _setNumPoses(int poses) {
    [FritzVisionUnityPoseModel shared].numPoses = poses;
  }

}

char* cStringCopy(const char* string){
  if (string == NULL){
    return NULL;
  }
  char* res = (char*)malloc(strlen(string)+1);
  strcpy(res, string);
  return res;
}
