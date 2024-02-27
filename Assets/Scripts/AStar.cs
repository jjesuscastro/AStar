using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStar : MonoBehaviour
{
    [SerializeField]
    private Tilemap tilemap;
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private LayerMask mask;

    private Vector3Int startPos, goalPos;

    private AStarNode currentNode;
    private HashSet<AStarNode> openList, closedList;
    private Dictionary<Vector3Int, AStarNode> allNodes = new Dictionary<Vector3Int, AStarNode>();
    private Stack<Vector3Int> path;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, mask);

            if(hit.collider != null)
            {
                Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int clickPos = tilemap.WorldToCell(mouseWorldPos);

                //if (startPos == Vector3Int.zero)
                //{
                //    startPos = clickPos;
                //}
                //else
                //{
                //    if (currentNode == null)
                //        Initialize();
                //    goalPos = clickPos;
                //    RunAlgo();
                //}
            }
        }
    }

    private void Initialize()
    {
        currentNode = GetNode(startPos);
        openList = new HashSet<AStarNode>();
        closedList = new HashSet<AStarNode>();

        openList.Add(currentNode);
    }

    public Stack<Vector3Int> Algorithm(Vector3 postion, Vector3 goal)
    {
        ClearAStar();
        startPos = tilemap.WorldToCell(postion);
        goalPos = tilemap.WorldToCell(goal);

        if (currentNode == null)
        {
            Initialize();
        }

        while (openList.Count > 0 && path == null)
        {
            List<AStarNode> neighbors = FindNeighbors(currentNode.position);

            ExamineNeighbors(neighbors, currentNode);

            UpdateCurrentNode(ref currentNode);

            path = GeneratePath(currentNode);
        }

        //AStarDebugger.Instance.CreateTiles(openList, closedList, allNodes, startPos, goalPos, path);

        return path;

    }

    private void ClearAStar()
    {
        if (currentNode != null)
        {
            allNodes.Clear();
            closedList.Clear();
            openList.Clear();
            path = null;
            currentNode = null;
        }

    }

    private void RunAlgo()
    {
        while(openList.Count > 0 && path == null)
        {
            List<AStarNode> neighbors = FindNeighbors(currentNode.position);
            ExamineNeighbors(neighbors, currentNode);
            UpdateCurrentNode(ref currentNode);

            path = GeneratePath(currentNode);
        }
                
        //AStarDebugger.Instance.CreateTiles(openList, closedList, allNodes, startPos, goalPos, path);
    }

    private List<AStarNode> FindNeighbors(Vector3Int parentPos)
    {
        List<AStarNode> neighbors = new List<AStarNode>();

        for(int x = -1; x <= 1; x++)
        {
            for(int y = -1; y <= 1; y++)
            {
                Vector3Int neighborPos = new Vector3Int(parentPos.x - x, parentPos.y - y, parentPos.z);

                if(x != 0 || y != 0)
                {
                    if(neighborPos != startPos && tilemap.GetTile(neighborPos))
                    {
                        AStarNode neighbor = GetNode(neighborPos);
                        neighbors.Add(neighbor);
                    }
                }
            }
        }

        return neighbors;
    }

    private void ExamineNeighbors(List<AStarNode> neighbors, AStarNode currentNode)
    {
        foreach(AStarNode neighbor in neighbors)
        {
            int gScore = CalculateGScore(neighbor.position, currentNode.position);

            if(openList.Contains(neighbor))
            {
                if(currentNode.G + gScore < neighbor.G)
                {
                    CalculateValues(currentNode, neighbor, gScore);
                }
            } else if(!closedList.Contains(neighbor))
            {
                CalculateValues(currentNode, neighbor, gScore);

                openList.Add(neighbor);
            }
        }
    }

    private void CalculateValues(AStarNode parent, AStarNode neighbor, int cost)
    {
        neighbor.parent = parent;
        neighbor.G = parent.G + cost;
        neighbor.H = (Math.Abs(neighbor.position.x - goalPos.x) + Math.Abs(neighbor.position.y - goalPos.y)) * 10;
    }

    private int CalculateGScore(Vector3Int neighbor, Vector3Int current)
    {
        int x = current.x - neighbor.x;
        int y = current.y - neighbor.y;

        return Math.Abs(x - y) % 2 == 1 ? 10 : 14;
    }

    private void UpdateCurrentNode(ref AStarNode current)
    {
        openList.Remove(current);
        closedList.Add(current);

        if(openList.Count > 0)
        {
            current = openList.OrderBy(x => x.F).First();
        }
    }

    private AStarNode GetNode(Vector3Int position)
    {
        if(allNodes.ContainsKey(position))
            return allNodes[position];

        AStarNode node = new AStarNode(position);
        allNodes.Add(position, node);
        return node;
    }

    public Stack<Vector3Int> GetPath()
    {
        return path;
    }

    private Stack<Vector3Int> GeneratePath(AStarNode currentNode)
    {
        if(currentNode.position == goalPos)
        {
            Stack<Vector3Int> finalPath = new Stack<Vector3Int>();

            while(currentNode.position != startPos)
            {
                finalPath.Push(currentNode.position);
                currentNode = currentNode.parent;
            }

            return finalPath;
        }

        return null;
    }
}

public enum TileType
{
    GRASS,
    WATER
}