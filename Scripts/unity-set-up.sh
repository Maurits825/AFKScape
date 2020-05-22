#!/bin/bash

echo "--- Activate License ---"
/opt/Unity/Editor/Unity -batchmode -manualLicenseFile license.ulf -logfile /dev/stdout -nographics -quit

echo "--- Sync solution ---"
/opt/Unity/Editor/Unity -batchmode -logFile /dev/stdout -projectPath AFKScape/ -executeMethod MainControllerEditor.createSLN -nographics -quit

echo "--- Checking Env ---"
ls AFKScape
