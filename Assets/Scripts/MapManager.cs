using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {
    public GameObject prefabCastle;
    public GameObject prefabSpawn;
    public GameObject prefabFirecamp;
    public GameObject prefabBase1;
    public GameObject prefabBase2;
    public GameObject prefabBase3;
    
    private List<Chunk> listeChunks;
    void Start() {
        int randomCastle = GenRandomCastleNum();
        List<int> randomFirecamp = GenRandomFirecamp();
        int chunk3 = Random.Range(2, 6);
        int chunk2 = Random.Range(5, 11);
        int chunk1 = 29 - chunk3 - chunk2;
        // Debug.Log(chunk1.ToString() + " " + chunk2.ToString() + " " + chunk3.ToString());
        listeChunks = new List<Chunk>();
        for(int i = 0; i < 35; i++) {
            listeChunks.Add(new Chunk(i%7, i/7));
            if (i == 17) {
                // Spawn
                listeChunks[i].UpdateTypeDif("spawn");
                Instantiate(prefabSpawn, new Vector3(listeChunks[i].GetX(), listeChunks[i].GetY(), 0), Quaternion.identity);
            } else if (i == randomCastle) {
                // Castle
                listeChunks[i].UpdateTypeDif("castle");
                Instantiate(prefabCastle, new Vector3(listeChunks[i].GetX(), listeChunks[i].GetY(), 0), Quaternion.identity);
            } else if (randomFirecamp.Contains(i)) {
                // Firecamp
                listeChunks[i].UpdateTypeDif("firecamp");
                Instantiate(prefabFirecamp, new Vector3(listeChunks[i].GetX(), listeChunks[i].GetY(), 0), Quaternion.identity);
            } else {
                // Base chunks
                int randCh = Random.Range(0, chunk1+chunk2+chunk3);
                if (randCh >= 0 && randCh < chunk3) {
                    // Difficulty 3
                    listeChunks[i].UpdateTypeDif("base", 3);
                    Instantiate(prefabBase3, new Vector3(listeChunks[i].GetX(), listeChunks[i].GetY(), 0), Quaternion.identity);
                    chunk3--;
                } else if (randCh >= chunk3 && randCh < chunk2+chunk3) {
                    // Difficulty 2
                    listeChunks[i].UpdateTypeDif("base", 2);
                    Instantiate(prefabBase2, new Vector3(listeChunks[i].GetX(), listeChunks[i].GetY(), 0), Quaternion.identity);
                    chunk2--;
                } else if (randCh >= chunk2+chunk3 && randCh <= chunk1+chunk2+chunk3) {
                    // Difficulty 1
                    listeChunks[i].UpdateTypeDif("base", 1);
                    Instantiate(prefabBase1, new Vector3(listeChunks[i].GetX(), listeChunks[i].GetY(), 0), Quaternion.identity);
                    chunk1--;
                }
            }
            // Debug.Log(listeChunks[i]);
        }
    }

    void Update() {}

    private int GenRandomCastleNum() {
        /**
         * Retourne un nombre aléatoire
         * entre 0, 6, 28 et 34.
        **/
        List<int> l = new List<int>{0, 6, 28, 34};
        return l[Random.Range(0, l.Count)];
    }

    private List<int> GenRandomFirecamp() {
        /**
         * Retourne une liste de nombres
         * aléatoires correspondant aux
         * positions des feux de camps.
        **/
        int left = 0;
        int right = 0;
        List<int> resListe = new List<int>();
        List<int> fLeft = new List<int>{9, 14, 23};
        List<int> fRight = new List<int>{11, 20, 25};
        for(int i = 0; i < 2; i++){
            left = fLeft[Random.Range(0, fRight.Count)];
            right = fRight[Random.Range(0, fRight.Count)];
            fLeft.Remove(left);
            fRight.Remove(right);
            resListe.Add(left);
            resListe.Add(right);
        }
        return resListe;
    }

    private List<Chunk> GetAdjacents(Chunk c) {
        /**
         * Retourne la liste des chunks
         * adjacents à celui en paramètre
        **/
        List<Chunk> resListe = new List<Chunk>();
        foreach(Chunk ch in listeChunks) {
            if (c.GetX() == ch.GetX() && (c.GetY() == ch.GetY() - 1 || c.GetY() == ch.GetY() + 1)) {
                // Si le X est identique et si le Y est adjacent
                resListe.Add(ch);
            } else if (c.GetY() == ch.GetY() && (c.GetX() == ch.GetX() - 1 || c.GetX() == ch.GetX() + 1)) {
                // Si le Y est identique et si le X est adjacent
                resListe.Add(ch);
            }
        }
        return resListe;
    }
}
