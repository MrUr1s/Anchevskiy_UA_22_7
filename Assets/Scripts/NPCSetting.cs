using System;
using UnityEngine;

[Serializable]
public class NPCSetting 
{
    [SerializeField]
    private int _maxHP;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private int _fastHit;
    [SerializeField]
    private int _strongHit;
    [SerializeField]
    private int _chanceMiss;
    [SerializeField]
    private int _chanceCrit;
    [SerializeField, Range(0, 100)]
    private int _probability;
    [SerializeField]
    private ColorTeam _team;
    [SerializeField]
    private Material _material;

    public NPCSetting()
    {

    }
    public NPCSetting(int maxHP, float speed, int fastHit, int strongHit, int chanceMiss, int chanceCrit, int probability, ColorTeam team, Material material)
    {
        _maxHP = maxHP;
        _speed = speed;
        _fastHit = fastHit;
        _strongHit = strongHit;
        _chanceMiss = chanceMiss;
        _chanceCrit = chanceCrit;
        _probability = probability;
        _team = team;
        _material = material;
    }

    public float Speed => _speed;
    public int FastHit => _fastHit;
    public int StrongHit => _strongHit;
    public float ChanceMiss => _chanceMiss;
    public float ChanceCrit => _chanceCrit;
    public int Probability => _probability;

    public int MaxHP => _maxHP;

    public ColorTeam Team => _team;
    public Material Material => _material;

    public NPCSetting Clone() => new NPCSetting(
        _maxHP,
        _speed,
        _fastHit,
        _strongHit,
        _chanceMiss,
        _chanceCrit,
        _probability,
        _team,
        _material
    );
}