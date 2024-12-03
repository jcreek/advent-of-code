const fs = require("fs");
const path = require("path");

describe("2015 Day 01", () => {
  let realData;

  beforeAll(() => {
    const filePath = path.resolve(__dirname, "./Data/Day01.dat");
    realData = fs.readFileSync(filePath, "utf-8").split("\n");
  });

  function part1(input) {
    const data = input ? input : realData;

    // Go through each character and add 1 for ( and -1 for )
    let floor = 0;

    // for each character in data
    data.split("").forEach((character) => {
      if (character === "(") {
        floor++;
      } else if (character === ")") {
        floor--;
      }
    });

    return floor;
  }

  function part2(input) {
    const data = input ? input : realData;

    // Go through each character and add 1 for ( and -1 for )
    let floor = 0;

    // for each character in data
    for (let index = 0; index < data.length; index++) {
      const character = data[index];
      if (character === "(") {
        floor++;
      } else if (character === ")") {
        floor--;
      }

      if (floor < 0) {
        return index + 1;
      }
    }

    throw new Error("Never went below 0");
  }

  test.each([
    ["(())", 0],
    ["()()", 0],
    ["(((", 3],
    ["(()(()(", 3],
    ["))(((((", 3],
    ["())", -1],
    ["))(", -1],
    [")))", -3],
    [")())())", -3],
    [null, 232], // The actual answer
  ])("Part 1: %s -> %i", (input, expected) => {
    const result = part1(input ? input : realData.join("\n"));
    expect(result).toBe(expected);
  });

  test.each([
    [")", 1],
    ["()())", 5],
    [null, 1783], // The actual answer
  ])("Part 2: %s -> %i", (input, expected) => {
    const result = part2(input ? input : realData.join("\n"));
    expect(result).toBe(expected);
  });
});
