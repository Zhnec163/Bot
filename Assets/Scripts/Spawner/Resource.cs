using System;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public event Action<Resource> Delivered; // отписку незабыть

    public void Release()
    {
        Delivered?.Invoke(this);
    }
}