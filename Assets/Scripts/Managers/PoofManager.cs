using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PoofManager : MonoBehaviour
{
    private static PoofManager instance;
    public static PoofManager Instance
    {
        get
        {
            return instance;
        }
    }

    [SerializeField]
    VFXAssetDuration[] poofsVFXAssets;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void InstantiatePoof(Transform spawnTransform)
    {
        GameObject obj = new GameObject("Poof");
        Poof poof = obj.AddComponent<Poof>();
        VFXAssetDuration randomVFX = poofsVFXAssets[Random.Range(0, poofsVFXAssets.Length - 1)];

        poof.SetVFX(randomVFX.VFXasset, randomVFX.duration);
        obj.transform.position = spawnTransform.position;
        obj.transform.rotation = spawnTransform.rotation;
        obj.transform.SetParent(transform);
    }
}

[System.Serializable]
public class VFXAssetDuration
{
    public VisualEffectAsset VFXasset;
    public float duration;
}
