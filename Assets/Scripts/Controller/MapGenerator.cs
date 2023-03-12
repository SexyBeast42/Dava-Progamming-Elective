// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.AI;
// using UnityEngine.AI.Navigation;
//
// public class MapGenerator : MonoBehaviour
// {
//     public GameObject floorPrefab;
//     public GameObject wallPrefab;
//     public GameObject npcPrefab;
//     public float tileSize = 1.0f;
//     public int mapWidth = 10;
//     public int mapHeight = 10;
//     public int minNpcsPerGroup = 1;
//     public int maxNpcsPerGroup = 5;
//     public int numNpcGroups = 3;
//
//     private NavMesh navMesh;
//     private NavMeshSurface navMeshSurface;
//
//     // Start is called before the first frame update
//     void Start()
//     {
//         // Spawn in floors and walls
//         for (int x = 0; x < mapWidth; x++)
//         {
//             for (int y = 0; y < mapHeight; y++)
//             {
//                 // Spawn in a floor tile
//                 Vector3 floorPosition = new Vector3(x * tileSize, 0, y * tileSize);
//                 Instantiate(floorPrefab, floorPosition, Quaternion.identity);
//
//                 // Spawn in walls around the edges of the map
//                 if (x == 0 || x == mapWidth - 1 || y == 0 || y == mapHeight - 1)
//                 {
//                     Vector3 wallPosition = new Vector3(x * tileSize, 0.5f, y * tileSize);
//                     Instantiate(wallPrefab, wallPosition, Quaternion.identity);
//                 }
//             }
//         }
//
//         // Generate NavMesh
//         navMeshSurface = gameObject.AddComponent<NavMeshSurface>();
//         navMeshSurface.BuildNavMesh();
//         navMesh = NavMesh.GetNavMeshById(0);
//
//         // Spawn in NPC groups
//         for (int i = 0; i < numNpcGroups; i++)
//         {
//             int numNpcsInGroup = Random.Range(minNpcsPerGroup, maxNpcsPerGroup + 1);
//
//             for (int j = 0; j < numNpcsInGroup; j++)
//             {
//                 Vector3 npcPosition = new Vector3(Random.Range(tileSize, (mapWidth - 1) * tileSize), 0, Random.Range(tileSize, (mapHeight - 1) * tileSize));
//                 Instantiate(npcPrefab, npcPosition, Quaternion.identity);
//             }
//         }
//     }
//
//     // Update is called once per frame
//     void Update()
//     {
//         // Do nothing
//     }
// }