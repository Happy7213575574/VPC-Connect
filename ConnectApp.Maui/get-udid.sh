#!/bin/bash

DEVICE=$1
UDID=$(/Applications/Xcode.app/Contents/Developer/usr/bin/simctl list \
  | awk '/== Devices ==/,/== Device Pairs ==/' \
  | grep "$DEVICE" \
  | cut -d "(" -f2 | cut -d ")" -f1)

echo $UDID