var envVarName = process.argv[2];

if (!envVarName) {
  console.error("Usage: node check-envvar.mjs <ENV_VAR_NAME>");
  process.exit(1);
}

if (!process.env[envVarName]) {
  console.error(`Environment variable ${envVarName} is not set.`);
  process.exit(1);
}
