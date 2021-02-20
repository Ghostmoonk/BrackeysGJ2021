using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIHoverButtons : MonoBehaviour {
    public TMP_Text text;
    public bool danger;
    public Color32 origin_color;
    private Color32 _gray_black;
    private Color32 _black;
    private Color32 _red;

    
    void Start() {
        _gray_black = new Color32(70, 70, 70, 255);
        _black = new Color32(0, 0, 0, 255);
        _red = new Color32(130, 0, 0, 255);
    }

    public void pointerEnter() {
        if (danger) {
            text.color = _red;
        } else {
            if (origin_color.Equals(new Color32(160, 160, 160, 255))) {
                text.color = _gray_black;
            } else if (origin_color.Equals(new Color32(50, 50, 50, 255))) {
                text.color = _black;
            }
        }
    }

    public void pointerExit() {
        text.color = origin_color;
    }
}
