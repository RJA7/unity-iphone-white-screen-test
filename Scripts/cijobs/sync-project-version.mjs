import fs from "fs";
import { fileURLToPath } from "url";
import { dirname } from "path";

const __filename = fileURLToPath(import.meta.url);
const __dirname = dirname(__filename);

const packageJson = JSON.parse(
  fs.readFileSync(`${__dirname}/../../package.json`, "UTF-8")
);

const version = packageJson.version;

const projectSettingsPath = `${__dirname}/../../ProjectSettings/ProjectSettings.asset`;
const projectSettings = fs.readFileSync(projectSettingsPath, "UTF-8");

const newProjectSettings = projectSettings.replace(
  /bundleVersion: .+/,
  `bundleVersion: ${version}`
);

fs.writeFileSync(projectSettingsPath, newProjectSettings, "UTF-8");
