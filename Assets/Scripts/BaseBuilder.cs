using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BaseBuilder : MonoBehaviour
{
    [SerializeField] private Base _prefab;
    [SerializeField] private ResourceRepository _resourceRepository;

    public Base Build(Vector3 position, Bot bot)
    {
        Base commandCenter = Instantiate(_prefab, position, _prefab.transform.rotation);
        commandCenter.Init(bot, _resourceRepository);
        return commandCenter;
    }
}