using System.Collections;
using System.Collections.Generic;

using UnityEditor.Experimental.GraphView;

using UnityEngine;

public class Bangeable : MonoBehaviour
{
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
        yield return new WaitForSeconds(3);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        Destroy(gameObject);
    }
}
