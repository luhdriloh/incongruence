using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject _player;
    public EnemyAIStats _enemyAiStats;
    public CharacterStats _enemyStats;

    protected enum EnemyState
    {
        PATROL = 0,
        PURSUIT = 1,
        IDLE = 2,
    };
    protected EnemyState _state;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    private EnemyWeapon _enemyWeapon;
    private int _health;

    protected void ChangeState(EnemyState state)
    {
        _state = state;
    }

    protected void CreateStateChange()
    { 
        switch (_state)
        {
            case EnemyState.IDLE:
                Stop();
                break;
            case EnemyState.PATROL:
                Patrol();
                break;
            case EnemyState.PURSUIT:
                Pursue();
                break;
        }
    }

    protected bool InView()
    {
        return _spriteRenderer.isVisible;
    }

    protected virtual void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _enemyWeapon = GetComponentInChildren<EnemyWeapon>();
        _enemyWeapon.SetPlayerStats(_player);

        _health = _enemyStats._health;
     }

    protected void Patrol()
    {
        if (_state != EnemyState.PATROL)
        {
            CreateStateChange();
        }

        // move around in a pattern around an area
        float randomAngleToMoveTowards = Random.Range(0, 359);
        float rads = randomAngleToMoveTowards * Mathf.Deg2Rad;

        // set movement variables up for character
        float timeToMoveFor = Random.Range(_enemyAiStats._movementTimeMin, _enemyAiStats._movementTimeMax);
        Vector2 _directionToTravel = new Vector2(Mathf.Cos(rads), Mathf.Sin(rads)).normalized;

        Move(_directionToTravel);
        Invoke("Stop", timeToMoveFor);


        // set variables up for the next movment
        float nextMovement = timeToMoveFor + Random.Range(_enemyAiStats._movementFrequencyMin, _enemyAiStats._movementFrequencyMax);
        Invoke("Patrol", nextMovement);
    }

    protected void Attack()
    {
        int shotsToFire = Random.Range(_enemyAiStats._attackProjectileMin, _enemyAiStats._attackProjectileMax);
        _enemyWeapon.FireWeaponAtPlayer(shotsToFire);
    }

    protected void Pursue()
    {
        if (_state != EnemyState.PURSUIT)
        {
            CreateStateChange();
        }

        // if the enemy sees you it pursues
        Vector3 direction = (_player.transform.position - transform.position).normalized;
        Move(direction);
    }


    protected void Move(Vector3 directionToTravel)
    {
        Vector2 movementInDirection = directionToTravel * _enemyStats._movementSpeed;
        _rigidbody.velocity = movementInDirection;
    }

    protected void Stop()
    {
        _rigidbody.velocity = Vector2.zero;
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
