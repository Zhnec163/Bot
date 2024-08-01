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
    
    public void MoveTo(Vector3 position, Action onMoveEnd)
    {
        transform.LookAt(position);
        StartCoroutine(Moving(position, onMoveEnd));
    }

    private IEnumerator Moving(Vector3 position, Action onMoveEnd)
    {
        while ((transform.position - position).sqrMagnitude > _minSqrDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, position, _speed * Time.deltaTime);
            yield return null;
        }
        
        onMoveEnd.Invoke();
    }
}
