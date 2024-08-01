using UnityEngine;

public class Resource : MonoBehaviour
{
    private ResourceSpawner _resourceSpawner;

    public void Init(ResourceSpawner resourceSpawner)
    {
        _resourceSpawner = resourceSpawner;
    }
    
    public void Attach(Transform parent, Vector3 attachPoint)
    {
        transform.SetParent(parent);
        transform.position = attachPoint;
    }
    
    public void Release()
    {
        _resourceSpawner.Release(this);
        transform.SetParent(null);
    }
}