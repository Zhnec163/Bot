using UnityEngine;

[RequireComponent(typeof(SlaveMover))]
public class Slave : MonoBehaviour
{
    [SerializeField] private Transform _resourceAttachPoint;

    private TownHall _townHall;
    private SlaveMover _slaveMover;
    private Resource _resource;

    public bool IsWorking { get; private set; }

    private void Awake()
    {
		_slaveMover = GetComponent<SlaveMover>();
    }

    public void Init(TownHall townHall)
    {
        _townHall = townHall;
    }

    public void BringResource(Resource resource)
    {
        IsWorking = true;
        _resource = resource;
        _slaveMover.MoveTo(_resource.transform.position, OnArrivedResource);
    }

    private void OnArrivedResource()
    {
        AttachResource();
        _slaveMover.MoveTo(_townHall.transform.position, OnReturnedBase);
    }

    private void OnReturnedBase()
    {
        _townHall.AddCollectedResource(_resource);
        ReleaseResource();
        IsWorking = false;
    }

    private void AttachResource()
    {
        _resource.transform.SetParent(transform);
        _resource.transform.position = _resourceAttachPoint.position;
    }
    
    private void ReleaseResource()
    {
        _resource.transform.SetParent(null);
        _resource.Release();
        _resource = null;
    }
}