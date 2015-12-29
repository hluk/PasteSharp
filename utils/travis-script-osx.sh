#!/bin/bash
set -e -x
make

# FIXME: There are some missing libraries.
#   (e.g. /Library/Frameworks/Mono.framework/Versions/Current/lib/libgtk-3.0.dylib)
#make tests

