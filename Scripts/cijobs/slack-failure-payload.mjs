import { getSlackPayload, isProductionBuild } from "./slack-payload.mjs";

const lastVersion = isProductionBuild()
  ? changelog.releases[1]
  : changelog.releases[0];
const versionNumber = isProductionBuild() ? lastVersion.version : null;

const slackPayload = getSlackPayload(false, versionNumber, []);

console.log(JSON.stringify(slackPayload, null, 2));
