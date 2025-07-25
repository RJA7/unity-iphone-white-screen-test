import { argv } from "process";
import fs from "fs";

export function isProductionBuild() {
  return argv[2] === "--production";
}

export function getSlackPayload(success, version, extraBlocks) {
  const buildNumber = fs.readFileSync("./build_number", "UTF-8").trim();

  const headerPrefix =
    version === null
      ? "🧪 Development release"
      : `🚀 Production release ${version}`;

  const headerSuffix = success
    ? "has been sent to Unity Cloud Build!"
    : "could not be sent to Unity Cloud Build!";

  return [
    {
      type: "header",
      text: {
        type: "plain_text",
        text: `${headerPrefix} (build ${buildNumber}) ${headerSuffix}`,
        emoji: true,
      },
    },
  ];
}
