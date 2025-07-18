#!/bin/bash

rm -r ConnectApp.Maui/bin
rm -r ConnectApp.Maui/obj
adb uninstall org.vpc.connect
UDID=$(./get-udid.sh "iPhone 14 Pro Max")
dotnet build -t:Run -f net9.0-ios --configuration Debug -p:_DeviceName=:v2:udid=$UDID
