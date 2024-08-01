using System;
using UnityEngine;

public class ResourceBalance : MonoBehaviour
{
    public int Balance { get; private set; }
    
    public event Action OnChange;

    public void Increment()
    {
        Balance++;
        OnChange?.Invoke();
    }
}
