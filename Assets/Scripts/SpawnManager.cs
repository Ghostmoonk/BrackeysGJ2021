using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
    [Header("Paramètres du chunk")]
    [Tooltip("Type du chunk"), SerializeField]
    private MapType mapType;

    [Header("Prefabs et modèles")]
    [Tooltip("Liste des modèles de villageois"), SerializeField]
    private List<GameObject> listePrefabVillager;
    // [Tooltip("Liste des modèles de chevaliers"), SerializeField]
    // private List<GameObject> listePrefabKnights;
    // [Tooltip("Liste des modèles de porteurs de torche"), SerializeField]
    // private List<GameObject> listePrefabTorchs;
    // [Tooltip("Modèle du fantôme"), SerializeField]
    private GameObject prefabGhost;
    // [Tooltip("Modèle du leader"), SerializeField]
    private GameObject prefabLeader;

    private List<Transform> listeSpawns;

    private int difficulty = 0;

    void FixedStart() {
        listeSpawns = new List<Transform>();
        foreach (Transform child in transform) {
            if (child.gameObject.CompareTag("MarkSpawn")) {
                listeSpawns.Add(child);
            }
        }
        Debug.Log(listeSpawns.Count);
        if (mapType == MapType.Spawn) {
            SpawnVillager(4, 4);
        } else if (mapType == MapType.Firecamp) {
            SpawnVillager(2, 6);
        } else if (mapType == MapType.Base) {
            // 1 2 pour une difficulté de 1
            // 3 4 pour une difficulté de 2
            // 4 7 pour une difficulté de 3
            int min = (int)Math.Abs(1.4*(difficulty*1.5));
            int max = (int)Math.Abs(1.4*(difficulty*2));
            SpawnVillager(min, max);
        }
    }

    private void SpawnVillager(int nbMin, int nbMax) {
        int randV = UnityEngine.Random.Range(nbMin, nbMax);
        for (int i = 0; i < randV; i++) {
            GameObject currentVillager = listePrefabVillager[UnityEngine.Random.Range(0, listePrefabVillager.Count)];
            Transform pos = listeSpawns[UnityEngine.Random.Range(0, listeSpawns.Count)];
            listeSpawns.Remove(pos);
            Instantiate(currentVillager, pos.position, Quaternion.identity);
        }
    }

    public void SetDifficulty(int _difficulty) {
        difficulty = _difficulty;
        FixedStart();
    }
}

public enum MapType{
    Spawn,
    Castle,
    Firecamp,
    Base
}