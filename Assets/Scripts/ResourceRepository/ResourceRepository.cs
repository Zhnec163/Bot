using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceRepository : MonoBehaviour
{
    private List<ResourceRepositoryItem> _repository = new();

    public void SynchronizeResources(Base commandCenter, List<Resource> resources)
    {
        foreach (var resource in resources)
        {
            if (_repository.Select(resourceRepositoryItem => resourceRepositoryItem.Resource).Contains(resource) == false)
                _repository.Add(new ResourceRepositoryItem(commandCenter, resource));
        }
    }

    public bool TryGetBaseResource(Base commandCenter, out Resource resource)
    {
        resource = null;
        ResourceRepositoryItem resourceRepositoryItem = _repository.Where(resourceRepositoryItem => resourceRepositoryItem.Owner == commandCenter)
            .Where(resourceRepositoryItem => resourceRepositoryItem.IsProcessed == false)
            .OrderBy(resourceRepositoryItem => (commandCenter.transform.position - resourceRepositoryItem.Resource.transform.position).sqrMagnitude)
            .FirstOrDefault();

        if (resourceRepositoryItem != null)
        {
            resourceRepositoryItem.Process();
            resource = resourceRepositoryItem.Resource;
            resource.Delivered += OnDelivered;
            return true;
        }

        return false;
    }

    private void OnDelivered(Resource resource)
    {
        ResourceRepositoryItem resourceRepositoryItem = _repository.Where(resourceRepositoryItem => resourceRepositoryItem.Resource == resource).FirstOrDefault();
        resourceRepositoryItem.Resource.Delivered -= OnDelivered;
        _repository.Remove(resourceRepositoryItem);
    }
}
