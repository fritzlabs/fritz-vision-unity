#!/bin/bash

# Download sdk to Frameworks folder.

SDK_VERSION=3.5.6
DEST=Frameworks/


function downloadFramework() {
    FRAMEWORK=$1

    echo "Downloading Framework $FRAMEWORK"
    wget "https://github.com/fritzlabs/swift-framework/releases/download/${SDK_VERSION}/${FRAMEWORK}.zip"
    unzip -o ${FRAMEWORK}.zip -d tmp
    mv tmp/Frameworks/* $DEST
    rm ${FRAMEWORK}.zip
}

mkdir tmp/
downloadFramework FritzBase
downloadFramework FritzVisionPoseModel
rm -rf tmp/
