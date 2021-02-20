using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour {
    void Start() {
        if (!PlayerPrefs.HasKey("bestScore")) {
            PlayerPrefs.SetInt("bestScore", 0);
        }
    }

    // public void EndGame() {

    // }
}
