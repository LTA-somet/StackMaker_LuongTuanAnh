using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    int[,] mapMatrix = new int[,]
{
    { 1, 0, 0, 1 },
    { 0, 1, 0, 0 },
    { 1, 0, 1, 1 },
    { 0, 0, 0, 1 }
};
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject brickPrefab;
    [SerializeField] private GameObject unBrickPrefab;
    [SerializeField] private GameObject winPrefab;
    [SerializeField] private Transform prefabHolder;
    void GenerateMap(int[,] mapMatrix)
    {
        for (int i = 0; i < mapMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < mapMatrix.GetLength(1); j++)
            {
                Vector3 position = new Vector3(i, 0, j);
                GameObject tile;

                switch (mapMatrix[i, j])
                {
                    case 0:
                        tile = Instantiate(brickPrefab, position, Quaternion.identity);
                        break;
                    case 1:
                        tile = Instantiate(wallPrefab, position, Quaternion.identity);
                        break;
                    case 2:
                        tile = Instantiate(unBrickPrefab, position, Quaternion.identity);
                        break;
                    case 3:
                        tile = Instantiate(winPrefab, position, Quaternion.identity);
                        break;

                    default:
                        tile = Instantiate(wallPrefab, position, Quaternion.identity);
                        break;
                }

                tile.transform.parent = prefabHolder;
            }
        }

    }
}
