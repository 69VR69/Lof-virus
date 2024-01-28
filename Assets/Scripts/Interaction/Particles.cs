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
        _visualEffects.enabled = true;
        _visualEffects.Play();
    }

    public void Stop()
    {
        _visualEffects.Stop();
    }

    public void Toggle()
    {
        var list = new List<string>();
        _visualEffects.GetSpawnSystemNames(list);
        var vinfo = _visualEffects.GetSpawnSystemInfo(list[0]);
        if (vinfo.playing)
            Stop();
        else
            Play();
    }
}
