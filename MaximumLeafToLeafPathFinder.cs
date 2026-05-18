using System;
using System.Collections.Generic;

public sealed class MaximumLeafToLeafPathFinder
{
    private int _maxSum;
    private readonly List<int> _maxPath = new();

    public MaxLeafToLeafResult Find(TreeNode root)
    {
        if (root is null)
        {
            throw new ArgumentNullException(nameof(root));
        }

        _maxSum = int.MinValue;
        _maxPath.Clear();

        CalculateMaxSumWithPath(root, new List<int>());

        if (_maxSum == int.MinValue)
        {
            throw new InvalidOperationException("The tree must contain at least two leaves to compute a leaf-to-leaf path.");
        }

        return new MaxLeafToLeafResult(_maxSum, _maxPath);
    }

    private int CalculateMaxSumWithPath(TreeNode? node, List<int> tempPath)
    {
        if (node is null)
        {
            return 0;
        }

        var leftPath = new List<int>();
        var rightPath = new List<int>();

        int leftSum = CalculateMaxSumWithPath(node.Left, leftPath);
        int rightSum = CalculateMaxSumWithPath(node.Right, rightPath);

        if (node.Left is not null && node.Right is not null)
        {
            int currentSum = leftSum + rightSum + node.Value;
            if (currentSum > _maxSum)
            {
                _maxSum = currentSum;
                _maxPath.Clear();
                _maxPath.AddRange(leftPath);
                _maxPath.Add(node.Value);
                rightPath.Reverse();
                _maxPath.AddRange(rightPath);
            }

            tempPath.AddRange(leftSum > rightSum ? leftPath : rightPath);
            tempPath.Add(node.Value);
            return Math.Max(leftSum, rightSum) + node.Value;
        }

        if (node.Left is null)
        {
            tempPath.AddRange(rightPath);
        }
        else
        {
            tempPath.AddRange(leftPath);
        }

        tempPath.Add(node.Value);
        return (node.Left is null ? rightSum : leftSum) + node.Value;
    }
}
