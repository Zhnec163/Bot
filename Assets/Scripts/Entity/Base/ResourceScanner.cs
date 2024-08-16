using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Base))]
public class ResourceScanner : MonoBehaviour
{
    [SerializeField] private float _scanRadius;
    [SerializeField] private LayerMask _resourceMask;
    [SerializeField] private ResourceRepository _resourceRepository;
    
    private Base _commandCenter;

    private void Awake()
    {
        _commandCenter = GetComponent<Base>();
    }

    public bool TryGetNearestResource(out Resource resource)
    {
        resource = null;
        _resourceRepository.SynchronizeResources(_commandCenter, ScanResources());

        if (_resourceRepository.TryGetBaseResource(_commandCenter, out Resource nearestResource) == false)
            return false;

        resource = nearestResource;
        return true;
    }

    private List<Resource> ScanResources()
    {
        List<Resource> resources = new List<Resource>();
        Collider[] colliders = Physics.OverlapSphere(transform.position, _scanRadius, _resourceMask);

        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent(out Resource resource))
                resources.Add(resource);
        }

        return resources;
    }
}