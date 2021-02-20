using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestUI : MonoBehaviour
{

    private TextMeshProUGUI text;
    Dictionary<int, string> questLabel = new Dictionary<int, string>() {
        {1, "Save {0}/{1} villagers"},
        {0, "Kill {0}/{1} monsters"},
        {2, "Discover {0}/{1} villages"},
    };

    private void Awake() {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void Start() {
    }
    public void SetQuestCondition(int questID, int current, int total) {
        Debug.Log(System.String.Format(questLabel[questID], total - current, total));
        text.text = System.String.Format(questLabel[questID], total - current, total);
    }
}
