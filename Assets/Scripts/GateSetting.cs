using System;
using UnityEngine;

[Serializable]
public class GateSetting
{
    [SerializeField]
    private float _spawnDelay;
    [SerializeField]
    private int _spawnCount;

    [SerializeField]
    private NPCSetting _nPCSetting;

    public GateSetting(float spawnDelay, int spawnCount, NPCSetting nPCSetting)
    {
        _spawnDelay = spawnDelay;
        _spawnCount = spawnCount;
        _nPCSetting = nPCSetting;
    }
    
    public void SetNPCsetting(NPCSetting nPCSetting)
    {
        _nPCSetting= nPCSetting.Clone();
    }
    public float SpawnDelay => _spawnDelay;
    public int SpawnCount => _spawnCount;

    public NPCSetting NPCSetting => _nPCSetting;

    public GateSetting Clone()
    {
        return new GateSetting( SpawnDelay, SpawnCount,(NPCSetting)NPCSetting.Clone());
    }
}