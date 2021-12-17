using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CXRayTarget : MonoBehaviour
{
    public Material alphaMaterial;

    private Dictionary<MeshRenderer, Material[]> _hiddenObjects;
    private MeshRenderer[] _renderers;
    private bool _hidden;
    private HashSet<GameObject> _hiders;
    private HashSet<GameObject> _toBeShown;

    private void Awake()
    {
        _hiddenObjects = new Dictionary<MeshRenderer, Material[]>();
        _hidden = false;
        _hiders = new HashSet<GameObject>();
        _toBeShown = new HashSet<GameObject>();
    }

    private void Start()
    {
        _renderers = GetComponentsInChildren< MeshRenderer >();
        foreach (var childRenderer in _renderers)
        {
            _hiddenObjects.Add(childRenderer, childRenderer.materials);
        }
    }
    
    public void Hide(GameObject hider)
    {
        if (_toBeShown.Contains(hider))
        {
            _toBeShown.Remove(hider);
        }

        if(_hiders.Contains(hider))
            return;

        _hiders.Add(hider);

        if(_hidden)
            return;

        foreach (var childRenderer in _renderers)
        {
            var newMaterials = new Material[childRenderer.materials.Length];
            for(var i=0; i<newMaterials.Length; i++)
            {
                newMaterials[i] = alphaMaterial;
            }

            childRenderer.materials = newMaterials;
        }

        _hidden = true;
    }

    private IEnumerator DelayedShow(GameObject reveler)
    {
        yield return new WaitForSeconds(2.0f);

        if (!_hidden || !_toBeShown.Contains(reveler)) yield break;
        _toBeShown.Remove(reveler);

        if (!_hiders.Contains(reveler)) yield break;
        _hiders.Remove(reveler);

        foreach (var childRenderer in _renderers)
        {
            Material[] restoredMaterials;
            _hiddenObjects.TryGetValue(childRenderer, out restoredMaterials);

            if (childRenderer != null)
            {
                childRenderer.materials = restoredMaterials;
            }
        }

        if (_hiders.Count == 0)
            _hidden = false;
    }

    public void Show(GameObject reveler)
    {
        if (_toBeShown.Contains(reveler))
            return;

        _toBeShown.Add(reveler);
        StartCoroutine(DelayedShow(reveler));
    }
}