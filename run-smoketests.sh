#!/bin/bash

docker build --platform linux/x86_64 . -f SmokeTests.dockerfile -t mvp-tests
docker run --rm --platform linux/x86_64 mvp-tests
