using System.Drawing;
using System;
namespace RoutePlanning;

public static class PathFinderTask
{
    private static int[] bestOrder;
    private static double minPathLength = double.MaxValue;

    public static int[] FindBestCheckpointsOrder(Point[] checkpoints)
    {
        bestOrder = MakeTrivialPermutation(checkpoints.Length);
        minPathLength = double.MaxValue;
        var currentOrder = new int[checkpoints.Length];
        currentOrder[0] = 0;
        MakePermutations(checkpoints, currentOrder, 1, 0.0);
        return bestOrder;
    }

    private static int[] MakeTrivialPermutation(int size)
    {
        var trivialOrder = new int[size];
        for (var i = 0; i < trivialOrder.Length; i++)
            trivialOrder[i] = i;
        return trivialOrder;
    }

    private static void MakePermutations(Point[] checkpoints, int[] currentOrder, int position, double currentPath)
    {
        if (position == currentOrder.Length)
        {
            if (currentPath < minPathLength)
            {
                minPathLength = currentPath;
                currentOrder.CopyTo(bestOrder, 0);
            }
            return;
        }

        for (var i = 1; i < currentOrder.Length; i++)
        {
            var found = false;
            for (var j = 0; j < position; j++)
            {
                if (currentOrder[j] == i)
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                var newPathLength = currentPath + GetDistance(checkpoints[currentOrder[position - 1]], checkpoints[i]);
                if (newPathLength >= minPathLength)
                    continue;
                currentOrder[position] = i;
                MakePermutations(checkpoints, currentOrder, position + 1, newPathLength);
            }
        }
    }

    private static double GetDistance(Point a, Point b)
    {
        var dx = a.X - b.X;
        var dy = a.Y - b.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }
}
