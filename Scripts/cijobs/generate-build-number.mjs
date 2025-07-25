function generateBuildNumber() {
  // The build number calculation is based on the number of seconds since the beginning of the project.
  const referenceDate = new Date(2024, 2, 21, 0, 0, 0, 0);
  const now = new Date();

  const diff = now.getTime() - referenceDate.getTime();
  const buildNumber = Math.floor(diff / 1000);

  return buildNumber;
}

console.log(generateBuildNumber());
