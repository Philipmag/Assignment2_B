# 🌳 Maximum Leaf-to-Leaf Path Finder

> Finds the maximum-sum path between any two leaf nodes in a binary tree — with full path reconstruction.

[![C#](https://img.shields.io/badge/C%23-.NET%208-239120?style=flat-square&logo=c-sharp&logoColor=white)](https://dotnet.microsoft.com)
[![Tests](https://img.shields.io/badge/Tests-Passing-brightgreen?style=flat-square)](https://github.com/Philipmag/Assignment2_B)
[![Status](https://img.shields.io/badge/Status-Complete-blue?style=flat-square)](https://github.com/Philipmag/Assignment2_B)

---

## Overview

A classic tree algorithm problem taken beyond the textbook: given a binary tree with positive and negative integer values, find the path between any two leaf nodes that produces the maximum sum — and return both the sum and the exact sequence of nodes in the path.

Built for a Data Structures & Algorithms course (COIS 2240) at Trent University. The implementation uses a single-pass depth-first search that tracks both the optimal path and its sum simultaneously, avoiding the need for a second traversal.

---

## Demo

**Input tree:**
```
         -15
        /    \
       5      6
      / \    / \
    -8   1  3   9
    / \          \
   2   6          0
                 / \
                4   10
                      \
                      -1
```

**Output:**
```
Maximum Sum Between Two Leaves: 27
Path with Maximum Sum: 6 → -8 → 5 → -15 → 6 → 9 → 0 → 10 → -1
```

---

## Features

- **Single-pass DFS** — Computes the maximum sum and reconstructs the full path in one traversal, O(n) time complexity.
- **Handles negative values** — Correctly handles trees with negative node values, where the optimal path is not always the longest.
- **Full path reconstruction** — Returns the exact sequence of node values from leaf to leaf, not just the sum.
- **Comprehensive test suite** — Covers edge cases including single-node trees, all-negative trees, and unbalanced trees.

---

## Tech Stack

| Layer | Technology |
|-------|------------|
| Language | C# (.NET 8) |
| Testing | MSTest |
| Build | .NET CLI / Visual Studio |

---

## Getting Started

```bash
git clone https://github.com/Philipmag/Assignment2_B
cd Assignment2_B
dotnet build
dotnet run
```

To run the test suite:
```bash
dotnet test Assignment2_B.Tests/
```

---

## How It Works

The algorithm uses a recursive DFS that, at each node, does two things simultaneously:

1. **Computes the max-sum path through the current node** (left subtree → node → right subtree) and updates the global best if it's a new maximum.
2. **Returns the best single-branch path** upward to the parent, so the parent can use it to evaluate its own cross-paths.

The key insight is that a leaf-to-leaf path must pass through exactly one node that has both a left and right child — that node is the "apex" of the path. The DFS identifies all such apex candidates in a single pass.

---

## What I Learned

- **Tracking state through recursion** — passing mutable path lists down and up the call stack requires careful handling to avoid aliasing bugs; learned to create new list copies at each branch.
- **The difference between "path through a node" and "path from a node"** — this distinction is what makes the single-pass approach work; it took several failed attempts before the mental model clicked.
- **Writing tests for tree algorithms** — manually constructing test trees as code is tedious but essential; discovered two edge-case bugs through testing that weren't caught by the main example.

---

## Roadmap

- [ ] Extend to support `k`-ary trees (not just binary).
- [ ] Add a visual tree renderer to display the tree structure and highlighted path in the console output.
