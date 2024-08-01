using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceScanner : MonoBehaviour
{
    [SerializeField] private float _scanRadius;
    [SerializeField] private float _scanTimeStep;
    [SerializeField] private LayerMask _resourceMask;

    private WaitForSeconds _scanDelay;
    private List<Resource> _resources = new ();

    public event Action<List<Resource>> OnResourcesDiscovered;

    public void Init(Action<List<Resource>> onResourcesDiscovered)
    {
        OnResourcesDiscovered += onResourcesDiscovered;
        _scanDelay = new WaitForSeconds(_scanTimeStep);
        StartCoroutine(Scanning());
    }

    private IEnumerator Scanning()
    {
        while (enabled)
        {
            yield return _scanDelay;
            _resources = new ();
            Physics.OverlapSphere(transform.position, _scanRadius, _resourceMask).ToList().ForEach(collider =>
            {
                if (collider.TryGetComponent(out Resource resource))
                    _resources.Add(resource);
            });

            if (_resources.Count > 0)
                OnResourcesDiscovered.Invoke(_resources);
        }
    }
}