#!/bin/bash

# Download sdk to Frameworks folder.

SDK_VERSION=3.7.0-beta.1
DEST=Frameworks/


function downloadFramework() {
    FRAMEWORK=$1

    echo "Downloading Framework $FRAMEWORK"
    rm ${FRAMEWORK}.zip
    rm -r $DEST/*
    wget "https://github.com/fritzlabs/swift-framework/releases/download/${SDK_VERSION}/${FRAMEWORK}.zip"

    unzip -o ${FRAMEWORK}.zip -d tmp
    mv tmp/Frameworks/* $DEST
    rm ${FRAMEWORK}.zip
}

mkdir tmp/
downloadFramework FritzBase
downloadFramework FritzVisionPoseModel
rm -rf tmp/
