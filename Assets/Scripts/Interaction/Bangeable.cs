using System.Collections;
using System.Collections.Generic;

using UnityEditor.Experimental.GraphView;

using UnityEngine;

public class Bangeable : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] _audioClips;
    [SerializeField]
    private AudioSource _audioSource;
    private Rigidbody rb;

    [SerializeField]
    private float _ejectionForce = 10;
    [SerializeField]
    private Rigidbody _rigidbody;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine(Eject());
        }
    }

    private IEnumerator Eject()
    {
        Vector3 direction = transform.up + transform.right + (-transform.forward);
        Vector3 force = direction * _ejectionForce;
        _rigidbody.AddForce(force, ForceMode.VelocityChange);
        rb.AddForce(force, ForceMode.VelocityChange);
        rb.AddTorque(force, ForceMode.VelocityChange);
        _audioSource.clip = _audioClips[Random.Range(0, _audioClips.Length)];
        _audioSource.Play();
        yield return new WaitForSeconds(3);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        Destroy(gameObject);
    }
}
