using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
    public GameObject wallPrefab;
    public List<GameObject> npcPrefab;
    public int mapWidth, mapHeight;

    public List<Vector3> playerLocations;

    private NavMeshSurface navMeshSurface;

    // Start is called before the first frame update
    void Start()
    {
        // Generate map
        GenerateMap();
        
        // Pauses time
        Time.timeScale = 0f;

        // Generate NavMesh
        // Works but navmesh gets screwed if the floor isn't one piece
        navMeshSurface = gameObject.AddComponent<NavMeshSurface>();
        navMeshSurface.BuildNavMesh();
        
        // Generate players
        GeneratePlayers();
        
        // Unpause time
        StartCoroutine(UnpauseTime());
    }

    IEnumerator UnpauseTime()
    {
        yield return new WaitForSecondsRealtime(2);

        Time.timeScale = 1f;
    }

    private void GenerateMap()
    {
        // Spawn walls
        for (int x = 0; x <= mapWidth; x++)
        {
            for (int y = 0; y <= mapHeight; y++)
            {
                float generateWalls = Random.Range(0f, 9f);
                
                // Determines whether or not we should spawn walls
                if (generateWalls < 2f)
                {
                    Vector3 wallPos = new Vector3(x - mapWidth / 2f, 1f, y - mapHeight / 2f);
                    
                    //Vector3 wallPos = new Vector3(x * tileSize, .5f, y * tileSize);
                    Instantiate(wallPrefab, wallPos, quaternion.identity, transform);

                }

                float spawnPlayer = Random.Range(0f, 100f);
                

                // Determines whether or not we should spawn a player, then save location
                if (spawnPlayer < 2f)
                {
                    Vector3 npcPosition = new Vector3(x - mapWidth / 2f, 1f, y - mapHeight / 2f);

                    playerLocations.Add(npcPosition);
                }
            }
        }
    }

    private void GeneratePlayers()
    {
        int lastPlayerSpawned = 4;
        
        for (int i = 0; i < playerLocations.Count; i++)
        {
            int teamNumber = Random.Range(0, 3);

            if (lastPlayerSpawned != teamNumber && !DetectWall(playerLocations[i]))
            {
                Instantiate(npcPrefab[teamNumber], playerLocations[i], Quaternion.identity);

                // Make sure we get at least one team versing each other
                lastPlayerSpawned = teamNumber;
            }
        }
    }
    
    private bool DetectWall(Vector3 position)
    {
        int layerMask = 6;
        int maxDist = 1;
        
        // Raycast all directions
        Vector3 playerPosition = position;
        Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };

        foreach (Vector3 direction in directions)
        {
            // Cast ray to direction
            RaycastHit hit;
            
            if (Physics.Raycast(playerPosition, direction, out hit, maxDist, layerMask))
            {
                return true;
            }
        }
        
        return false;
    }
}