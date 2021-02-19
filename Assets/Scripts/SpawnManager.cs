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
    [Tooltip("Liste des modèles de chevaliers"), SerializeField]
    // private List<GameObject> listePrefabKnights;
    // [Tooltip("Liste des modèles de porteurs de torche"), SerializeField]
    // private List<GameObject> listePrefabTorchs;
    // [Tooltip("Modèle du fantôme"), SerializeField]
    private GameObject prefabGhost;
    [Tooltip("Modèle du leader"), SerializeField]
    private GameObject prefabLeader;

    private int difficulty = 0;

    void FixedStart() {
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
            float pX = gameObject.transform.position.x;
            float pZ = gameObject.transform.position.z;
            Vector3 pos = new Vector3(UnityEngine.Random.Range(pX-8, pX+8), 10, UnityEngine.Random.Range(pZ-8, pZ+8));
            Instantiate(currentVillager, pos, Quaternion.identity);
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