using System;
using UnityEngine;

public class ResourceBalance : MonoBehaviour
{
    public event Action Changed;

    public int Balance { get; private set; }

    public void Increment()
    {
        Balance++;
        Changed?.Invoke();
    }

    public bool TrySubstract(int value)
    {
        if (Balance >= value)
        {
            Balance -= value;
            Changed?.Invoke();
            return true;
        }

        return false;
    }
}
