//
//  FritzVisionUnityBridge.mm
//  FritzVisionUnity
//
//  Created by Christopher Kelly on 7/9/19.
//  Copyright © 2019 Fritz Labs Incorporated. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <AVFoundation/AVFoundation.h>

#include "FritzVisionUnity-Swift.h"
#pragma mark - C interface

extern "C" {

  void _startCamera() {
    [[FritzVisionUnityPoseModel shared] startCamera];
  }

  void _stopCamera() {
    [[FritzVisionUnityPoseModel shared] stopCamera];
    
  }

  void _configure() {
    [FritzVisionUnity configure];
  }

  char* _getLatestPose() {
    NSString *returnString = [[FritzVisionUnityPoseModel shared] getLatestEncodedPose];
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
