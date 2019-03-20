using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathFinder;
using UnityEngine.Tilemaps;

#if false

public class TileMapPathFinder : MonoBehaviour
{
    public Camera camera;
    public Tilemap tilemapGround;
    public List<Tilemap> tilemapObjects;

    PathFinder.Graph graph = new Graph();
    PathFinder.GraphSearch graphSearch = new GraphSearch();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update(int startX, int startY, int sizeX, int sizeY)
    {
        var tile = new Tile();
        int size = 200;

        foreach (var x in Enumerable.Range(startX, sizeX))
            foreach (var y in Enumerable.Range(startY, sizeY))
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

    void BuildGrapch(PathFinder.Graph graph)
    {
    }

    int GetCellId(Vector3Int cell)
    {
        if (!tilemapGround.HasTile(cell))
        {
            return 0;
        }
        return string.Format("{0:0000}{1:0000}{2:0000}", cell.x, cell.y, cell.z).GetHashCode();
    }
}
#endif//false
