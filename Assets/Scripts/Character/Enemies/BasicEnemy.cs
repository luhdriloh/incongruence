using UnityEngine;

public class BasicEnemy : Enemy
{
    protected override void Start()
    {
        base.Start();
        ChangeState(EnemyState.PATROL);
        CreateStateChange();

        float nextTimeToAttack = Random.Range(_enemyAiStats._attackFrequencyMin, _enemyAiStats._attackFrequencyMax);
        Invoke("AttackRoutine", nextTimeToAttack);
    }

    private void AttackRoutine()
    {
        Attack();
        float nextTimeToAttack = Random.Range(_enemyAiStats._attackFrequencyMin, _enemyAiStats._attackFrequencyMax);
        Invoke("AttackRoutine", nextTimeToAttack);
    }
}
