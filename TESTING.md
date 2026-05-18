# Assignment 2B — Testing Document

## 1. Program Overview

**Project:** Assignment 2B  
**Language:** C# / .NET 8.0  
**Test Framework:** MSTest 3.6.4 (`Microsoft.VisualStudio.TestTools.UnitTesting`)  

### What the program does

This program solves the **maximum leaf-to-leaf path sum** problem on a binary tree.

A *leaf-to-leaf path* is any path that starts at one leaf node, travels up through zero or more internal nodes, and ends at another leaf node.  The program computes:

| Output field | Type | Description |
|---|---|---|
| `Sum` | `int` | The **maximum sum of all node values** along the highest-sum leaf-to-leaf path |
| `Path` | `IReadOnlyList<int>` | The ordered sequence of node values forming that path (left leaf → root → right leaf) |

### Core files

| File | Responsibility |
|---|---|
| `TreeNode.cs` | Immutable binary-tree node (`Value`, `Left`, `Right`, `IsLeaf` property) |
| `MaximumLeafToLeafPathFinder.cs` | Post-order DFS solver; tracks the global maximum sum and the path that achieves it in one traversal |
| `MaxLeafToLeafResult.cs` | Value object holding the `Sum` and `Path` returned by the solver |
| `Program.cs` | Builds the demo tree and prints the result |

### Algorithm (O(n))

The solver performs a single post-order DFS traversal:

1. Recurse into the left and right subtrees, receiving each subtree's *max leaf-to-any-descendant sum* and *the path that achieves it*.
2. At a node that has **both** children, the leaf-to-leaf candidate through that node is:
   ```csharp
   candidateSum = leftSum + rightSum + node.Value
   candidatePath = leftPath + [node.Value] + reversed(rightPath)
   ```
   This candidate is compared against the running global maximum.
3. For nodes with **only one** side, the "heavier" subtree is propagated upward along with the current node's value.
4. At the end, if no valid leaf-to-leaf path was ever found (e.g., the tree has fewer than two leaves), an `InvalidOperationException` is thrown.

---

## 2. Root Cause Summary of Found and Fixed Issues

### Issue 1 — Aggressive SDK glob compiled stray source files into the wrong projects

**Symptoms:** Every build (both `Assignment2_B.csproj` and `Assignment2_B.Tests.csproj`) failed with dozens of compiler errors such as:

```
error CS0246: The type or namespace name 'TestClassAttribute' could not be found
error CS0246: The type or namespace name 'TestMethodAttribute' could not be found
error CS0246: The type or namespace name 'ParallelizeAttribute' could not be found
```

**Root cause:** The .NET SDK's default `**/*.cs` implicit glob included *all* `.cs` files inside the project folder — including files from co-located but unrelated projects (`TempTestProj/`) and the sibling test project (`Assignment2_B.Tests/`). MVC Test attributes that exist in MSTest 3.x were resolved against the main project's MSTest 3.6.4 reference, causing type-not-found errors for the stray files.

**Fix:** Added `<EnableDefaultCompileItems>false</EnableDefaultCompileItems>` to both `Assignment2_B.csproj` and `Assignment2_B.Tests.csproj`, then explicitly listed only the intended source files in `<Compile Include="…" />` item groups.

### Issue 2 — `CollectionAssert.AreEqual` overload mismatch with MSTest 3.x

**Symptoms:** After fixing Issue 1, two test methods still failed to compile:

```
error CS1503: Argument 2: cannot convert from
  'System.Collections.Generic.IReadOnlyList<int>' to 'System.Collections.ICollection?'
```

**Root cause:** MSTest 3.0 tightened the `CollectionAssert.AreEqual` overload to accept `System.Collections.ICollection?` (the non-generic collection interface). The return type of `MaxLeafToLeafResult.Path` is `IReadOnlyList<int>`, which implements `IReadOnlyCollection<int>` but does **not** implicitly implement the non-generic `ICollection`. This overload mismatch did not exist in MSTest 2.x.

**Fix:** Changed both assertion calls to pass `List<int>` arguments instead:

```csharp
// Before (MSTest 2.x — compiled with MSTest 3.x)
CollectionAssert.AreEqual(new[] {2, 1, 3}, result.Path);

// After (works with MSTest 3.x)
CollectionAssert.AreEqual(new[] {2, 1, 3}.ToList(), result.Path.ToList());
```

Both `.ToList()` calls produce `List<int>`, which implements `System.Collections.ICollection` explicitly and satisfies the MSTest 3.x overload.

---

## 3. Test Suite

### Test methods (`Assignment2_B.Tests/UnitTest1.cs`)

| # | Test method | Scenario | Validates |
|---|---|---|---|
| 1 | `Find_ReturnsExpectedSumAndPath_ForSimpleBalancedTree` | Tree: `(1 ↔ 2, 3)` | Correct sum = `2+1+3 = 6`; correct path `[2,1,3]` |
| 2 | `Find_ReturnsExpectedSumAndPath_WhenTreeContainsNegativeValues` | Tree: `5 ↔ 2(–1,1), 3` | Correct sum = `1+2+5+3 = 11`; correct path `[1,2,5,3]` |
| 3 | `Find_ThrowsArgumentNullException_WhenRootIsNull` | `Find(null)` | Guards against null input |
| 4 | `Find_ThrowsInvalidOperationException_WhenTreeDoesNotContainTwoLeaves` | Single-branch tree `1 ← 2` | Detects trees that cannot form a leaf-to-leaf path |

### Running the tests

```bash
# Build and run all tests (from the solution directory)
dotnet test Assignment2_B.Tests\Assignment2_B.Tests.csproj

# Build and run the main program
dotnet run --project Assignment2_B.csproj
```

### Current test results

```
Test run ...
Passed: 4 / 4  |  Duration: 48 ms  |  Failed: 0
```

### Demo program output

```
Maximum Sum Between Two Leaves: 27
Path with Maximum Sum: 3 -> 6 -> 9 -> 0 -> -1 -> 10
```

The demo tree (constructed in `Program.cs`) has five leaves: `{–8, 6, 1, 3, –1}`.  
The highest-sum leaf-to-leaf path is:

```
3 --6--> 6 --9--> 9 --0--> 0 --10--> –1
sum = 3 + 6 + 9 + 0 + (–1) + 10 = 27
```

All four unit tests pass cleanly against this implementation.
