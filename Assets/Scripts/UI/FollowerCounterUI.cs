using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FollowerCounterUI : MonoBehaviour {
    TextMeshProUGUI[] countersText;
    Dictionary<System.Type, int> typeDict = new Dictionary<System.Type, int>
    {
        {typeof(SimpleFollower),0},
        {typeof(KnightFollower),1},
        {typeof(TorchedFollower),2}
    };

    int[] counters = { 0, 0, 0 };
    int[] totals = { -1, 0, 0 };

    private void Start() {
        PlayerLead.FollowerAdded += addFollower;
        PlayerLead.FollowerRemoved += removeFollower;
        countersText = GetComponentsInChildren<TextMeshProUGUI>();
        for(int i = 0; i < 3; i++) {
            updateTexts(i, 0);

        }
    }

    public void addFollower(Follower follower) {
        updateTexts(typeDict[follower.GetType()], 1);
        
    }

    public void removeFollower(Follower follower) {
        updateTexts(typeDict[follower.GetType()], -1);
    }

    public void increaseTotal(ItemType item) {
        if(item == ItemType.Sword) {
            totals[1] += 1;
            updateTexts(typeDict[typeof(KnightFollower)], 0);
        }
        if(item == ItemType.Torch) {
            totals[2] += 1;
            updateTexts(typeDict[typeof(TorchedFollower)], 0);
        }
    }

    private void updateTexts(int i, int value) {
        counters[i] += value;
        countersText[i].text = totals[i] != -1 ? counters[i] + " / " + totals[i] : counters[i].ToString();
    }

    private void OnDisable() {
        PlayerLead.FollowerAdded -= addFollower;
        PlayerLead.FollowerRemoved -= removeFollower;
    }
}
