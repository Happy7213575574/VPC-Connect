#!/bin/bash

rm -r ConnectApp.Maui/bin/Release/
rm -r ConnectApp.Maui/obj/Release/
UDID=$(./get-udid.sh "iPhone 14 Pro Max")
dotnet build -t:Run -f net7.0-ios --configuration Release -p:_DeviceName=:v2:udid=$UDID
