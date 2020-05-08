#! /bin/sh

UNITY_PATH="/Applications/Unity/Unity.app/Contents/MacOS/Unity"
echo "[SYNG2] Unity path: ${UNITY_PATH}"
echo "[SYNG2] Travis build dir: ${TRAVIS_BUILD_DIR}"

returnLicense() {
    echo "[SYNG2] Return license"

    ${UNITY_PATH} \
        -logFile "${TRAVIS_BUILD_DIR}/unity.returnlicense.log" \
        -batchmode \
        -returnlicense \
        -quit
    cat "$(pwd)/unity.returnlicense.log"
}

activateLicense() {
    echo "[SYNG2] Activate Unity"

    ${UNITY_PATH} \
        -logFile "${TRAVIS_BUILD_DIR}/unity.activation.log" \
        -serial ${UNITY_SERIAL} \
        -username ${UNITY_USER} \
        -password ${UNITY_PWD} \
        -batchmode \
        -noUpm \
        -quit
    echo "[SYNG2] Unity activation log"
    cat "${TRAVIS_BUILD_DIR}/unity.activation.log"
}

unitTests() {
    echo "[SYNG2] Running editor unit tests for ${UNITY_PROJECT_NAME}"

    ${UNITY_PATH} \
        -batchmode \
        -silent-crashes \
        -serial ${UNITY_SERIAL} \
        -username ${UNITY_USER} \
        -password ${UNITY_PWD} \
        -logFile "${TRAVIS_BUILD_DIR}/unity.unittests.log" \
        -projectPath "${TRAVIS_BUILD_DIR}/${UNITY_PROJECT_NAME}/" \
        -runEditorTests \
        -editorTestsResultFile "${TRAVIS_BUILD_DIR}/unity.unittests.xml"

    rc0=$?
    echo "[SYNG2] Unit test log"
    cat "${TRAVIS_BUILD_DIR}/unity.unittests.xml"

    # exit if tests failed
    if [ $rc0 -ne 0 ]; then { echo "[SYNG2] Unit tests failed"; exit $rc0; } fi
}

prepareBuilds() {
    echo "[SYNG2] Preparing building"

    mkdir ${BUILD_PATH}
    echo "[SYNG2] Created directory: ${BUILD_PATH}"
}

buildWindows() {
    echo "[SYNG2] Building ${UNITY_PROJECT_NAME} for Windows"

    ${UNITY_PATH} \
        -batchmode \
        -silent-crashes \
        -logFile "${TRAVIS_BUILD_DIR}/unity.build.log" \
        -projectPath "${TRAVIS_BUILD_DIR}/${UNITY_PROJECT_NAME}" \
        -buildWindows64Player "${BUILD_PATH}/win/ci-build.exe" \
        -quit

    rc1=$?
    echo "[SYNG2] Build logs (iOS)"
    cat "${TRAVIS_BUILD_DIR}/unity.build.ios.log"

    # exit if build failed
    if [ $rc1 -ne 0 ]; then { echo "[SYNG2] Build failed"; exit $rc1; } fi
}


# ------------------------------------------------------------------------------

# if environment variable CI is not set
activateLicense
unitTests
prepareBuilds
buildWindows
returnLicense
exit 0
