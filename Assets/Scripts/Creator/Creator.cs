using UnityEngine;

public class Creator<T> : MonoBehaviour
{
    [SerializeField] protected T _prefab;
    [SerializeField] protected int _cost;
    
    public int Cost => _cost;
}