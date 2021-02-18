using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMesh : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int r = Random.Range(0, transform.childCount);
        for(int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).gameObject.SetActive(false);
            if(i==r) {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }
}
