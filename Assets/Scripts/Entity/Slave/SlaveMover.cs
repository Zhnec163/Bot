using System;
using System.Collections;
using UnityEngine;

public class SlaveMover : MonoBehaviour
{
    [SerializeField] private float _speed;

    private float _minDistance = 0.2f;
    private float _minSqrDistance;

    public void Awake()
    {
        _minSqrDistance = _minDistance * _minDistance;
    }
    
    public void MoveTo(Vector3 position, Action onFinishedMovement)
    {
        transform.LookAt(position);
        StartCoroutine(Moving(position, onFinishedMovement));
    }

    private IEnumerator Moving(Vector3 position, Action onFinishedMovement)
    {
        while ((transform.position - position).sqrMagnitude > _minSqrDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, position, _speed * Time.deltaTime);
            yield return null;
        }
        
        onFinishedMovement?.Invoke();
    }
}
