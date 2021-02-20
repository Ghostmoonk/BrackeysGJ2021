using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(VisualEffect))]
public class Poof : MonoBehaviour
{
    VisualEffect effect;
    VisualEffectAsset effectAsset;
    [SerializeField]
    float effectDuration;

    private void Start()
    {
        effect = GetComponent<VisualEffect>();
        effect.visualEffectAsset = effectAsset;
        StartCoroutine(PlayAndDestroy());
    }

    public void SetVFX(VisualEffectAsset asset, float duration)
    {
        effectDuration = duration;
        effectAsset = asset;
    }

    IEnumerator PlayAndDestroy()
    {
        effect.Play();
        yield return new WaitForSeconds(effectDuration);
        Destroy(gameObject);
    }
}
