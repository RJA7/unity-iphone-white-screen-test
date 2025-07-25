import fs from "fs";

function getEnvironmentVariable(key, fallback) {
  const value = process.env[key];

  if (value === undefined) {
    if (fallback === undefined) {
      throw new Error(`Environment variable ${key} is not set`);
    }

    return fallback;
  }

  return value;
}

const UCB_BUILD_TARGET = getEnvironmentVariable("UCB_BUILD_TARGET");

function readBuildNumberFile() {
  const buildNumber = fs.readFileSync("build_number", "UTF-8").trim();

  return buildNumber;
}

function getFirebaseCredentials() {
  if (UCB_BUILD_TARGET === "tile-hero-webgl-dev-build") {
    return getEnvironmentVariable("FIREBASE_HOSTING_DEV_CREDENTIALS");
  }

  return getEnvironmentVariable("FIREBASE_HOSTING_PROD_CREDENTIALS");
}

function isProductionBuild() {
  return (
    UCB_BUILD_TARGET === "tile-hero-ios" ||
    UCB_BUILD_TARGET === "tile-hero-android" ||
    UCB_BUILD_TARGET === "tile-hero-fb-instant" ||
    UCB_BUILD_TARGET === "tile-hero-webgl"
  );
}

function getPlatformEnvvars() {
  if (
    UCB_BUILD_TARGET === "tile-hero-ios" ||
    UCB_BUILD_TARGET === "tile-hero-ios-dev-build"
  ) {
    return {
      APPLE_CONNECT_KEY: getEnvironmentVariable("APPLE_CONNECT_KEY"),
    };
  }

  if (
    UCB_BUILD_TARGET === "tile-hero-android" ||
    UCB_BUILD_TARGET === "tile-hero-android-dev-build"
  ) {
    return {
      PLAYSTORE_KEY: getEnvironmentVariable("PLAYSTORE_KEY"),
      FIREBASE_APP_ID: getEnvironmentVariable("FIREBASE_APP_ID"),
    };
  }

  if (
    UCB_BUILD_TARGET === "tile-hero-webgl" ||
    UCB_BUILD_TARGET === "tile-hero-webgl-dev-build"
  ) {
    return {
      GITLAB_NPM_TOKEN: getEnvironmentVariable(
        "BUILD_PIPELINE_GITLAB_PACKAGE_TOKEN"
      ),
      SENTRY_AUTH_TOKEN_WEB: getEnvironmentVariable("SENTRY_AUTH_TOKEN_WEB"),
      FIREBASE_CREDENTIALS: getFirebaseCredentials(),
    };
  }

  if (
    UCB_BUILD_TARGET === "tile-hero-fb-instant" ||
    UCB_BUILD_TARGET === "tile-hero-fb-instant-dev-build"
  ) {
    return {
      GITLAB_NPM_TOKEN: getEnvironmentVariable(
        "BUILD_PIPELINE_GITLAB_PACKAGE_TOKEN"
      ),
      SENTRY_AUTH_TOKEN_WEB: getEnvironmentVariable("SENTRY_AUTH_TOKEN_WEB"),
      FB_INSTANT_ACCESS_TOKEN: getEnvironmentVariable(
        "FB_INSTANT_ACCESS_TOKEN"
      ),
      FB_UPLOAD_COMMENT: [
        getEnvironmentVariable("CIJOBS_BUILD_NOTES", ""),
        `Build number: ${readBuildNumberFile()}`,
        `Branch: ${getEnvironmentVariable("CI_COMMIT_BRANCH", "-")} (${getEnvironmentVariable("CI_COMMIT_SHORT_SHA")})`
      ].filter(Boolean).join('; '),
    };
  }

  return {};
}

function generateEnvvars() {
  return {
    ...getPlatformEnvvars(),
    CI_PIPELINE_BUILD_NUMBER: readBuildNumberFile(),
    IS_PRODUCTION_BUILD: isProductionBuild() ? "true" : "false",
  };
}

console.log(JSON.stringify(generateEnvvars()));
