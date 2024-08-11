using System;
using UnityEngine;

[RequireComponent(typeof(BotMover))]
public class Bot : MonoBehaviour
{
    [SerializeField] private Transform _resourceAttachPoint;
    [SerializeField] private Base _basePrefab;

    private Vector3 _constructionPosition;
    private BotMover _botMover;
    private Resource _resource;
    private Vector3 _collectZonePosition;
    
    public event Action<Resource> ResourceDelivered;
    public event Action ConstructionCompleted;

    public bool IsWorking { get; private set; }

    private void Awake()
    {
		_botMover = GetComponent<BotMover>();
    }

    public void Init(Vector3 collectZonePosition)
    {
        _collectZonePosition = collectZonePosition;
    }

    public void BuildBase(Vector3 position)
    {
        IsWorking = true;
        _constructionPosition = position;
        _botMover.MoveTo(_constructionPosition, OnArrivedBuildPosition);
    }

    public void BringResource(Resource resource)
    {
        IsWorking = true;
        _resource = resource;
        _botMover.MoveTo(_resource.transform.position, OnArrivedResource);
    }
    
    private void OnArrivedBuildPosition()
    {
        Base commandCenter = Instantiate(_basePrefab, _constructionPosition, _basePrefab.transform.rotation);
        commandCenter.Init(this);
        ConstructionCompleted?.Invoke();
        IsWorking = false;
    }

    private void OnArrivedResource()
    {
        AttachResource();
        _botMover.MoveTo(_collectZonePosition, OnReturnedBase);
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