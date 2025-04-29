using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CuttingEffectLogic : MonoBehaviour
{
    public ParticleSystem effect;
    public ParticleSystem destoryEffect;
    public void PlayEffect()
    {
        if (effect.isPlaying)
            effect.Stop();
        effect.Play();
    }
    public void PlayDestoryedEffect()
    {
        StartCoroutine(PlayDestoryedEff());
    }
    IEnumerator PlayDestoryedEff()
    {
        destoryEffect.Play();
        while (true)
        {
            if (!destoryEffect.isPlaying)
            {
                GameObject.Destroy(gameObject);
                yield break;
            }
            yield return null;
        }
    }
}
