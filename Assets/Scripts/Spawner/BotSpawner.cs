using UnityEngine;

[RequireComponent(typeof(Base))]
public class BotSpawner : MonoBehaviour
{
    [SerializeField] protected Bot _prefab;
    
    public Bot Spawn(Vector3 position) => Instantiate(_prefab, position, _prefab.transform.rotation);
}