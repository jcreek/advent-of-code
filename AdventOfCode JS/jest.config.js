module.exports = {
  testEnvironment: "node",
  roots: ["<rootDir>"],
  moduleNameMapper: {
    "^Data/(.*)$": "<rootDir>/2024/Data/$1",
    "^Tasks/(.*)$": "<rootDir>/2024/Tasks/$1",
  },
};
