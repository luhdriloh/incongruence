using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStats.asset", menuName = "Character/Character Stats", order = 1)]
public class CharacterStats : ScriptableObject
{
    public int _health;
    public float _movementSpeed;
}
