using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour {
    private int questCleared;
    private QuestUI UI;
    private GameObject Crowd;
    private bool conditionClear = false;
    private int randNumQuest;
    private int conditionInt;
    private int conditionIntClear;

    [SerializeField]
    List<GameObject> rewards;

    [SerializeField]
    Transform spawnReward;

    [SerializeField] float delayNextQuest;
    [SerializeField] Vector2Int MinMaxVillager = new Vector2Int(4, 10);
    [SerializeField] Vector2Int minMaxMonster = new Vector2Int(2, 5);

    void Start() {
        Enemy.OnDieEvent += killedEnemy;
        PlayerLead.FollowerAdded += addedVillager;

        questCleared = 0;
        conditionIntClear = -1;
        UI = GameObject.Find("QuestDisplay").GetComponent<QuestUI>();
        // Crowd = GameObject.Find("Crowd").GetComponent<CrowdManager>();

        StartCoroutine(NewQuest());
    }

    void Update() {
        StartCoroutine(IsConditionCleared());
        if (conditionClear) {
            QuestClear();
            StartCoroutine(NewQuest());

        }
    }

    IEnumerator IsConditionCleared() {
        /**
         * Vérifie si la quête en cours est complétée
        **/
        if (randNumQuest == 0) {
            // avoir un certain nombre de villageois
            if (conditionIntClear == 0) {
                conditionClear = true;
                yield break;
            }
        } else if (randNumQuest == 1) {
            // tuer un certain nombre de monstres
            if (conditionIntClear == 0) {
                conditionClear = true;
                yield break;
            }
        }
    }

    private void QuestClear() {
        /**
         * Fini une quête et attribue les récompenses
        **/
        conditionClear = false;
        questCleared += 1;
        Instantiate(rewards[Random.Range(0, rewards.Count)], 
            spawnReward.position + (spawnReward.forward * 4),
            Quaternion.identity);
        
    }

    private IEnumerator NewQuest() {
        /**
         * Crée une nouvelle quête
        **/
        StartCoroutine(UI.questCleared());
        ResRandNum();
        if (randNumQuest == 0) {
            // avoir un certain nombre de villageois
            conditionInt = Random.Range(MinMaxVillager.x, MinMaxVillager.y); // Le nombre de villageois à obtenir
            // conditionInt += Crowd.GetNb(); // GET LE NOMBRE ACTUEL DE VILLAGEOIS \\
        } else if (randNumQuest == 1) {
            // tuer un certain nombre de monstres
            conditionInt = Random.Range(minMaxMonster.x, minMaxMonster.y); // Le nombre de monstres à tuer
        }
        conditionIntClear = conditionInt;
        yield return new WaitForSeconds(delayNextQuest);
        UI.SetQuestCondition(randNumQuest, conditionIntClear, conditionInt); // Envoi des infos à l'UI
    }

    private void ResRandNum() {
        /**
         * Reset l'identifiant de quête
        **/
        randNumQuest = Random.Range(0, 1);
    }


    private void AddToQuestCondition(int _numQuest) {
        /**
         * Fonction appelée quand une condition de quête est remplie
        **/
        if (randNumQuest == _numQuest) {
            conditionIntClear -= 1;
            UI.SetQuestCondition(randNumQuest, conditionIntClear, conditionInt);
        }
    }

    public void addedVillager(Follower follower) {
        AddToQuestCondition(0);
    }
    public void killedEnemy(Enemy enemy) {
        AddToQuestCondition(1);
    }

    public int GetNumQuestCleard() {
        return questCleared;
    }
}
