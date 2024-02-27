using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Penguin : MonoBehaviour
{
    [SerializeField]
    private Tilemap tilemap;
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private LayerMask mask;
    [SerializeField]
    private Animator animator;

    private Stack<Vector3Int> path;
    private Vector3 destination;
    private Vector3 goal;

    [SerializeField]
    private AStar aStar;

    public float movementSpeed;

    private void Update()
    {
        ClickToMove();

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, mask);

            if (hit.collider != null)
            {
                Debug.Log("Hit");
                Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

                GetPath(mouseWorldPos);
            }
        }
    }

    public void GetPath(Vector3 goal)
    {
        path = aStar.Algorithm(transform.parent.position, goal);
        destination = tilemap.CellToWorld(path.Pop());
        this.goal = goal;
    }

    private void ClickToMove()
    {
        if(path != null)
        {
            animator.SetFloat("movementSpeed", movementSpeed);
            transform.parent.position = Vector2.MoveTowards(transform.parent.position, destination, movementSpeed * Time.deltaTime);

            float distance = Vector2.Distance(destination, transform.parent.position);

            if(distance <= 0f)
            {
                if(path.Count > 0)
                {
                    destination = tilemap.CellToWorld(path.Pop());
                    Debug.Log($"Destination: {destination}");
                } else
                {
                    animator.SetFloat("movementSpeed", 0);
                    path = null;
                }
            }
        }
    }
}
