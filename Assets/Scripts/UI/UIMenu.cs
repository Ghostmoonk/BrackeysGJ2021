using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMenu : MonoBehaviour {
    [SerializeField]
    private GameObject confirmationWindow;

    [SerializeField]
    private GameObject resolutionBug;

    void Start() {
        confirmationWindow.SetActive(false);
    }
    
    public void GoPlay() {
        SceneManager.LoadScene("Level 1");
    }

    public void ConfimationActive() {
        confirmationWindow.SetActive(true);
    }

    public void ConfirmationDesactive() {
        confirmationWindow.SetActive(false);
        resolutionBug.GetComponent<UIHoverButtons>().pointerExit();
    }

    public void QuitGame() {
        Application.Quit();
    }
}
