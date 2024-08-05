using System;
using UnityEngine;

[RequireComponent(typeof(SlaveMover))]
public class Slave : MonoBehaviour
{
    [SerializeField] private Transform _resourceAttachPoint;

    private Vector3 _townHallPosition;
    private SlaveMover _slaveMover;
    private Resource _resource;
    private Action<Resource> _onResourceDelivered;

    public bool IsWorking { get; private set; }

    private void Awake()
    {
		_slaveMover = GetComponent<SlaveMover>();
    }

    public void Init(Vector3 townHallPosition, Action<Resource> onResourceDelivered)
    {
        _townHallPosition = townHallPosition;
        _onResourceDelivered += onResourceDelivered;
    }

    public void Collect(Resource resource)
    {
        IsWorking = true;
        _resource = resource;
        _slaveMover.MoveTo(_resource.transform.position, OnArrivedResource);
    }

    private void OnArrivedResource()
    {
        _resource.transform.SetParent(transform);
        _resource.transform.position = _resourceAttachPoint.position;
        _slaveMover.MoveTo(_townHallPosition, OnReturnedBase);
    }

    private void OnReturnedBase()
    {
        _onResourceDelivered?.Invoke(_resource);
        _resource.transform.SetParent(null);
        _resource.Release();
        _resource = null;
        IsWorking = false;
    }
}