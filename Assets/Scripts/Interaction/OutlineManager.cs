using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class OutlineManager : MonoBehaviour
{
    [SerializeField] private Material _outlineMaterial;
    private int _outlineIndex;

    [SerializeField] private float _outlineScale = 1.05f;
    [SerializeField] private Renderer _renderer;

    private bool _isOutlined = false;

    private void Start()
    {
        if (_outlineMaterial == null)
        {
            Debug.LogError("Outline material is null");
        }

        // Add outline shader to gameObject
        var renderer = _renderer ?? GetComponent<Renderer>();
        var oldMaterials = renderer.materials;
        var newMaterials = new Material[oldMaterials.Length + 1];
        for (int i = 0; i < oldMaterials.Length; i++)
        {
            newMaterials[i] = oldMaterials[i];
        }
        newMaterials[oldMaterials.Length] = _outlineMaterial;
        _outlineIndex = oldMaterials.Length;

        renderer.materials = newMaterials;
        renderer.materials[_outlineIndex].SetFloat("_Scale", 1f);
    }

    public void GrowOutline()
    {
        if (_isOutlined)
            return;

        StartCoroutine(DoOutline(true));
    }

    public void RemoveOutline()
    {
        if (!_isOutlined)
            return;
        StartCoroutine(DoOutline(false));
    }

    private IEnumerator DoOutline(bool outline)
    {
        _isOutlined = outline;

        // Get Scale of outline
        var renderer = _renderer ?? GetComponent<Renderer>();
        
        var scale = renderer.materials[_outlineIndex].GetFloat("_Scale");

        while ((outline && scale < _outlineScale) || (!outline && scale >= 1))
        {
            if (outline) scale += Time.deltaTime * 2;
            else scale -= Time.deltaTime * 2;
            renderer.materials[_outlineIndex].SetFloat("_Scale", scale);
            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }
}
