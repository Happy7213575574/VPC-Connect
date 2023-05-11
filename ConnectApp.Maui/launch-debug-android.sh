#!/bin/bash

rm -r ConnectApp.Maui/bin/Debug/
rm -r ConnectApp.Maui/obj/Debug/
dotnet build -t:Run -f net7.0-android --configuration Debug
