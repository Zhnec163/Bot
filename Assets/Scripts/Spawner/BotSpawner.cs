﻿using UnityEngine;

public class BotSpawner : MonoBehaviour
{
    [SerializeField] private Bot _prefab;
    
    public Bot Spawn(Vector3 position) => Instantiate(_prefab, position, _prefab.transform.rotation);
}