using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAIStats.asset", menuName = "EnemyAIStats/EnemyAIStats Stats", order = 1)]
public class EnemyAIStats : ScriptableObject
{
    // attack variables
    public float _attackFrequencyMin;
    public float _attackFrequencyMax;
    public int _attackProjectileMin;
    public int _attackProjectileMax;

    // movement variables
    public bool _constantMovement;
    public float _movementFrequencyMin;
    public float _movementFrequencyMax;

    public float _movementTimeMin;
    public float _movementTimeMax;
}
