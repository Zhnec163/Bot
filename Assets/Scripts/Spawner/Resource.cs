using System;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public event Action<Resource> Delivered;

    private void OnDestroy()
    {
        Delivered = null;
    }

    public void Release()
    {
        Delivered?.Invoke(this);
    }
}