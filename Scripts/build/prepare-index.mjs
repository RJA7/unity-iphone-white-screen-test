import fs from "fs";
import path from "path";

let playerPath;

if (process.env.UNITY_PLAYER_PATH) {
  playerPath = process.env.UNITY_PLAYER_PATH;
} else {
  playerPath = "../Build";
}

const src = path.join(playerPath, "index.html");
const dest = path.resolve("index.html");

if (!fs.existsSync(src)) {
  console.error(`Source file not found: ${src}`);
  process.exit(1);
}

try {
  if (fs.existsSync(dest)) {
    fs.unlinkSync(dest);
  }
  fs.copyFileSync(src, dest);
  console.log(`Copied ${src} to ${dest}`);
} catch (err) {
  console.error("Error copying file:", err.message);
  process.exit(1);
}
