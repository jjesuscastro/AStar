using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] penguins;
    [SerializeField]
    private Tilemap tileMap;

    private List<Vector3> availablePlaces;

    private void Start()
    {
        //Position von jedem Floortile ermitteln:
        availablePlaces = new List<Vector3>();

        for (int n = tileMap.cellBounds.xMin; n < tileMap.cellBounds.xMax; n++)
        {
            for (int p = tileMap.cellBounds.yMin; p < tileMap.cellBounds.yMax; p++)
            {
                Vector3Int localPlace = (new Vector3Int(n, p, (int)tileMap.transform.position.y));
                Vector3 place = tileMap.CellToWorld(localPlace);
                if (tileMap.HasTile(localPlace))
                {
                    availablePlaces.Add(place);
                }
            }
        }

        SpawnPenguin();
    }

    public void SpawnPenguin()
    {
        for(int i = 0; i < penguins.Length; i++)
        {
            penguins[i].SetActive(true);
            penguins[i].transform.position = availablePlaces[Random.Range(0, availablePlaces.Count)];
        }
    }
}
