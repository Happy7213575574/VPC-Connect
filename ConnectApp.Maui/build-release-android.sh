#!/bin/bash

rm -r ConnectApp.Maui/bin
rm -r ConnectApp.Maui/obj
dotnet build -f net9.0-android --configuration Release

