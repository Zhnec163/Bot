using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceFinder : MonoBehaviour
{
    [SerializeField] private LayerMask _resourceMask;

    private List<Resource> _foundedResources = new ();

    public Resource GetNearestResource(Vector3 origin, float radius)
    {
        Resource nearestResource = null;
        Collider[] colliders = Physics.OverlapSphere(origin, radius, _resourceMask);

        if (colliders.Length > 0)
        {
            List<Resource> resources = new();

            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent(out Resource resource))
                    resources.Add(resource);
            }

            resources = resources.Except(_foundedResources).ToList();

            if (resources.Count > 0)
            {
                nearestResource = resources.OrderByDescending(resource => Vector3.Distance(origin, resource.transform.position)).FirstOrDefault();
                _foundedResources.Add(nearestResource);
                nearestResource.Delivered += OnDelivered;
            }
        }

        return nearestResource;
    }

    private void OnDelivered(Resource resource)
    {
        _foundedResources.Remove(resource);
        resource.Delivered -= OnDelivered;
    } 
}