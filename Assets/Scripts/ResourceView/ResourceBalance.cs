using System;
using UnityEngine;

[RequireComponent(typeof(Base))]
public class ResourceBalance : MonoBehaviour
{
    public event Action Changed;

    public int Balance { get; private set; }

    public void Increment()
    {
        Balance++;
        Changed?.Invoke();
    }

    public bool HasSum(int value)
    {
        if (Balance >= value)
            return true;

        return false;
    }

    public void Substract(int value)
    {
        Balance -= value;
        Changed?.Invoke();
    }
}
