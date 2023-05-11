#!/bin/bash

rm -r ConnectApp.Maui/bin/Release/
rm -r ConnectApp.Maui/obj/Release/
dotnet build -t:Run -f net7.0-android --configuration Release
