/** @type {import('ts-jest').JestConfigWithTsJest} **/
module.exports = {
  setupFilesAfterEnv: ["<rootDir>/src/setupTests.ts"],
  testEnvironment: "jsdom",
  transform: {
    "^.+.tsx?$": ["ts-jest", {}]
  }
};
