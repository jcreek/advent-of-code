const fs = require("fs");
const path = require("path");

describe("2024 Day 01", () => {
  let realData;

  beforeAll(() => {
    const filePath = path.resolve(__dirname, "./Data/Day01.dat");
    realData = fs.readFileSync(filePath, "utf-8").split("\n");
  });

  function parseInput(input) {
    const lines = input.split("\n");
    const left = [];
    const right = [];

    lines.forEach((line) => {
      const [leftString, rightString] = line.split(/\s+/); // Split by any whitespace
      left.push(parseInt(leftString, 10));
      right.push(parseInt(rightString, 10));
    });

    return { left, right };
  }

  function part1(input) {
    const { left, right } = parseInput(input || realData.join("\n"));
    let totalDistance = 0;

    left.sort((a, b) => a - b);
    right.sort((a, b) => a - b);

    for (let i = 0; i < left.length; i++) {
      totalDistance += Math.abs(left[i] - right[i]);
    }

    return totalDistance;
  }

  function part2(input) {
    const { left, right } = parseInput(input || realData.join("\n"));
    let similarityScore = 0;

    left.forEach((value) => {
      const count = right.filter((x) => x === value).length;
      similarityScore += value * count;
    });

    return similarityScore;
  }

  test.each([
    [
      `3   4
4   3
2   5
1   3
3   9
3   3`,
      11,
    ],
    [null, 2000468], // The actual answer
  ])("Part 1: %s -> %i", (input, expected) => {
    const result = part1(input ? input : realData.join("\n"));
    expect(result).toBe(expected);
  });

  test.each([
    [
      `3   4
4   3
2   5
1   3
3   9
3   3`,
      31,
    ],
    [null, 18567089], // The actual answer
  ])("Part 2: %s -> %i", (input, expected) => {
    const result = part2(input ? input : realData.join("\n"));
    expect(result).toBe(expected);
  });
});
