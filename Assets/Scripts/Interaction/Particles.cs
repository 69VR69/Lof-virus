using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Particles : MonoBehaviour
{
    [SerializeField] private VisualEffect _visualEffects;


    public void Start()
    {
        _visualEffects.Stop();
    }

    public void Play()
    {
        _visualEffects.Play();
    }

    public void Stop()
    {
        _visualEffects.Stop();
    }
}
