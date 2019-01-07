using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShooterState.asset", menuName = "Shooter/Shooter Stats", order = 1)]
public class ShooterStats : ScriptableObject
{
    public string _description;

    public float _recoilInDegrees;

    public int _rpmMax;

    public int _rpmTapMax;

    public int _projectilesPerShot;

    public ProjectileType _projectileType;
}