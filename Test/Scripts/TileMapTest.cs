using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using rc;
using PathFinder;

public class TileMapTest : MonoBehaviour
{
    public Tilemap tilemapGround;
    public List<Tilemap> tilemapObjects;

    public Camera camera;

    PathFinder.Graph graph = new Graph();
    PathFinder.GraphSearch graphSearch = new GraphSearch();
    Queue<PathFinder.Node> paths = new Queue<Node>();

    public GameObject chara;
    Vector3 start = new Vector3Int();
    Vector3 destination = new Vector3Int();

    int GetCellId(Vector3Int cell)
    {
        if (!tilemapGround.HasTile(cell))
        {
            return 0;
        }
        return string.Format("{0:0000}{1:0000}{2:0000}", cell.x, cell.y, cell.z).GetHashCode();
    }

    // Start is called before the first frame update
    void Start()
    {
        //tilemap.SetTile(new Vector3Int(0, 0, 0), null);
        //tilemap.SetColor(new Vector3Int(0, 0, 0), Color.red);

        //var block = tilemapGround.GetTilesBlock(new BoundsInt(-10, -10, -10, 100, 100, 100));
        destination = tilemapGround.CellToWorld(new Vector3Int(10, 10, 0));

        int size = 200;

        foreach (var x in Enumerable.Range(-100, size))
            foreach (var y in Enumerable.Range(-100, size))
            {
                var cell = new Vector3Int(x, y, 0);
                if (!tilemapGround.HasTile(cell))
                {
                    continue;
                }
                if (tilemapObjects[0].HasTile(cell))
                {
                    continue;
                }
                graph.AddNode(new Node(GetCellId(cell), cell));
            }
        foreach (var x in Enumerable.Range(-100, size))
            foreach (var y in Enumerable.Range(-100, size))
            {
                var cell = new Vector3Int(x, y, 0);
                if (!tilemapGround.HasTile(cell))
                {
                    continue;
                }
                foreach (var ox in Enumerable.Range(-1, 3))
                    foreach (var oy in Enumerable.Range(-1, 3))
                    {
                        if (ox == 0 && oy == 0)
                        {
                            continue;
                        }
                        var oCell = new Vector3Int(ox, oy, 0) + cell;
                        if (!tilemapGround.HasTile(oCell))
                        {
                            continue;
                        }
                        if (tilemapObjects[0].HasTile(oCell))
                        {
                            continue;
                        }
                        if (graph.GetNode(GetCellId(cell)) == default(Node))
                        {
                            continue;
                        }
                        if (graph.GetNode(GetCellId(oCell)) == default(Node))
                        {
                            continue;
                        }
                        int cost = Mathf.Abs(ox + oy);
                        graph.AddEdge(new Edge(GetCellId(cell), GetCellId(oCell), cost));
                    }
            }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            var mousePos = Input.mousePosition;
            //mousePos.z = tilemapGround.gameObject.transform.position.z;
            var grid = camera.ScreenToWorldPoint(mousePos);

            var cellFrom = tilemapGround.WorldToCell(chara.transform.position);
            var cellTo = tilemapGround.WorldToCell(grid);

            graphSearch.Search(graph, GetCellId(cellFrom), GetCellId(cellTo));
            var pathList = graphSearch.GetShortestPaths();
            //pathList.Reverse();
            paths = new Queue<Node>(pathList);
            //paths = new Queue<Node>(graphSearch.CalcShortestPaths(GetCellId(cell)));
            start = chara.transform.position;


            foreach (var node in paths)
            {

            }
        }

        if (0 < paths.Count)
        {
            var cell = (Vector3Int)paths.Peek().UserData;
            destination = tilemapGround.CellToWorld(cell);

            chara.transform.position = Vector3.MoveTowards(chara.transform.position, destination, 0.1f);
            //chara.transform.position = Vector3.Lerp(chara.transform.position, destination,  (destination - start).magnitude * Time.deltaTime);

            var distance = Vector3.Distance(chara.transform.position, destination);
            if (distance < 0.1f)
            {
                paths.Dequeue();
            }
        }
        foreach (var edge in graph.GetEdges())
        {
            var from = graph.GetNode(edge.From);
            var to = graph.GetNode(edge.To);
            var fromPos = tilemapGround.CellToWorld((Vector3Int)(from.UserData));
            var toPos = tilemapGround.CellToWorld((Vector3Int)(to.UserData));

            Color color = Color.white;
            color = Color.black;
            //color.a = 1.0f - (float)(edge.Cost * 0.1f);
            Debug.DrawLine(fromPos, toPos, color);
        }



    }
}
