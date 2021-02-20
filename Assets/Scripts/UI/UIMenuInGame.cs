using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMenuInGame : MonoBehaviour {
    [SerializeField]
    private GameObject menuInGame;
    void Start() {
        menuInGame.SetActive(false);
    }

    void Update() {
        if (Input.GetKeyDown("escape")) {
            if (menuInGame.activeSelf) {
                MenuDesactive();
            } else {
                MenuActive();
            }
            
        }
    }

    public void MenuActive() {
        menuInGame.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void MenuDesactive() {
        menuInGame.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void GoToMenu() {
        SceneManager.LoadScene("menu");
    }
}
