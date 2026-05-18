using System;
using System.Collections.Generic;

public sealed class MaxLeafToLeafResult
{
    public int Sum { get; }
    public IReadOnlyList<int> Path { get; }

    public MaxLeafToLeafResult(int sum, IReadOnlyList<int> path)
    {
        Sum = sum;
        Path = path ?? throw new ArgumentNullException(nameof(path));
    }
}
