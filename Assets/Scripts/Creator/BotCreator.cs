using UnityEngine;

[RequireComponent(typeof(Base))]
public class BotCreator : Creator<Bot>
{
    public Bot Create(Vector3 origin)
    {
        return Instantiate(_prefab, origin, _prefab.transform.rotation);
    }
}