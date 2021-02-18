// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class QuestManager : MonoBehaviour {
//     private int questCleared;
//     private GameObject UI;
//     private GameObject Crowd;
//     private bool conditionClear = false;
//     private int randNumQuest;
//     private int conditionInt;
//     private int conditionIntClear;

//     void Start() {
//         questCleared = 0;
//         conditionIntClear = -1;
//         UI = GameObject.Find("UI").GetComponent<UIManager>();
//         Crowd = GameObject.Find("Crowd").GetComponent<CrowdManager>();

//         NewQuest();
//     }

//     void Update() {
//         StartCoroutine(IsConditionCleared());
//         if (conditionClear) {
//             QuestClear();
//             NewQuest();
//         }
//     }

//     IEnumerator IsConditionCleared() {
//         /**
//          * Vérifie si la quête en cours est complétée
//         **/
//         if (randNumQuest == 0) {
//             // avoir un certain nombre de villageois
//             if (conditionIntClear == 0) {
//                 conditionClear = true;
//                 yield break;
//             }
//         } else if (randNumQuest == 1) {
//             // tuer un certain nombre de monstres
//             if (conditionIntClear == 0) {
//                 conditionClear = true;
//                 yield break;
//             }
//         } else if (randNumQuest == 2) {
//             // explorer une partie de la carte
//             if (conditionIntClear == 0) {
//                 conditionClear = true;
//                 yield break;
//             }
//         } else if (randNumQuest == 3) {} else if (randNumQuest == 4) {}
//     }

//     private void QuestClear() {
//         /**
//          * Fini une quête et attribue les récompenses
//         **/
//         conditionClear = false;
//         questCleared += 1;
//         if (randNumQuest == 0) {
//             // avoir un certain nombre de villageois
//             Crowd.SetPanicLevel(conditionInt);
//         } else if (randNumQuest == 1) {
//             // tuer un certain nombre de monstres
//             Crowd.SetPanicLevel(conditionInt*2);
//             // BUFF TORCHE
//         } else if (randNumQuest == 2) {
//             // explorer une partie de la carte
//             UI.SetMarkRare();
//         } else if (randNumQuest == 3) {} else if (randNumQuest == 4) {}
//     }

//     private void NewQuest() {
//         /**
//          * Crée une nouvelle quête
//         **/
//         ResRandNum();
//         if (randNumQuest == 0) {
//             // avoir un certain nombre de villageois
//             conditionInt = Random.Range(5, 10); // Le nombre de villageois à obtenir
//             conditionInt += Crowd.GetNb(); // GET LE NOMBRE ACTUEL DE VILLAGEOIS \\
//         } else if (randNumQuest == 1) {
//             // tuer un certain nombre de monstres
//             conditionInt = Random.Range(1, 3); // Le nombre de monstres à tuer
//         } else if (randNumQuest == 2) {
//             // explorer une partie de la carte
//             conditionInt = Random.Range(2, 6); // Le nombre de chunks à explorer
//         } else if (randNumQuest == 3) {} else if (randNumQuest == 4) {}
//         conditionIntClear = conditionInt;
//         UI.SetQuestCondition(randNumQuest, conditionInt); // Envoi des infos à l'UI
//     }

//     private void ResRandNum() {
//         /**
//          * Reset l'identifiant de quête
//         **/
//         randNumQuest = Random.Range(0, 5);
//     }


//     public void AddToQuestCondition(int _numQuest, int _conditionInt = 1) {
//         /**
//          * Fonction appelée quand une condition de quête est remplie
//         **/
//         if (randNumQuest == _numQuest) {
//             conditionIntClear -= _conditionInt;
//         }
//     }

//     public int GetNumQuestCleard() {
//         return questCleared;
//     }
// }
