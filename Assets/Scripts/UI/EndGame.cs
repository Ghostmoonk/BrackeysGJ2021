using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndGame : MonoBehaviour {

    [SerializeField]
    private GameObject CanvasInfo;
    private GameObject MainCharacter;
    private GameObject CanvasEndGame;
    private GameObject TextScore;
    void Start() {
        CanvasEndGame = GameObject.FindGameObjectsWithTag("CanvasEndGame")[0];
        TextScore = GameObject.FindGameObjectsWithTag("TextEndGame")[0];
        MainCharacter = GameObject.FindGameObjectsWithTag("Player")[0];
        CanvasInfo.SetActive(false);
        CanvasEndGame.SetActive(false);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            End();
        }
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.CompareTag("Player")) {
            CanvasInfo.SetActive(true);
        }
    }

    void OnTriggerExit(Collider collider) {
        if (collider.CompareTag("Player")) {
            CanvasInfo.SetActive(false);
        }
    }

    private void End() {
        int nbFollowers = MainCharacter.GetComponent<PlayerLead>().GetFollowers().Count;
        CanvasEndGame.SetActive(true);
        string t = "Congratulations !\n\n";
        if (!PlayerPrefs.HasKey("bestScore") || nbFollowers > PlayerPrefs.GetInt("bestScore")) {
            PlayerPrefs.SetInt("bestScore", nbFollowers);
            if (nbFollowers == 1) {
                t += "You beat your record : " + nbFollowers.ToString() + " villager saved.";
            } else {
                t += "You beat your record : " + nbFollowers.ToString() + " villagers saved.";
            }
        } else {
            if (nbFollowers == 1) {
                t += "You saved " + nbFollowers.ToString() + " villager, try harder !";
            } else {
                t += "You saved " + nbFollowers.ToString() + " villagers, try harder !";
            }
        }
        TextScore.GetComponent<TextMeshProUGUI>().text = t;
    }
}
