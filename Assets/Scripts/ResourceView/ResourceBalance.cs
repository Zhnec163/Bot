using System;
using UnityEngine;

public class ResourceBalance : MonoBehaviour
{
    public event Action Change;

    public int Balance { get; private set; }

    public void Increment()
    {
        Balance++;
        Change?.Invoke();
    }
}
