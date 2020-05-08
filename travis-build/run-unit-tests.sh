#! /bin/sh

echo "[SYNG2] Running editor unit tests for ${UNITY_PROJECT_PATH}"

${UNITY_PATH} \
  -batchmode \
  -silent-crashes \
  -logFile "${TRAVIS_BUILD_DIR}/unity.unittests.log" \
  -projectPath "${TRAVIS_BUILD_DIR}/${UNITY_PROJECT_PATH}/" \
  -runEditorTests \
  -editorTestsResultFile "${TRAVIS_BUILD_DIR}/unity.unittests.xml"
