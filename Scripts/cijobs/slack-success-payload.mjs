import fs from "fs";
import { parser } from "keep-a-changelog";
import { getSlackPayload, isProductionBuild } from "./slack-payload.mjs";

function formatChangelogBlock(changes, title) {
  if (!changes || changes.length === 0) {
    return null;
  }

  const formattedChanges = changes
    .map((change) => `• ${change.title}`)
    .join("\n");

  return {
    type: "section",
    text: {
      type: "mrkdwn",
      text: `*${title}*\n${formattedChanges}`,
    },
  };
}

function pushChangelogBlockConditionally(changelogSections, changes) {
  if (changes === null) {
    return;
  }

  changelogSections.push(changes);
}

const changelog = parser(fs.readFileSync("CHANGELOG.md", "UTF-8"));
const lastVersion = isProductionBuild() ? changelog.releases[1] : changelog.releases[0];
const versionNumber = isProductionBuild() ? lastVersion.version : null;

const addedChanges = formatChangelogBlock(
  lastVersion.changes.get("added"),
  "✨ Added"
);
const changedChanges = formatChangelogBlock(
  lastVersion.changes.get("changed"),
  "♻️ Changed"
);
const fixedChanges = formatChangelogBlock(
  lastVersion.changes.get("fixed"),
  "🔧 Fixed"
);
const removedChanges = formatChangelogBlock(
  lastVersion.changes.get("removed"),
  "❌ Removed"
);
const deprecatedChanges = formatChangelogBlock(
  lastVersion.changes.get("deprecated"),
  "🛑 Deprecated"
);
const securityChanges = formatChangelogBlock(
  lastVersion.changes.get("security"),
  "🔒 Security"
);

const changelogSections = [];

pushChangelogBlockConditionally(changelogSections, addedChanges);
pushChangelogBlockConditionally(changelogSections, changedChanges);
pushChangelogBlockConditionally(changelogSections, fixedChanges);
pushChangelogBlockConditionally(changelogSections, removedChanges);
pushChangelogBlockConditionally(changelogSections, deprecatedChanges);
pushChangelogBlockConditionally(changelogSections, securityChanges);

const slackPayload = getSlackPayload(true, versionNumber, [
  {
    type: "divider",
  },
  ...changelogSections,
]);

console.log(JSON.stringify(slackPayload, null, 2));
