using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Assignment2_B.Tests;

[TestClass]
public class MaximumLeafToLeafPathFinderTests
{
    [TestMethod]
    public void Find_ReturnsExpectedSumAndPath_ForSimpleBalancedTree()
    {
        var root = new TreeNode(1)
        {
            Left = new TreeNode(2),
            Right = new TreeNode(3)
        };

        var solver = new MaximumLeafToLeafPathFinder();
        MaxLeafToLeafResult result = solver.Find(root);

        Assert.AreEqual(6, result.Sum);
        CollectionAssert.AreEqual(new[] {2, 1, 3}.ToList(), result.Path.ToList());
    }

    [TestMethod]
    public void Find_ReturnsExpectedSumAndPath_WhenTreeContainsNegativeValues()
    {
        var root = new TreeNode(5)
        {
            Left = new TreeNode(2)
            {
                Left = new TreeNode(-1),
                Right = new TreeNode(1)
            },
            Right = new TreeNode(3)
        };

        var solver = new MaximumLeafToLeafPathFinder();
        MaxLeafToLeafResult result = solver.Find(root);

        Assert.AreEqual(11, result.Sum);
        CollectionAssert.AreEqual(new[] {1, 2, 5, 3}.ToList(), result.Path.ToList());
    }

    [TestMethod]
    public void Find_ThrowsArgumentNullException_WhenRootIsNull()
    {
        var solver = new MaximumLeafToLeafPathFinder();

        Assert.ThrowsException<ArgumentNullException>(() => solver.Find(null!));
    }

    [TestMethod]
    public void Find_ThrowsInvalidOperationException_WhenTreeDoesNotContainTwoLeaves()
    {
        var root = new TreeNode(1)
        {
            Left = new TreeNode(2)
        };

        var solver = new MaximumLeafToLeafPathFinder();

        Assert.ThrowsException<InvalidOperationException>(() => solver.Find(root));
    }
}
