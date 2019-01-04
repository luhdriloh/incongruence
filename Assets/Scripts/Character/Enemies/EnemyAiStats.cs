using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAIStats.asset", menuName = "EnemyAIStats/EnemyAIStats Stats", order = 1)]
public class EnemyAIStats : ScriptableObject
{
    // bullet attack variables
    public float _attackFrequencyMin;
    public float _attackFrequencyMax;
    public int _numOfProjectiles;

    // pursuit variable
    public bool _onSitePursue;
    public float _minRangePursue;
    public float _pursueTimeWhenNotInView;
    public float _timeGapForCheckingIfPlayerInView;

    // movement variables
    public float _movementFrequencyMin;
    public float _movementFrequencyMax;

    public float _movementTimeMin;
    public float _movementTimeMax;
}
