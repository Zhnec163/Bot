using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Bot))]
public class BotMover : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _minimumDistanceToTarget = 0.2f;

    private Vector3 _target;
    private Coroutine _currentMovement;

    public void MoveTo(Vector3 target)
    {
        if (_currentMovement != null)
            StopCoroutine(_currentMovement);

        _target = target;
        transform.LookAt(_target);
        _currentMovement = StartCoroutine(Moving());
    }

    public bool IsNearTarget() => Vector3.Distance(transform.position, _target) < _minimumDistanceToTarget;

    private IEnumerator Moving()
    {
        while (IsNearTarget() == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, _target, _speed * Time.deltaTime);
            yield return null;
        }
    }
}
