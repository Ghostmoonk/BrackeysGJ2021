using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk {
    private int positionX;
    private int positionY;
    private int difficulty;
    private string type;

    public Chunk(int _positionX, int _positionY) {
        this.positionX = _positionX;
        this.positionY = _positionY;
        this.difficulty = 0;
        this.type = "";
    }

    public void UpdateTypeDif(string _type, int _difficulty = 0) {
        this.difficulty = _difficulty;
        this.type = _type;
    }

    public override string ToString() {
        string pos = this.positionX.ToString() + "|" + this.positionY.ToString();
        return pos + " - " + this.type + " - " + this.difficulty;
    }

    public int GetX() { return this.positionX; }
    public int GetY() { return this.positionY; }
    public string GetTypeCh() { return this.type; }
    public int GetDifficulty() { return this.difficulty; }
}