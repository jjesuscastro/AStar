using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStarDebugger : MonoBehaviour
{
    [SerializeField]
    private Grid grid;
    [SerializeField]
    private Tilemap tilemap;
    [SerializeField]
    private Tile tile;
    [SerializeField]
    private GameObject debugTextPrefab;
    [SerializeField]
    private Color openColor, closedColor, pathColor, goalColor;

    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private List<GameObject> debugTiles;

    public static AStarDebugger Instance;

    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(this);
        }
    }

    public void CreateTiles(HashSet<AStarNode> openList, HashSet<AStarNode> closedList, Dictionary<Vector3Int, AStarNode> allNodes,  Vector3Int start, Vector3Int goal, Stack<Vector3Int> path = null)
    {
        foreach(AStarNode node in openList)
        {
            ColorTile(node.position, openColor);
        }

        foreach(AStarNode node in closedList)
        {
            ColorTile(node.position, closedColor);
        }

        if (path != null)
        {
            foreach (Vector3Int pos in path)
            {
                if (pos != start && pos != goal)
                {
                    ColorTile(pos, pathColor);
                }
            }
        }

        ColorTile(start, pathColor);
        ColorTile(goal, goalColor);

        foreach(KeyValuePair<Vector3Int, AStarNode> node in allNodes)
        {
            if(node.Value.parent != null)
            {
                GameObject go = Instantiate(debugTextPrefab, canvas.transform);
                go.transform.position = grid.CellToWorld(node.Key);
                debugTiles.Add(go);
                GenerateDebugText(node.Value, go.GetComponent<DebugText>());
            }
        }
    }

    private void GenerateDebugText(AStarNode node, DebugText debugText)
    {
        debugText.GScore.text = $"{node.G}";
        debugText.HScore.text = $"{node.H}";
        debugText.FScore.text = $"{node.F}";
        debugText.arrow.localRotation = Quaternion.Euler(new Vector3(0, 0, 180f / Mathf.PI * Mathf.Atan2(node.parent.position.y - node.position.y, node.parent.position.x - node.position.x)));
    }

    public void ColorTile(Vector3Int position, Color color)
    {
        tilemap.SetTile(position, tile);
        tilemap.SetTileFlags(position, TileFlags.None);
        tilemap.SetColor(position, color);
    }


}
