using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {
    [Tooltip("Liste des modèles de chunk château"), SerializeField]
    private List<GameObject> prefabCastle;
    [Tooltip("Liste des modèles de chunk spawn"), SerializeField]
    private List<GameObject> prefabSpawn;
    [Tooltip("Liste des modèles de chunk feu de camp"), SerializeField]
    private List<GameObject> prefabFirecamp;
    [Tooltip("Liste des modèles de chunk base 1"), SerializeField]
    private List<GameObject> prefabBase1;
    [Tooltip("Liste des modèles de chunk base 2"), SerializeField]
    private List<GameObject> prefabBase2;
    [Tooltip("Liste des modèles de chunk base 3"), SerializeField]
    private List<GameObject> prefabBase3;

    [Space]
    [Tooltip("Taille des chunks"), SerializeField]
    private int tailleChunks;
    
    private List<Chunk> listeChunks;

    public GameObject ChunkHolder;

    void Start() {
        GameObject chCache;
        int curDif;
        int randomCastle = GenRandomCastleNum();
        List<int> randomFirecamp = GenRandomFirecamp();
        int chunk3 = Random.Range(2, 6);
        int chunk2 = Random.Range(5, 11);
        int chunk1 = 29 - chunk3 - chunk2;
        //Debug.Log(chunk1.ToString() + " " + chunk2.ToString() + " " + chunk3.ToString());
        listeChunks = new List<Chunk>();
        for(int i = 0; i < 35; i++) {
            Debug.Log("oskour");
            listeChunks.Add(new Chunk(i%7, i/7));
            if (i == 17) {
                // Spawn
                listeChunks[i].UpdateTypeDif("spawn");
                chCache = prefabSpawn[Random.Range(0, prefabSpawn.Count)];
                curDif = 0;
            } else if (i == randomCastle) {
                // Castle
                listeChunks[i].UpdateTypeDif("castle");
                chCache = prefabCastle[Random.Range(0, prefabCastle.Count)];
                curDif = 0;
            } else if (randomFirecamp.Contains(i)) {
                // Firecamp
                listeChunks[i].UpdateTypeDif("firecamp");
                chCache = prefabFirecamp[Random.Range(0, prefabFirecamp.Count)];
                curDif = 0;
            } else {
                // Base chunks
                int randCh = Random.Range(0, chunk1+chunk2+chunk3);
                if (randCh >= 0 && randCh < chunk3) {
                    // Difficulty 3
                    listeChunks[i].UpdateTypeDif("base", 3);
                    chCache = prefabBase3[Random.Range(0, prefabBase3.Count)];
                    chunk3--;
                    curDif = 3;
                } else if (randCh >= chunk3 && randCh < chunk2+chunk3) {
                    // Difficulty 2
                    listeChunks[i].UpdateTypeDif("base", 2);
                    chCache = prefabBase2[Random.Range(0, prefabBase2.Count)];
                    chunk2--;
                    curDif = 2;
                } else if (randCh >= chunk2+chunk3 && randCh <= chunk1+chunk2+chunk3) {
                    // Difficulty 1
                    listeChunks[i].UpdateTypeDif("base", 1);
                    chCache = prefabBase1[Random.Range(0, prefabBase1.Count)];
                    chunk1--;
                    curDif = 1;
                } else {
                    chCache = prefabBase1[0];
                    curDif = 0;
                }
            }
            GameObject lastObject = Instantiate(chCache, new Vector3(listeChunks[i].GetX()*tailleChunks, 0, listeChunks[i].GetY()*tailleChunks), Quaternion.identity);

            lastObject.transform.SetParent(ChunkHolder.transform);
            lastObject.GetComponent<SpawnManager>().SetDifficulty(curDif);
            //Debug.Log(listeChunks[i]);
        }
    }

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

    public List<Chunk> GetChunks() {
        return listeChunks;
    }
}
