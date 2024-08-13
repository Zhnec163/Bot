using UnityEngine;

[RequireComponent(typeof(Base))]
public class ResourceScanner : MonoBehaviour
{
    [SerializeField] private float _scanRadius;
    [SerializeField] private ResourceFinder _resourceFinder;

    public ResourceFinder ResourceFinder => _resourceFinder;

    public void Init(ResourceFinder resourceFinder)
    {
        _resourceFinder = resourceFinder;
    }

    public bool TryGetNearestResource(out Resource nearestResource)
    {
        nearestResource = _resourceFinder.GetNearestResource(transform.position, _scanRadius);

        if (nearestResource == null)
            return false;

        return true;
    }
}