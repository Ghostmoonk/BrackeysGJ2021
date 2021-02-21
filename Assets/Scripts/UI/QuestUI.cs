using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestUI : MonoBehaviour
{

    private TextMeshProUGUI text;
    Dictionary<int, string> questLabel = new Dictionary<int, string>() {
        {0, "Save {0}/{1} villagers"},
        {1, "Kill {0}/{1} monsters"},
    };

    [SerializeField]
    Color ColorQuestComplete;
    [SerializeField]
    Color ColorQuest;

    private void Awake() {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void Start() {
        text.text = "waiting for next quest ...";
    }

    public IEnumerator questCleared() {
        text.color = ColorQuestComplete;
        yield return new WaitForSeconds(2);
        text.text = "waiting for next quest ...";
    }
    public void SetQuestCondition(int questID, int current, int total) {
        text.color = ColorQuest;
        text.text = System.String.Format(questLabel[questID], total - current, total);
    }
}
