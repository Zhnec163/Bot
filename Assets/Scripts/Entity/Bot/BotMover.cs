using System;
using System.Collections;
using UnityEngine;

public class BotMover : MonoBehaviour
{
    [SerializeField] private float _speed;

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

    public bool IsNearTarget() => (transform.position - _target).sqrMagnitude < Mathf.Epsilon;

    private IEnumerator Moving()
    {
        while (IsNearTarget() == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, _target, _speed * Time.deltaTime);
            yield return null;
        }
    }
}
