using System;
using System.Collections.Generic;
using UnityEngine;
using BlueRaja;

// a* algorithm to find path between rooms
// https://dotnetcoretutorials.com/2020/07/25/a-search-pathfinding-algorithm-in-c/
// https://stackoverflow.com/questions/2138642/how-to-implement-an-a-algorithm
public class Pathfinder
{
  public class Node
  {
    public Vector2Int Position { get; private set; }
    public Node Previous { get; set; }
    public float Cost { get; set; }

    public Node(Vector2Int position)
    {
      this.Position = position;
    }
  }

  public struct PathCost
  {
    public bool isTraversable;
    public float cost;
  }

  static readonly Vector2Int[] neighbors = {
        new Vector2Int(1, 0),
        new Vector2Int(-1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(0, -1),
    };

  Grid<Node> grid;
  // HashSet<Node> visited;

  public Pathfinder(Vector2Int size)
  {
    grid = new Grid<Node>(size, Vector2Int.zero);

    // queue = new SimplePriorityQueue<Node, float>();
    // visited = new HashSet<Node>();

    for (int x = 0; x < size.x; x++)
    {
      for (int y = 0; y < size.y; y++)
      {
        grid[x, y] = new Node(new Vector2Int(x, y));
      }
    }
  }

  void InitGrid()
  {
    var size = grid.Size;

    for (int x = 0; x < size.x; x++)
    {
      for (int y = 0; y < size.y; y++)
      {
        var node = grid[x, y];
        node.Previous = null;
        node.Cost = float.PositiveInfinity;
      }
    }
  }

  public List<Vector2Int> FindPath(Vector2Int start, Vector2Int end, Func<Node, Node, PathCost> costFunction)
  {
    InitGrid();
    SimplePriorityQueue<Node, float> queue = new SimplePriorityQueue<Node, float>();
    HashSet<Node> visited = new HashSet<Node>();

    grid[start].Cost = 0;
    queue.Enqueue(grid[start], 0);

    while (queue.Count > 0)
    {
      Node node = queue.Dequeue();
      visited.Add(node);

      if (node.Position == end)
      {
        return ConstructPath(node);
      }

      foreach (var offset in neighbors)
      {
        if (!grid.InBounds(node.Position + offset)) continue;
        var neighbor = grid[node.Position + offset];
        if (visited.Contains(neighbor)) continue;

        var pathCost = costFunction(node, neighbor);
        if (!pathCost.isTraversable) continue;

        float newCost = node.Cost + pathCost.cost;

        if (newCost < neighbor.Cost)
        {
          neighbor.Previous = node;
          neighbor.Cost = newCost;

          if (queue.TryGetPriority(node, out float existingPriority))
          {
            queue.UpdatePriority(node, newCost);
          }
          else
          {
            queue.Enqueue(neighbor, neighbor.Cost);
          }
        }
      }
    }

    return null;
  }

  List<Vector2Int> ConstructPath(Node node)
  {
    Stack<Vector2Int> currentPath = new Stack<Vector2Int>();
    List<Vector2Int> result = new List<Vector2Int>();

    while (node != null)
    {
      currentPath.Push(node.Position);
      node = node.Previous;
    }

    while (currentPath.Count > 0)
    {
      result.Add(currentPath.Pop());
    }

    return result;
  }
}
