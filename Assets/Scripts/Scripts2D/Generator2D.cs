using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using Graphs;

public class Generator2D : MonoBehaviour
{
  enum CellType
  {
    None,
    Room,
    Hallway
  }

  class Room
  {
    public RectInt bounds;
    public GameObject[] cells;
    public bool[][] statusList;

    public Room(Vector2Int location, Vector2Int size)
    {
      bounds = new RectInt(location, size);
      cells = new GameObject[size.x * size.y];
    }

    public static bool Intersect(Room a, Room b)
    {
      return !((a.bounds.position.x >= (b.bounds.position.x + b.bounds.size.x)) || ((a.bounds.position.x + a.bounds.size.x) <= b.bounds.position.x)
          || (a.bounds.position.y >= (b.bounds.position.y + b.bounds.size.y)) || ((a.bounds.position.y + a.bounds.size.y) <= b.bounds.position.y));
    }

    public void UpdateRoomCell(int index)
    {
      this.cells[index].GetComponent<CellBehavior>().UpdateCell(this.statusList[index]);
    }
  }

  class Hallway
  {
    public RectInt bounds;

    public GameObject cell;

    public Vector2Int location;
    public bool[] status = { true, true, true, true };
    public Hallway(Vector2Int location, Vector2Int size)
    {
      bounds = new RectInt(location, size);
      this.location = location;
    }

    public bool[] GetIntersectStatus(Hallway neighbor)
    {
      if (Mathf.Abs(this.bounds.position.x - neighbor.bounds.position.x) <= this.bounds.size.x && Mathf.Abs(this.bounds.position.y - neighbor.bounds.position.y) <= float.Epsilon)
      {
        if (this.bounds.position.x - neighbor.bounds.position.x > 0)
        {
          this.status[2] = false;
        }
        else
        {
          this.status[0] = false;
        }
      }
      else if (Mathf.Abs(this.bounds.position.y - neighbor.bounds.position.y) <= this.bounds.size.y && Mathf.Abs(this.bounds.position.x - neighbor.bounds.position.x) <= float.Epsilon)
      {
        if (this.bounds.position.y - neighbor.bounds.position.y > 0)
        {
          this.status[1] = false;
        }
        else
        {
          this.status[3] = false;
        }
      }

      return this.status;

    }

    public void UpdateGameObjectStatus()
    {
      cell.GetComponent<CellBehavior>().UpdateCell(this.status);
    }


  }

  [SerializeField]Vector2Int size;
  [SerializeField]int roomCount;
  [SerializeField]Vector2Int roomMaxSize;
  [SerializeField]GameObject cubePrefab;
  [SerializeField]GameObject roomPrefab;
  [SerializeField]GameObject hallwayPrefab;
  [SerializeField]GameObject pillarPrefab;
  [SerializeField]Material redMaterial;
  [SerializeField]Material blueMaterial;

  Random random;
  Grid2D<CellType> grid;
  List<Room> rooms;

  List<Hallway> hallwayCells;
  Delaunay2D delaunay;
  HashSet<Prim.Edge> selectedEdges;

  void Start()
  {
    Generate();
  }

  void Generate()
  {
    random = new Random(0);
    grid = new Grid2D<CellType>(size, Vector2Int.zero);
    rooms = new List<Room>();
    hallwayCells = new List<Hallway>();

    PlaceRooms();
    Triangulate();
    CreateHallways();
    PathfindHallways();
    foreach (Hallway cell in hallwayCells)
    {
      Debug.Log(cell.location.ToString());
    }
  }

  void PlaceRooms()
  {
    for (int i = 0; i < roomCount; i++)
    {
      int attempts = 0;
      while (attempts < 5)
      {
        attempts++;
        Vector2Int location = new Vector2Int(
            random.Next(0, size.x),
            random.Next(0, size.y)
        );

        Vector2Int roomSize = new Vector2Int(
            random.Next(1, roomMaxSize.x + 1),
            random.Next(1, roomMaxSize.y + 1)
        );

        bool add = true;
        Room newRoom = new Room(location, roomSize);
        Room buffer = new Room(location + new Vector2Int(-1, -1), roomSize + new Vector2Int(2, 2));

        foreach (var room in rooms)
        {
          if (Room.Intersect(room, buffer))
          {
            add = false;
            break;
          }
        }

        if (newRoom.bounds.xMin < 0 || newRoom.bounds.xMax >= size.x
            || newRoom.bounds.yMin < 0 || newRoom.bounds.yMax >= size.y)
        {
          add = false;
        }

        if (add)
        {
          rooms.Add(newRoom);
          PlaceRoom(newRoom.bounds.position, newRoom.bounds.size, newRoom);

          foreach (var pos in newRoom.bounds.allPositionsWithin)
          {
            grid[pos] = CellType.Room;
          }
          break;
        }
      }
    }
  }

  void Triangulate()
  {
    List<Vertex> vertices = new List<Vertex>();

    foreach (var room in rooms)
    {
      vertices.Add(new Vertex<Room>((Vector2)room.bounds.position + ((Vector2)room.bounds.size) / 2, room));
    }

    delaunay = Delaunay2D.Triangulate(vertices);
  }

  void CreateHallways()
  {
    List<Prim.Edge> edges = new List<Prim.Edge>();

    foreach (var edge in delaunay.Edges)
    {
      edges.Add(new Prim.Edge(edge.U, edge.V));
    }

    List<Prim.Edge> mst = Prim.MinimumSpanningTree(edges, edges[0].U);

    selectedEdges = new HashSet<Prim.Edge>(mst);
    var remainingEdges = new HashSet<Prim.Edge>(edges);
    remainingEdges.ExceptWith(selectedEdges);

    foreach (var edge in remainingEdges)
    {
      if (random.NextDouble() < 0.125)
      {
        selectedEdges.Add(edge);
      }
    }
  }

  void PathfindHallways()
  {
    DungeonPathfinder2D aStar = new DungeonPathfinder2D(size);

    foreach (var edge in selectedEdges)
    {
      var startRoom = (edge.U as Vertex<Room>).Item;
      var endRoom = (edge.V as Vertex<Room>).Item;

      var startPosf = startRoom.bounds.center;
      var endPosf = endRoom.bounds.center;
      var startPos = new Vector2Int((int)startPosf.x, (int)startPosf.y);
      var endPos = new Vector2Int((int)endPosf.x, (int)endPosf.y);

      var path = aStar.FindPath(startPos, endPos, (DungeonPathfinder2D.Node a, DungeonPathfinder2D.Node b) =>
      {
        var pathCost = new DungeonPathfinder2D.PathCost();

        pathCost.cost = Vector2Int.Distance(b.Position, endPos);    //heuristic
        pathCost.cost += Mathf.Min(Mathf.Abs(b.Position.x - startPos.x), Mathf.Abs(b.Position.x - endPos.x));
        pathCost.cost += Mathf.Min(Mathf.Abs(b.Position.y - startPos.y), Mathf.Abs(b.Position.y - endPos.y));

        if (grid[b.Position] == CellType.Room)
        {
          pathCost.cost += 10;
        }
        else if (grid[b.Position] == CellType.None)
        {
          pathCost.cost += 5;
        }
        else if (grid[b.Position] == CellType.Hallway)
        {
          pathCost.cost += 1;
        }

        pathCost.traversable = true;

        return pathCost;
      });

      if (path != null)
      {
        for (int i = 0; i < path.Count; i++)
        {
          var current = path[i];

          if (grid[current] == CellType.None)
          {
            grid[current] = CellType.Hallway;
          }

          if (i > 0)
          {
            var prev = path[i - 1];
            var delta = current - prev;
          }
        }

        Hallway preHallway = null;
        Hallway curHallway = null;

        var started = false;
        foreach (var pos in path)
        {
          if (grid[pos] == CellType.Hallway)
          {

            // curHallway = hallwayCells.Find(x => x.location.x == pos.x && x.location.y == pos.y);
            curHallway = hallwayCells.Find(x => x.location == pos);
            if (curHallway == null)
            {
              curHallway = new Hallway(pos, new Vector2Int(1, 1));
              hallwayCells.Add(curHallway);
              curHallway.cell = PlaceHallway(pos);
            }
            if (preHallway != null)
            {
              curHallway.GetIntersectStatus(preHallway);
              preHallway.GetIntersectStatus(curHallway);
              preHallway.UpdateGameObjectStatus();
            }

            preHallway = curHallway;
            if (!started)
            {
              started = true;
              Intersect(curHallway, startRoom);
              curHallway.cell.name = "pathStart";
            }
          }
        }

        Intersect(curHallway, endRoom);
        curHallway.cell.name = "pathEnd";

        preHallway.UpdateGameObjectStatus();



      }
    }
  }

  GameObject PlaceCube(Vector2Int location)
  {
    GameObject go = Instantiate(roomPrefab, new Vector3(location.x, 0, location.y), Quaternion.identity);
    var prefabSize = getPrefabSize(go).size;
    go.GetComponent<Transform>().localScale = new Vector3(1 / prefabSize.x, 1 / prefabSize.y, 1 / prefabSize.z);
    return go;
  }

  void PlaceRoom(Vector2Int location, Vector2Int size, Room room)
  {
    GameObject[] objs = new GameObject[size.x * size.y];
    bool[][] statusList = new bool[size.x * size.y][];
    for (int i = 0; i < size.x; i++)
    {
      for (int j = 0; j < size.y; j++)
      {
        var obj = PlaceCube(new Vector2Int(location.x + i, location.y + j));
        objs[i * size.y + j] = obj;
        bool[] status = { false, false, false, false };

        if (i == 0) status[2] = true;
        if (i == size.x - 1) status[0] = true;
        if (j == 0) status[1] = true;
        if (j == size.y - 1) status[3] = true;

        obj.GetComponent<CellBehavior>().UpdateCell(status);
        obj.name = "room" + (i * size.y + j).ToString();
        statusList[i * size.y + j] = status;
      }
    }
    room.cells = objs;
    room.statusList = statusList;
  }

  GameObject PlaceHallway(Vector2Int location)
  {
    // PlaceCube(location, new Vector2Int(1, 1), blueMaterial);
    GameObject go = Instantiate(hallwayPrefab, new Vector3(location.x, 0, location.y), Quaternion.identity);

    // var size = getPrefabSize(go).size;
    var prefabBounds = getPrefabSize(go);
    var size = prefabBounds.size;
    var center = prefabBounds.center;
    // Debug.Log(bounds.ToString());
    // go.GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
    //(1,1,1)=localScale*(3,4,5) 
    // Debug.Log("before scaling: " + getPrefabSize(go).center);
    go.GetComponent<Transform>().localScale = new Vector3(1 / size.x, 1 / size.y, 1 / size.z);
    // Debug.Log("after scaling: " + getPrefabSize(go).center);
    // go.GetComponent<Transform>().position = center;
    return go;
  }

  // 
  void Intersect(Hallway hallwayCell, Room room)
  {
    var hallwayPositionX = hallwayCell.bounds.position.x;
    var hallwayPositionY = hallwayCell.bounds.position.y;
    var unitWidth = hallwayCell.bounds.size.x;
    var unitLength = hallwayCell.bounds.size.y;

    for (int i = 0; i < room.bounds.size.x * room.bounds.size.y; i++)
    {
      var roomPositionX = room.bounds.center.x - 0.5f * room.bounds.size.x + i / room.bounds.size.y;
      var roomPositionY = room.bounds.center.y - 0.5f * room.bounds.size.y + i % room.bounds.size.y;
      // Debug.Log("room size: " + room.bounds.size.x + ", " + room.bounds.size.y);
      // Debug.Log("cellname:" + hallwayCell.cell.name + ", roomname:" + room.cells[i].name + ", i:" + i + "pos:" + roomPositionX + ", " + roomPositionY);

      // hallway horizontal of room position
      if (Mathf.Abs(hallwayPositionX - roomPositionX) <= unitWidth && Mathf.Abs(hallwayPositionY - roomPositionY) <= float.Epsilon)
      {
        var right = hallwayPositionX < roomPositionX;
        var hallCell = right ? 0 : 2;
        var roomCell = right ? 2 : 0;

        hallwayCell.status[hallCell] = false;
        hallwayCell.UpdateGameObjectStatus();
        room.statusList[i][roomCell] = false;
        room.UpdateRoomCell(i);
        room.cells[i].name = "roomStart";
        break;
      }
      // hallway vertical of room position
      else if (Mathf.Abs(hallwayPositionY - roomPositionY) <= unitLength && Mathf.Abs(hallwayPositionX - roomPositionX) <= float.Epsilon)
      {
        var above = hallwayPositionY < roomPositionY;
        var hallCell = above ? 3 : 1;
        var roomCell = above ? 1 : 3;

        hallwayCell.status[hallCell] = false;
        hallwayCell.UpdateGameObjectStatus();
        room.statusList[i][roomCell] = false;
        room.UpdateRoomCell(i);
        room.cells[i].name = "roomStart";
        break;
      }
    }
  }

  Bounds getPrefabSize(GameObject obj)
  {
    Vector3 center = new Vector3(0, 0, 0);
    foreach (Transform child in obj.transform)
    {
      center += child.gameObject.GetComponent<Renderer>().bounds.center;
    }
    center /= obj.transform.childCount; //center is average center of children

    //Now you have a center, calculate the bounds by creating a zero sized 'Bounds', 
    Bounds bounds = new Bounds(center, Vector3.zero);
    foreach (Transform child in obj.transform)
    {
      bounds.Encapsulate(child.gameObject.GetComponent<Renderer>().bounds);
    }
    return bounds;

  }
}
