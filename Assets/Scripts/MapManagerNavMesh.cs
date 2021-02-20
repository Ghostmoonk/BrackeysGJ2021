using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MapManagerNavMesh : MonoBehaviour
{
    MapManager mapManagerScript;
    NavMeshSurface[] navMeshSurfaces;
    private GameObject chunkHolder;


    // Start is called before the first frame update
    void Start()
    {
        mapManagerScript = GetComponent<MapManager>();
        List<Chunk> listeChunks = mapManagerScript.GetChunks();
        Debug.Log("Il y a "+ listeChunks.Count + "chunks");
        chunkHolder = mapManagerScript.ChunkHolder;
        foreach (Transform child in chunkHolder.transform)
        {
            child.gameObject.GetComponentInChildren<NavMeshSurface>().BuildNavMesh();
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
