using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Base))]
public class BaseCreator : Creator<Base>
{
    public Base Create(Vector3 origin, Bot bot, ResourceFinder resourceFinder)
    {
        Base commandCenter = Instantiate(_prefab, origin, _prefab.transform.rotation);
        commandCenter.Init(bot, resourceFinder);
        return commandCenter;
    }
}
