using System;
using UnityEngine;

[RequireComponent(typeof(BotMover))]
public class Bot : MonoBehaviour
{
    [SerializeField] private Transform _resourceAttachPoint;

    private BotMover _botMover;
    private Resource _resource;
    private Transform _collectZone;
    
    public event Action<Resource> ResourceDelivered;

    public bool IsWorking { get; private set; }

    private void Awake()
    {
		_botMover = GetComponent<BotMover>();
    }

    public void Init(Base commandCenter)
    {
        _collectZone = commandCenter.CollectZone;
    }

    public void BringResource(Resource resource)
    {
        IsWorking = true;
        _resource = resource;
        _botMover.MoveTo(_resource.transform.position, OnArrivedResource);
    }

    private void OnArrivedResource()
    {
        AttachResource();
        _botMover.MoveTo(_collectZone.position, OnReturnedBase);
    }

    private void OnReturnedBase()
    {
        ResourceDelivered?.Invoke(_resource);
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