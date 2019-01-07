using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats.asset", menuName = "Player/Player Stats", order = 1)]
public class PlayerStats : ScriptableObject
{
    public int _health;
    public float _movementSpeed;

    public Dictionary<ProjectileType, int> _ammo;
    public Dictionary<ProjectileType, int> _ammoMaxCarry;
}
