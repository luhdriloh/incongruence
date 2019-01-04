using UnityEngine;

public class BasicEnemy : Enemy
{
    protected override void Start()
    {
        base.Start();
        Patrol();

        // only attack if the ai spec says to attack
        float nextTimeToAttack = Random.Range(_enemyAiStats._attackFrequencyMin, _enemyAiStats._attackFrequencyMax);
        Invoke("AttackRoutine", nextTimeToAttack);
    }


    private void AttackRoutine()
    {
        if (PlayerInView())
        {
            Attack();
        }

        float nextTimeToAttack = Random.Range(_enemyAiStats._attackFrequencyMin, _enemyAiStats._attackFrequencyMax);
        Invoke("AttackRoutine", nextTimeToAttack);
    }
}
