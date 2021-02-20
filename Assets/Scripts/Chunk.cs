using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk {
    /**
     * Classe de chunk
     * positionX, positionY : la position du chunk
     * difficulty : la difficulté du chunk (si c'est un chunk de base)
     * type : le type du chunk
    **/
    private int positionX;
    private int positionY;
    private int difficulty;
    // spawn - castle - firecamp - base
    private string type;
    

    public Chunk(int _positionX, int _positionY) {
        /**
         * Constructeur par défaut
        **/
        this.positionX = _positionX;
        this.positionY = _positionY;
        this.difficulty = 0;
        this.type = "";
    }

    public void UpdateTypeDif(string _type, int _difficulty = 0) {
        /**
         * Setter de la difficulté et du type du chunk
        **/
        this.difficulty = _difficulty;
        this.type = _type;
    }

    public override string ToString() {
        /**
         * Affichage en console
        **/
        string pos = this.positionX.ToString() + "|" + this.positionY.ToString();
        return pos + " - " + this.type + " - " + this.difficulty;
    }

    public int GetX() { return this.positionX; }
    public int GetY() { return this.positionY; }
    public string GetTypeCh() { return this.type; }
    public int GetDifficulty() { return this.difficulty; }
}