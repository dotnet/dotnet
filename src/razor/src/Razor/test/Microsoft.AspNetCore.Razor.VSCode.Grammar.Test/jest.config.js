// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

module.exports = {
  globals: {
    "ts-jest": {
      "tsConfig": "./tsconfig.json",
      "babeConfig": true,
      "diagnostics": true
    }
  },
  testPathIgnorePatterns: [ 'dist' ],
  preset: 'ts-jest',
  testEnvironment: 'jsdom'
};
