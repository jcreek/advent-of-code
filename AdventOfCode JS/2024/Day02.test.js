const fs = require("fs");
const path = require("path");

describe("2024 Day 02", () => {
  let realData;

  beforeAll(() => {
    const filePath = path.resolve(__dirname, "./Data/Day02.dat");
    realData = fs.readFileSync(filePath, "utf-8").split("\n");
  });

  function areNumbersAllIncreasingOrDecreasing(numbers) {
    if (numbers.length <= 1) {
      // If there's only one number, it's either increasing or decreasing
      return true;
    }

    let isIncreasing = true;
    let isDecreasing = true;

    for (let i = 1; i < numbers.length; i++) {
      if (numbers[i] > numbers[i - 1]) {
        isDecreasing = false;
      } else if (numbers[i] < numbers[i - 1]) {
        isIncreasing = false;
      }

      if (!isIncreasing && !isDecreasing) {
        return false;
      }
    }

    return true;
  }

  function areAdjacentNumbersWithinRange(numbers) {
    for (let i = 1; i < numbers.length; i++) {
      const diff = Math.abs(numbers[i] - numbers[i - 1]);
      if (diff < 1 || diff > 3) {
        return false;
      }
    }
    return true;
  }

  function isReportSafe(report) {
    const numbers = report.split(" ").map((x) => parseInt(x, 10));

    if (!areNumbersAllIncreasingOrDecreasing(numbers)) {
      return false;
    }

    if (!areAdjacentNumbersWithinRange(numbers)) {
      return false;
    }

    return true;
  }

  function isReportSafeAllowRemovingOneNumber(report) {
    const numbers = report.split(" ").map((x) => parseInt(x, 10));

    function isSafe(numbers) {
      return (
        areNumbersAllIncreasingOrDecreasing(numbers) &&
        areAdjacentNumbersWithinRange(numbers)
      );
    }

    if (isSafe(numbers)) {
      return true;
    }

    // Try removing each number and check if the resulting array is safe
    for (let i = 0; i < numbers.length; i++) {
      const modifiedNumbers = [...numbers.slice(0, i), ...numbers.slice(i + 1)];
      if (isSafe(modifiedNumbers)) {
        return true;
      }
    }

    // If no single removal makes the report safe, return false
    return false;
  }

  function part1(input) {
    // In the input, each line is a report
    const reports = input.split("\n");
    // Each report is a string of numbers separated by spaces

    let amountOfSafeReports = 0;

    reports.forEach((report) => {
      if (isReportSafe(report)) {
        amountOfSafeReports += 1;
      }
    });

    return amountOfSafeReports;
  }

  function part2(input) {
    // In the input, each line is a report
    const reports = input.split("\n");
    // Each report is a string of numbers separated by spaces

    let amountOfSafeReports = 0;

    reports.forEach((report) => {
      if (isReportSafeAllowRemovingOneNumber(report)) {
        amountOfSafeReports += 1;
      }
    });

    return amountOfSafeReports;
  }

  test.each([
    [
      `7 6 4 2 1
  1 2 7 8 9
  9 7 6 2 1
  1 3 2 4 5
  8 6 4 4 1
  1 3 6 7 9`,
      2,
    ],
    [null, 585], // The actual answer
  ])("Part 1: %s -> %i", (input, expected) => {
    const result = part1(input ? input : realData.join("\n"));
    expect(result).toBe(expected);
  });

  test.each([
    [
      `7 6 4 2 1
1 2 7 8 9
9 7 6 2 1
1 3 2 4 5
8 6 4 4 1
1 3 6 7 9`,
      4,
    ],
    [null, 626], // The actual answer
  ])("Part 2: %s -> %i", (input, expected) => {
    const result = part2(input ? input : realData.join("\n"));
    expect(result).toBe(expected);
  });
});
