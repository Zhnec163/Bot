using System;
using UnityEngine;

[RequireComponent(typeof(SlaveMover))]
public class Slave : MonoBehaviour
{
    [SerializeField] private Transform _resourceAttachPoint;

    private TownHall _townHall;
    private SlaveMover _slaveMover;
    private Resource _resource;
    private Action<Resource> _onResourceDelivered;

    public bool IsWorking { get; private set; }

    private void Awake()
    {
        _slaveMover = GetComponent<SlaveMover>();
    }

    public void Init(TownHall townHall, Action<Resource> onResourceDelivered)
    {
        _townHall = townHall;
        _onResourceDelivered += onResourceDelivered;
    }

    public void Collect(Resource resource)
    {
        IsWorking = true;
        _resource = resource;
        _slaveMover.MoveTo(_resource.transform.position, HandleArrivalToResource);
    }

    private void HandleArrivalToResource()
    {
        _resource.Attach(transform, _resourceAttachPoint.position);
        _slaveMover.MoveTo(_townHall.transform.position, HandleReturnOnBase);
    }

    private void HandleReturnOnBase()
    {
        _onResourceDelivered?.Invoke(_resource);
        _resource.Release();
        _resource = null;
        IsWorking = false;
    }
}