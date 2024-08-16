using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(BotMover))]
public class Bot : MonoBehaviour
{
    [SerializeField] private Transform _resourceAttachPoint;
    [SerializeField] private BaseBuilder _baseBuilder;

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
    
    public void BuildBase(Vector3 position) => StartCoroutine(BaseBuilding(position));

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

    private IEnumerator BaseBuilding(Vector3 position)
    {
        IsWorking = true;
        _botMover.MoveTo(position);
        yield return new WaitUntil(() => _botMover.IsNearTarget());
        _baseBuilder.Build(position, this);
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