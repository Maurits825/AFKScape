#! /bin/sh

PROJECT_PATH=$(pwd)/$UNITY_PROJECT_PATH
UNITY_BUILD_DIR=$(pwd)/Build
UNITY_PATH="/Applications/Unity/Unity.app/Contents/MacOS/Unity"
LOG_FILE=$UNITY_BUILD_DIR/unity-win.log

echo "Items in project path ($PROJECT_PATH):"
ls "$PROJECT_PATH"

echo "Building project for Windows..."
mkdir $UNITY_BUILD_DIR
${UNITY_PATH} \
  -batchmode \
  -nographics \
  -silent-crashes \
  -logFile \
  -projectPath "$PROJECT_PATH" \
  -buildWindows64Player  "$(pwd)/build/win/ci-build.exe" \
  -quit \
  | tee "$LOG_FILE"
