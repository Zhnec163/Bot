using System;
using UnityEngine;

public class Resource : MonoBehaviour
{
    private Action<Resource> _delivered;

    private void OnDestroy()
    {
        _delivered = null;
    }

    public void Init(Action<Resource> onDelivered)
    {
        _delivered += onDelivered;
    }

    public void Release()
    {
        _delivered?.Invoke(this);
    }
}