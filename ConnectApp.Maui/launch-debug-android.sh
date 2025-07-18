#!/bin/bash

rm -r ConnectApp.Maui/bin
rm -r ConnectApp.Maui/obj
adb uninstall org.vpc.connect
dotnet build -t:Run -f net9.0-android --configuration Debug
