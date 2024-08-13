using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BotMover))]
public class Bot : MonoBehaviour
{
    [SerializeField] private Transform _resourceAttachPoint;

    private Vector3 _collectZonePosition;
    private BotMover _botMover;

    public event Action<Resource> ResourceDelivered;
    public event Action<Bot> ConstructionCompleted;

    public bool IsWorking { get; private set; }

    private void Awake()
    {
		_botMover = GetComponent<BotMover>();
    }

    public void Init(Vector3 collectZonePosition)
    {
        _collectZonePosition = collectZonePosition;
    }

    public void BringResource(Resource resource) => StartCoroutine(ResourceCollecting(resource));
    
    public void BuildBase(BaseCreator baseCreator, Vector3 position, ResourceFinder resourceFinder) => StartCoroutine(BaseBuilding(baseCreator, position, resourceFinder));

    private IEnumerator ResourceCollecting(Resource resource)
    {
        IsWorking = true;
        _botMover.MoveTo(resource.transform.position);
        yield return new WaitUntil(() => _botMover.IsNearTarget());
        AttachResource(resource);
        _botMover.MoveTo(_collectZonePosition);
        yield return new WaitUntil(() => _botMover.IsNearTarget());
        ResourceDelivered?.Invoke(resource);
        ReleaseResource(resource);
        IsWorking = false;
    }

    private IEnumerator BaseBuilding(BaseCreator baseCreator, Vector3 position, ResourceFinder resourceFinder)
    {
        IsWorking = true;
        _botMover.MoveTo(position);
        yield return new WaitUntil(() => _botMover.IsNearTarget());
        baseCreator.Create(position, this, resourceFinder);
        ConstructionCompleted?.Invoke(this);
        IsWorking = false;
    }

    private void AttachResource(Resource resource)
    {
        resource.transform.SetParent(transform);
        resource.transform.position = _resourceAttachPoint.position;
    }
    
    private void ReleaseResource(Resource resource)
    {
        resource.transform.SetParent(null);
        resource.Release();
    }
}