using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class anim : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Animation an = GetComponent<Animation>();
        an.Play("Burn01");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
