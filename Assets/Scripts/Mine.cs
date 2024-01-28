using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.VFX;

public class Mine : MonoBehaviour
{
    private VisualEffect _fart;
    private Light _light;

    [SerializeField]
    private float _fartDelay = 1.5f;
    [SerializeField]
    private string _collisionTag;

    private void Awake()
    {
        _fart = GetComponent<VisualEffect>();
        _light = GetComponent<Light>();
    }

    private void Start()
    {
        _fart.enabled = true;
        _fart.Stop();
        _light.enabled = true;
    }

    public void Explode()
    {
        _fart.Play();
        _light.enabled = false;
        StartCoroutine("StopFart");
    }

    private IEnumerator StopFart()
    {
        yield return new WaitForSeconds(_fartDelay);

        _fart.Stop();
    }
}
