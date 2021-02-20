using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MapManagerNavMesh : MonoBehaviour
{
    MapManager mapManagerScript;
    NavMeshSurface[] navMeshSurfaces;
    private GameObject chunkHolder;
    public GameObject navMeshChunk;
    GameObject SpawnNavMesh;

    // Start is called before the first frame update
    void Start()
    {
        mapManagerScript = GetComponent<MapManager>();
        List<Chunk> listeChunks = mapManagerScript.GetChunks();
        Debug.Log("Il y a "+ listeChunks.Count + "chunks");
        chunkHolder = mapManagerScript.ChunkHolder;
        SpawnNavMesh = GameObject.FindGameObjectsWithTag("Spawn")[0];
        Debug.Log(SpawnNavMesh);
        NavMeshSurface ChildrenNavMesh = SpawnNavMesh.GetComponentInChildren<NavMeshSurface>();
        Debug.Log(ChildrenNavMesh);
        ChildrenNavMesh.BuildNavMesh();
        //navMeshChunk.GetComponent<NavMeshSurface>().BuildNavMesh();



        
        foreach (Transform child in chunkHolder.transform)
        {
            //child.gameObject.GetComponentInChildren<NavMeshSurface>().BuildNavMesh();
        }
            for (int i = 0; i < listeChunks.Count; i++)
        {
            
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
