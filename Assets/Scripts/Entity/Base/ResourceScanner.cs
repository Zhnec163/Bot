using System.Collections.Generic;
using UnityEngine;

public class ResourceScanner : MonoBehaviour
{
    [SerializeField] private float _scanRadius;
    [SerializeField] private LayerMask _resourceMask;

    public List<Resource> Search()
    {
        List<Resource> resources = new();
        Collider[] colliders = Physics.OverlapSphere(transform.position, _scanRadius, _resourceMask);

        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent(out Resource resource))
                resources.Add(resource);
        }

        return resources;
    }
}