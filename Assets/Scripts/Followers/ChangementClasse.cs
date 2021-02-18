using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ChangementClasse : MonoBehaviour
{
    VisualEffect smokePoof;
    GameObject currentModel;
    GameObject saveModel;

    FollowerBehavior followerBehavScript;
    public GameObject TorcheModel;
    public GameObject ChevalierModel;
    string Type = "Villager";
    // Start is called before the first frame update
    void Start()
    {
        smokePoof = GetComponent<VisualEffect>();
        currentModel = this.gameObject.transform.GetChild(0).gameObject;
        followerBehavScript = GetComponent<FollowerBehavior>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(onSwitch(0.2f, "Chevalier"));
            
        }

    }

    IEnumerator onSwitch(float waitTime, string TypeCharacter)
    {
        smokePoof.Play();
        yield return new WaitForSeconds(waitTime);
        

        if (TypeCharacter =="Torche")
        {
            print("je suis une torche");
            saveModel = currentModel;
            Destroy(currentModel);
            currentModel = Instantiate(TorcheModel, currentModel.transform.position, currentModel.transform.rotation);
            currentModel.transform.SetParent(this.gameObject.transform);
            followerBehavScript.ChangeAnimator(currentModel.GetComponent<Animator>());
            
            currentModel = TorcheModel;
            
        }

        if (TypeCharacter == "Chevalier")
        {
            print("je suis un chevalier");
            saveModel = currentModel;
            Destroy(currentModel);
            currentModel = Instantiate(ChevalierModel, currentModel.transform.position, currentModel.transform.rotation);
            currentModel.transform.SetParent(this.gameObject.transform);
            followerBehavScript.ChangeAnimator(currentModel.GetComponent<Animator>());

            currentModel = ChevalierModel;

        }
    }
}
