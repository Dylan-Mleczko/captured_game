using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using System.Linq;
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
    public Vector2Int location;
    public GameObject[] cells;
    public bool[][] statusList;

    public Room(Vector2Int location, Vector2Int size)
    {
      bounds = new RectInt(location, size);
      this.location = location;
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

  class Pillar
  {
    public Vector2 location;
    public Pillar(Vector2 location)
    {
      this.location = location;
    }

  }


  [SerializeField] Vector2Int size;
  [SerializeField] int roomCount;
  [SerializeField] Vector2Int roomMaxSize;
  // [SerializeField] GameObject cubePrefab;
  [SerializeField] GameObject roomPrefab;
  [SerializeField] GameObject hallwayPrefab;
  [SerializeField] GameObject pillarPrefab;
  [SerializeField] GameObject roomLightPrefab;
  [SerializeField] GameObject[] miniRooms;
  [SerializeField] GameObject key;
  [SerializeField] GameObject exitDoor;
  [SerializeField] GameObject RookPrefab;
  [SerializeField] GameObject KnightPrefab;
  // [SerializeField] GameObject door;



  Random random;
  Grid2D<CellType> grid;
  List<Room> rooms;

  List<Hallway> hallwayCells;
  List<Pillar> pillars;
  Delaunay2D delaunay;
  HashSet<Prim.Edge> selectedEdges;

  float ratio;

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
    pillars = new List<Pillar>();

    PlaceRooms();
    Triangulate();
    CreateHallways();
    PathfindHallways();
    PlaceExitDoor();
    PlaceEnemy();
    // Debug.Log(rooms[0].statusList.GetLength(0).ToString());
    // rooms[0].statusList.GetLength(0);

    // foreach (Hallway cell in hallwayCells)
    // {
    //   Debug.Log(cell.location.ToString());
    // }
  }

  void PlaceEnemy()
  {
    var rookIndex = random.Next(0, rooms.Count - 1);
    var knightIndex = random.Next(0, rooms.Count - 1);

    while (rooms[rookIndex].bounds.size.x <= 1)
    {
      rookIndex = random.Next(0, rooms.Count - 1);
    }

    var rookLocation = rooms[rookIndex].location;
    var rookRange = rooms[rookIndex].bounds.size.x;
    GameObject rook = Instantiate(RookPrefab, new Vector3(rookLocation.x, 0, rookLocation.y), Quaternion.identity);
    var prefabSize = getPrefabSize(rook).size;
    rook.GetComponent<Transform>().localScale = new Vector3(1 / (5 * prefabSize.x), 1 / (5 * prefabSize.y), 1 / (5 * prefabSize.z));
    // rook.transform.Rotate(-90.0f, 0.0f, 0.0f, Space.Self);
    rook.GetComponent<Rook>().UpdateRange(rookRange);

    Debug.Log(rookLocation.ToString());

    // var knightLocation = rooms[knightIndex].location;
    // GameObject knight = Instantiate(KnightPrefab, new Vector3(rookLocation.x, 0, rookLocation.y), Quaternion.identity);
    // prefabSize = knight.gameObject.GetComponent<Renderer>().bounds.size;
    // knight.GetComponent<Transform>().localScale = new Vector3(1 / prefabSize.x, 1 / prefabSize.y, 1 / prefabSize.z);
    // knight.transform.Rotate(-90.0f, 0.0f, 0.0f, Space.Self);



    // Debug.Log(knightLocation.ToString());


  }

  void PlaceRooms()
  {
    var isExit = false;
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
          if (rooms.Count == 1)
          {
            // Handle null or empty list
            GetRatio(newRoom.cells[0]);
            PlaceKey(newRoom.bounds.position);
            Debug.Log("First room is here!");
            Debug.Log(newRoom.bounds.position.ToString());
          }
          // else if (!isExit && newRoom.bounds.size.x == 2 && newRoom.bounds.size.y == 1)
          // {
          //   Debug.Log("Exit is here!");
          //   PlaceExitDoor(newRoom.bounds.position);
          //   Debug.Log(newRoom.bounds.position.ToString());
          //   isExit = true;
          // }

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

              var preLocation = preHallway.location;


              if (preHallway.status[0])
              {
                AddPillar(new Vector2(preLocation.x + 0.45f, preLocation.y + 0.45f));
              }
              else if (preHallway.status[1])
              {
                AddPillar(new Vector2(preLocation.x + 0.45f, preLocation.y - 0.45f));
              }
              else if (preHallway.status[2])
              {
                AddPillar(new Vector2(preLocation.x - 0.45f, preLocation.y - 0.45f));
              }
              else if (preHallway.status[3])
              {
                AddPillar(new Vector2(preLocation.x - 0.45f, preLocation.y + 0.45f));
              }


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
        var preLocation1 = preHallway.location;
        for (int k = 0; k < 4; k++)
        {
          if (preHallway.status[k])
          {
            AddPillar(new Vector2(preLocation1.x + 0.45f, preLocation1.y + 0.45f));
          }

        }

      }
    }
  }

  void AddPillar(Vector2 location)
  {

    var pillar = pillars.Find(x => x.location == location || x.location.x - location.x <= 0.01f || x.location.y - location.y <= 0.01f);
    // var pillar = pillars.Find(x => x.location == location);
    if (pillar == null)
    {
      PlacePillar(location);
      pillars.Add(new Pillar(location));
    }

  }
  GameObject PlaceCube(Vector2Int location)
  {
    GameObject go = Instantiate(roomPrefab, new Vector3(location.x, 0, location.y), Quaternion.identity);
    var prefabSize = getPrefabSize(go).size;
    go.GetComponent<Transform>().localScale = new Vector3(1 / prefabSize.x, 1 / prefabSize.y, 1 / prefabSize.z);
    return go;
  }

  GameObject PlacePillar(Vector2 location)
  {
    GameObject go = Instantiate(pillarPrefab, new Vector3(location.x, 0, location.y), Quaternion.identity);
    var prefabSize = go.gameObject.GetComponent<Renderer>().bounds.size;
    go.GetComponent<Transform>().localScale = new Vector3(1 / (4 * prefabSize.x), 1 / prefabSize.y, 1 / (4 * prefabSize.z));
    return go;
  }

  GameObject PlaceKey(Vector2 location)
  {
    GameObject go = Instantiate(key, new Vector3(location.x, 0.05f, location.y), Quaternion.identity);
    var prefabSize = go.gameObject.GetComponent<Renderer>().bounds.size;
    go.GetComponent<Transform>().localScale = new Vector3(3 * ratio, 3 * ratio, 3 * ratio);
    return go;
  }
  void PlaceExitDoor()
  {
    var isExit = false;
    bool[] status = { false, false, false, false };

    foreach (Room room in rooms)
    {
      for (int i = room.statusList.GetLength(0) - 1; i >= 0; i--)
      {
        for (int j = 0; j < 4; j++)
        {
          if (room.statusList[i][j] == true)
          {
            // i = row * size.y + column;
            var posX = Mathf.Floor(i / room.bounds.size.y) + room.location.x;
            var posY = i % room.bounds.size.y + room.location.y;
            Debug.Log(posX.ToString());
            Debug.Log(posY.ToString());
            GameObject go = Instantiate(exitDoor, new Vector3(posX + 0.4f, 0, posY), Quaternion.identity);
            var prefabSize = GetWrappedPrefabSize(go).size;
            go.GetComponent<Transform>().localScale = new Vector3(1 / (prefabSize.x), 1 / (prefabSize.y), 1 / (prefabSize.z));
            status[j] = true;
            go.GetComponent<CellBehavior>().UpdateCell(status);
            // go.GetComponent<Transform>().position = new Vector3(posX, 0, posY);

            isExit = true;
            break;
          }

        }
        if (isExit) break;
      }
      if (isExit) break;

    }
  }

  GameObject PlaceRoomLight(Vector2 location)
  {
    GameObject go = Instantiate(roomLightPrefab, new Vector3(location.x, 0.7f, location.y), Quaternion.identity);
    // var prefabSize = go.gameObject.GetComponent<Renderer>().bounds.size;
    var prefabBounds = getPrefabSize(go);
    var size = prefabBounds.size;
    go.GetComponent<Transform>().localScale = new Vector3(1 / (4 * size.x), 1 / (4 * size.y), 1 / (4 * size.z));
    return go;
  }

  void PlaceRoomDecor(Vector2 location, Vector2Int size)
  {
    GameObject decor;
    if (size.x == 1 && size.y == 1)
    {
      decor = Instantiate(miniRooms[0], new Vector3(location.x + 0.5f, 0, location.y + 0.5f), Quaternion.identity);
      var prefabSize = getPrefabSize(decor).size;
      decor.GetComponent<Transform>().localScale = new Vector3(size.x / (2 * prefabSize.x), 1 / (4 * prefabSize.y), size.y / (2 * prefabSize.z));
    }
    // else if (size.x == 1 && size.y == 2)
    // {
    //   decor = Instantiate(miniRooms[1], new Vector3(location.x, 0.5f, location.y), Quaternion.identity);
    //   var prefabSize = getPrefabSize(decor).size;
    //   decor.GetComponent<Transform>().localScale = new Vector3(size.x / (2 * prefabSize.x), 1 / (4 * prefabSize.y), size.y / (2 * prefabSize.z));
    //   decor.GetComponent<Transform>().position = new Vector3(location.x + size.x / 2, 0f, location.y + size.y / 2);
    // }
    // else if (size.x == 2 && size.y == 1)
    // {
    //   decor = Instantiate(miniRooms[2], new Vector3(location.x, 0.5f, location.y), Quaternion.identity);
    //   var prefabSize = getPrefabSize(decor).size;
    //   decor.GetComponent<Transform>().localScale = new Vector3(size.x / (2 * prefabSize.x), 1 / (4 * prefabSize.y), size.y / (2 * prefabSize.z));
    //   decor.GetComponent<Transform>().position = new Vector3(location.x + size.x / 2, 0f, location.y + size.y / 2);
    // }
    else if (size.x == 2 && size.y == 2)
    {
      decor = Instantiate(miniRooms[3], new Vector3(location.x, 0.2f, location.y), Quaternion.identity);
      var prefabSize = getPrefabSize(decor).size;
      decor.GetComponent<Transform>().localScale = new Vector3(size.x / (prefabSize.x), 1 / (2 * prefabSize.y), size.y / (prefabSize.z));
      // decor.GetComponent<Transform>().localScale = new Vector3(ratio, ratio, ratio);
      decor.GetComponent<Transform>().position = new Vector3(location.x + size.x / 2, 0.26f, location.y + size.y / 2);
      // decor.GetComponent<Transform>().position = new Vector3(location.x + size.x / 2, size.y * ratio, location.y + size.y / 2);
      // Debug.Log(size.ToString());
    }
    // var prefabSize = getPrefabSize(decor).size;
    // decor.GetComponent<Transform>().localScale = new Vector3(size.x / (4 * prefabSize.x), 1 / (4 * prefabSize.y), size.y / (4 * prefabSize.z));
  }

  void PlaceRoom(Vector2Int location, Vector2Int size, Room room)
  {
    // Debug.Log(size.ToString());
    // Debug.Log("hahah");
    GameObject[] objs = new GameObject[size.x * size.y];
    bool[][] statusList = new bool[size.x * size.y][];
    List<Pillar> pillars = new List<Pillar>();

    for (int i = 0; i < size.x; i++)
    {
      for (int j = 0; j < size.y; j++)
      {
        var obj = PlaceCube(new Vector2Int(location.x + i, location.y + j));
        objs[i * size.y + j] = obj;
        bool[] status = { false, false, false, false };

        if (i == 0)
        {
          PlacePillar(new Vector2(location.x + i - 0.45f, location.y + j - 0.45f));
          status[2] = true;
        }
        if (i == size.x - 1)
        {
          PlacePillar(new Vector2(location.x + i + 0.45f, location.y + j + 0.45f));

          status[0] = true;
        }
        if (j == 0)
        {
          PlacePillar(new Vector2(location.x + i + 0.45f, location.y + j - 0.45f));


          status[1] = true;
        }
        if (j == size.y - 1)
        {
          PlacePillar(new Vector2(location.x + i - 0.45f, location.y + j + 0.45f));
          // PlacePillar(new Vector2(location.x + i + 0.45f, location.y + j - 0.45f));


          status[3] = true;
        }

        obj.GetComponent<CellBehavior>().UpdateCell(status);
        obj.name = "room" + (i * size.y + j).ToString();
        statusList[i * size.y + j] = status;
      }
    }

    // try place the room light in the middle of the room and only once
    PlaceRoomLight(new Vector2(location.x + (size.x) / 2, location.y + (size.y) / 2));

    PlaceRoomDecor(new Vector2(location.x, location.y), size);


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

  Bounds GetWrappedPrefabSize(GameObject obj)
  {
    Vector3 center = new Vector3(0, 0, 0);
    List<GameObject> objs = GetGameObjects(obj);

    foreach (GameObject child in objs)
    {
      center += child.GetComponent<Renderer>().bounds.center;
    }
    center /= objs.Count(); //center is average center of children

    //Now you have a center, calculate the bounds by creating a zero sized 'Bounds', 
    Bounds bounds = new Bounds(center, Vector3.zero);
    foreach (GameObject child in objs)
    {
      bounds.Encapsulate(child.GetComponent<Renderer>().bounds);
    }
    return bounds;
  }

  List<GameObject> GetGameObjects(GameObject obj)
  {

    List<GameObject> currentList = new List<GameObject>();
    if (obj.transform.childCount == 0 && obj.GetComponent<Renderer>())
    {
      currentList.Add(obj);
      Debug.Log("Add a child");
      return currentList;
    }
    else if (obj.transform.childCount != 0)
    {
      foreach (Transform child in obj.transform)
      {
        List<GameObject> childrenList = GetGameObjects(child.gameObject);
        currentList = currentList.Concat(childrenList).ToList();
      }
    }
    return currentList;

  }

  void GetRatio(GameObject obj)
  {
    var prefabSize = getPrefabSize(obj).size;
    this.ratio = 1 / prefabSize.x;

  }
}
