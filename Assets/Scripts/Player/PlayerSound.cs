using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [SerializeField]
    AudioSource footStepSource;

    public void PlayFootstepSound(string footstepName) => SoundManager.Instance.PlaySound(footStepSource, footstepName);
}
