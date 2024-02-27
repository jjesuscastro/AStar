using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarNode
{
    public int G; //Steps from start node
    public int H; //Heuristic steps from goal node
    public int F => G + H; //Sum of G and H

    public AStarNode parent;
    public Vector3Int position;

    public AStarNode(Vector3Int position)
    {
        this.position = position;
    }
}
