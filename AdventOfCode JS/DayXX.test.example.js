// const fs = require("fs");
// const path = require("path");

// describe("2024 Day 01", () => {
//   let realData;

//   beforeAll(() => {
//     const filePath = path.resolve(__dirname, "./Data/Day01.dat");
//     realData = fs.readFileSync(filePath, "utf-8").split("\n");
//   });

//   function part1(input) {
//     const data = input ? input.split("\n") : realData;
//     // Replace this with actual logic for Part 1
//     return input ? 0 : 232; // Example answer
//   }

//   function part2(input) {
//     const data = input ? input.split("\n") : realData;
//     // Replace this with actual logic for Part 2
//     return input ? 1 : 1783; // Example answer
//   }

//   test.each([
//     ["blah", 0],
//     [null, 232], // The actual answer
//   ])("Part 1: %s -> %i", (input, expected) => {
//     const result = part1(input ? input : realData.join("\n"));
//     expect(result).toBe(expected);
//   });

//   test.each([
//     ["blah", 1],
//     [null, 1783], // The actual answer
//   ])("Part 2: %s -> %i", (input, expected) => {
//     const result = part2(input ? input : realData.join("\n"));
//     expect(result).toBe(expected);
//   });
// });
