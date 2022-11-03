using System;
using System.Collections.Generic;
using UnityEngine;
using Graphs;

// Prim's algorithm for minimum spanning tree
// https://www.geeksforgeeks.org/prims-algorithm-using-priority_queue-stl/
public static class Prim
{
  public class Edge : Graphs.Edge
  {
    public float Distance { get; private set; }

    public Edge(Vertex u, Vertex v) : base(u, v)
    {
      Distance = Vector3.Distance(u.Position, v.Position);
    }

    public static bool operator ==(Edge left, Edge right)
    {
      return (left.U == right.U && left.V == right.V)
          || (left.U == right.V && left.V == right.U);
    }

    public static bool operator !=(Edge left, Edge right)
    {
      return !(left == right);
    }

    public override bool Equals(object obj)
    {
      if (obj is Edge e)
      {
        return this == e;
      }

      return false;
    }

    public bool Equals(Edge e)
    {
      return this == e;
    }

    public override int GetHashCode()
    {
      return U.GetHashCode() ^ V.GetHashCode();
    }
  }

  public static List<Edge> MST(List<Edge> edges, Vertex start)
  {
    HashSet<Vertex> unvisited = new HashSet<Vertex>();
    HashSet<Vertex> visited = new HashSet<Vertex>();

    foreach (var edge in edges)
    {
      unvisited.Add(edge.U);
      unvisited.Add(edge.V);
    }

    visited.Add(start);

    List<Edge> results = new List<Edge>();

    while (unvisited.Count > 0)
    {
      bool chosen = false;
      Edge chosenEdge = null;

      float minWeight = float.PositiveInfinity;

      foreach (var edge in edges)
      {
        int closedVertices = 0;
        if (!visited.Contains(edge.U)) closedVertices++;
        if (!visited.Contains(edge.V)) closedVertices++;
        if (closedVertices != 1) continue;

        if (edge.Distance < minWeight)
        {
          chosenEdge = edge;
          chosen = true;
          minWeight = edge.Distance;
        }
      }

      if (!chosen) break;


      results.Add(chosenEdge);
      unvisited.Remove(chosenEdge.U);
      unvisited.Remove(chosenEdge.V);
      visited.Add(chosenEdge.U);
      visited.Add(chosenEdge.V);
    }

    return results;
  }
}
