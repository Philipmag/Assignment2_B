using System;

internal sealed class Program
{
    private static void Main(string[] args)
    {
        var root = new TreeNode(-15)
        {
            Left = new TreeNode(5)
            {
                Left = new TreeNode(-8)
                {
                    Left = new TreeNode(2),
                    Right = new TreeNode(6)
                },
                Right = new TreeNode(1)
            },
            Right = new TreeNode(6)
            {
                Left = new TreeNode(3),
                Right = new TreeNode(9)
                {
                    Right = new TreeNode(0)
                    {
                        Left = new TreeNode(4),
                        Right = new TreeNode(10)
                        {
                            Right = new TreeNode(-1)
                        }
                    }
                }
            }
        };

        var solver = new MaximumLeafToLeafPathFinder();
        MaxLeafToLeafResult result = solver.Find(root);

        Console.WriteLine($"Maximum Sum Between Two Leaves: {result.Sum}");
        Console.WriteLine($"Path with Maximum Sum: {string.Join(" -> ", result.Path)}");
    }
}
