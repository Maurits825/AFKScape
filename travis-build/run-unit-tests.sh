#! /bin/sh

echo "Running editor unit tests for ${UNITY_PROJECT_PATH}"
echo ${UNITY_PATH} 

${UNITY_PATH} \
  -silent-crashes \
  -batchmode \
  -logFile "${TRAVIS_BUILD_DIR}/unity.unittests.log" \
  -projectPath "${TRAVIS_BUILD_DIR}/${UNITY_PROJECT_PATH}/" \
  -runEditorTests \
  -editorTestsResultFile "${TRAVIS_BUILD_DIR}/unity.unittests.xml"
