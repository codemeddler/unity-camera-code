using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CXRayView : MonoBehaviour
{
    public LayerMask blockLayer;
    public string blockTag;
    public float checkBuffer;

    private Transform _myTransform;
    private Transform _playerTransform;
    private Dictionary<GameObject, CXRayTarget> _hiddenObjects;

    private void Awake()
    {
        _hiddenObjects = new Dictionary<GameObject, CXRayTarget>();
        _myTransform = transform;
    }

    /*public void OnDestroy()
    {
        if (_hiddenObjects.Keys.Count <= 0)
            return;

        var keys = new GameObject[_hiddenObjects.Keys.Count];
        _hiddenObjects.Keys.CopyTo(
            keys,
            0);

        foreach (var hiddenObject in keys)
        {
            Material restoredMaterial;
            _hiddenObjects.TryGetValue(
                hiddenObject,
                out restoredMaterial);

            if (hiddenObject != null)
            {
                var renderers = hiddenObject.GetComponentsInChildren<MeshRenderer>();

                foreach (var hiddenRenderer in renderers)
                {
                    hiddenRenderer.material = restoredMaterial;
                }
            }

            _hiddenObjects.Remove(hiddenObject);
        }
    }*/

    public void LateUpdate()
    {
        if (_playerTransform == null)
            _playerTransform = GameObject.FindWithTag("Player").transform;

        // Calculate needed values for the check
        var position1 = _playerTransform.position;
        var position = position1;
        var checkDir = position1 - _myTransform.position;
        position += checkDir * checkBuffer;
        var checkDistance = checkDir.magnitude;
        checkDir.Normalize();

        // Do the collision check
        var blockers = Physics.SphereCastAll(
            position,
            checkBuffer,
            checkDir,
            checkDistance,
            blockLayer);

        // If there where blockers, iterate through them
        if (blockers.Length > 0)
        {
            foreach (var blocker in blockers)
            {
                var hitObject = blocker.transform.gameObject;

                if (!hitObject.CompareTag(blockTag))
                    continue;
                
                var sourceObject = hitObject;
                if (hitObject.transform.parent != null)
                {
                    sourceObject = hitObject.transform.parent.gameObject;
                }
                
                if(_hiddenObjects.ContainsKey(hitObject))
                    continue;

                var target = sourceObject.GetComponent<CXRayTarget>();
                target.Hide(hitObject);
                _hiddenObjects.Add(hitObject, target);
            }
        }

        var keys = new GameObject[_hiddenObjects.Keys.Count];
        _hiddenObjects.Keys.CopyTo(keys, 0);

        foreach (var t in keys)
        {
            var bFound = blockers.Select(t1 => t1.transform.gameObject).Any(hitObject => hitObject == t);

            if (bFound)
                continue;

            CXRayTarget target;
            _hiddenObjects.TryGetValue(t, out target);

            if (target != null)
            {
                target.Show(t);
            }

            _hiddenObjects.Remove(t);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag(blockTag))
            return;

        var contact = other.gameObject;
        if (other.transform.parent != null)
            contact = other.transform.parent.gameObject;

        var hider = contact.GetComponent<CXRayTarget>();
        if (hider == null)
            return;

        hider.Hide(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag(blockTag))
            return;

        var contact = other.gameObject;
        if (other.transform.parent != null)
            contact = other.transform.parent.gameObject;
        
        var hider = contact.GetComponent<CXRayTarget>();
        if (hider == null)
            return;

        hider.Show(other.gameObject);
    }
}