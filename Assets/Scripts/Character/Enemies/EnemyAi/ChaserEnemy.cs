using UnityEngine;

public class ChaserEnemy : Enemy
{
    protected override void Start()
    {
        base.Start();
        Patrol();

        // only attack if the ai spec says to attack
        Invoke("PursueRoutine", _enemyAiStats._timeGapForCheckingIfPlayerInView);
    }

    private void PursueRoutine()
    {
        if (PlayerInView() && _state != EnemyState.PURSUIT)
        {
            ChangeState(EnemyState.PURSUIT);
        }
        else if (_state != EnemyState.PATROL)
        {
            ChangeState(EnemyState.PATROL);
        }

        Invoke("PursueRoutine", _enemyAiStats._timeGapForCheckingIfPlayerInView);
    }
}
