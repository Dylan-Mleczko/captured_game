using System;
using System.Collections.Generic;
using UnityEngine;
using Graphs;


// Bowyer-Watson Algorithm for Delaunator Triangulation 
// https://gorillasun.de/blog/bowyer-watson-algorithm-for-Delaunator-triangulation
public class Delaunator
{
  public class Triangle : IEquatable<Triangle>
  {
    public Vertex A { get; set; }
    public Vertex B { get; set; }
    public Vertex C { get; set; }
    public bool IsDuplicate { get; set; }


    public Triangle(Vertex a, Vertex b, Vertex c)
    {
      A = a;
      B = b;
      C = c;
    }

    public bool IsInTriangle(Vector3 v)
    {
      return Vector3.Distance(v, A.Position) < 0.01f
          || Vector3.Distance(v, B.Position) < 0.01f
          || Vector3.Distance(v, C.Position) < 0.01f;
    }

    public bool inCircumCircle(Vector3 v)
    {
      Vector3 a = A.Position;
      Vector3 b = B.Position;
      Vector3 c = C.Position;

      float circumX = CalcCircumCirc(true);
      float circumY = CalcCircumCirc(false);
      Vector3 circum = new Vector3(circumX / 2, circumY / 2);
      float circumRadius = Vector3.SqrMagnitude(a - circum);
      float distance = Vector3.SqrMagnitude(v - circum);
      return distance <= circumRadius;
    }

    private float CalcCircumCirc(bool isX)
    {
      float a;
      float b;
      float c;
      float opA;
      float opB;
      float opC;
      if (isX)
      {
        a = this.A.Position.y;
        b = this.B.Position.y;
        c = this.C.Position.y;
        opA = this.A.Position.x;
        opB = this.B.Position.x;
        opC = this.C.Position.x;

      }
      else
      {
        a = this.A.Position.x;
        b = this.B.Position.x;
        c = this.C.Position.x;
        opA = this.A.Position.y;
        opB = this.B.Position.y;
        opC = this.C.Position.y;
      }
      float aSqr = A.Position.sqrMagnitude;
      float bSqr = B.Position.sqrMagnitude;
      float cSqr = C.Position.sqrMagnitude;

      return (aSqr * (c - b) + bSqr * (a - c) + cSqr * (b - a)) / (opA * (c - b) + opB * (a - c) + opC * (b - a));
    }

    public static bool operator ==(Triangle t1, Triangle t2)
    {
      return (t1.A == t2.A || t1.A == t2.B || t1.A == t2.C)
          && (t1.B == t2.A || t1.B == t2.B || t1.B == t2.C)
          && (t1.C == t2.A || t1.C == t2.B || t1.C == t2.C);
    }

    public static bool operator !=(Triangle t1, Triangle t2)
    {
      return !(t1 == t2);
    }

    public override bool Equals(object obj)
    {
      if (obj is Triangle t)
      {
        return this == t;
      }

      return false;
    }

    public bool Equals(Triangle t)
    {
      return this == t;
    }

  }

  public class Edge
  {
    public Vertex U { get; set; }
    public Vertex V { get; set; }
    public bool IsDuplicate { get; set; }


    public Edge(Vertex u, Vertex v)
    {
      this.U = u;
      this.V = v;
    }

    public static bool operator ==(Edge left, Edge right)
    {
      return (left.U == right.U || left.U == right.V)
          && (left.V == right.U || left.V == right.V);
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

    public static bool Equal(Edge left, Edge right)
    {
      return Delaunator.Equal(left.U, right.U) && Delaunator.Equal(left.V, right.V)
          || Delaunator.Equal(left.U, right.V) && Delaunator.Equal(left.V, right.U);
    }
  }

  static bool Equal(float x, float y)
  {
    return Mathf.Abs(x - y) <= float.Epsilon * Mathf.Abs(x + y) * 2
        || Mathf.Abs(x - y) < float.MinValue;
  }

  static bool Equal(Vertex left, Vertex right)
  {
    return Equal(left.Position.x, right.Position.x) && Equal(left.Position.y, right.Position.y);
  }

  public List<Vertex> Vertices { get; private set; }
  public List<Edge> Edges { get; private set; }
  public List<Triangle> Triangles { get; private set; }

  Delaunator()
  {
    Edges = new List<Edge>();
    Triangles = new List<Triangle>();
  }

  public static Delaunator Triangulate(List<Vertex> vertices)
  {
    Delaunator Delaunator = new Delaunator();
    Delaunator.Vertices = new List<Vertex>(vertices);
    Delaunator.Triangulate();

    return Delaunator;
  }

  void Triangulate()
  {
    float minX = Vertices[0].Position.x;
    float minY = Vertices[0].Position.y;
    float maxX = minX;
    float maxY = minY;

    foreach (var vertex in Vertices)
    {
      if (vertex.Position.x < minX) minX = vertex.Position.x;
      if (vertex.Position.x > maxX) maxX = vertex.Position.x;
      if (vertex.Position.y < minY) minY = vertex.Position.y;
      if (vertex.Position.y > maxY) maxY = vertex.Position.y;
    }

    float dx = maxX - minX;
    float dy = maxY - minY;
    float deltaMax = Mathf.Max(dx, dy) * 2;

    Vertex p1 = new Vertex(new Vector2(minX - 1, minY - 1));
    Vertex p2 = new Vertex(new Vector2(minX - 1, maxY + deltaMax));
    Vertex p3 = new Vertex(new Vector2(maxX + deltaMax, minY - 1));

    Triangles.Add(new Triangle(p1, p2, p3));

    foreach (var vertex in Vertices)
    {
      List<Edge> triangleMesh = new List<Edge>();

      foreach (var t in Triangles)
      {
        if (t.inCircumCircle(vertex.Position))
        {
          t.IsDuplicate = true;
          triangleMesh.Add(new Edge(t.A, t.B));
          triangleMesh.Add(new Edge(t.B, t.C));
          triangleMesh.Add(new Edge(t.C, t.A));
        }
      }

      Triangles.RemoveAll((Triangle t) => t.IsDuplicate);

      for (int i = 0; i < triangleMesh.Count; i++)
      {
        for (int j = i + 1; j < triangleMesh.Count; j++)
        {
          if (Edge.Equal(triangleMesh[i], triangleMesh[j]))
          {
            triangleMesh[i].IsDuplicate = true;
            triangleMesh[j].IsDuplicate = true;
          }
        }
      }

      triangleMesh.RemoveAll((Edge e) => e.IsDuplicate);

      foreach (var edge in triangleMesh)
      {
        Triangles.Add(new Triangle(edge.U, edge.V, vertex));
      }
    }

    Triangles.RemoveAll((Triangle t) => t.IsInTriangle(p1.Position) ||
    t.IsInTriangle(p2.Position) ||
    t.IsInTriangle(p3.Position));


    // avoid duplication of adding same edges
    HashSet<Edge> edgeSet = new HashSet<Edge>();

    foreach (var t in Triangles)
    {
      var ab = new Edge(t.A, t.B);
      var bc = new Edge(t.B, t.C);
      var ca = new Edge(t.C, t.A);

      // See if edge is unique using HashSet return
      // HashSet return true if the element is present
      if (edgeSet.Add(ab))
      {
        Edges.Add(ab);
      }

      if (edgeSet.Add(bc))
      {
        Edges.Add(bc);
      }

      if (edgeSet.Add(ca))
      {
        Edges.Add(ca);
      }
    }
  }
}
