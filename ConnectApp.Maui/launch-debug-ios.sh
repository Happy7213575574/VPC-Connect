#!/bin/bash

UDID=$(./get-udid.sh "iPhone 14 Pro Max")
dotnet build -t:Run -f net7.0-ios --configuration Debug -p:_DeviceName=:v2:udid=$UDID
