using UnityEngine;

public enum EnemyState
{
    PATROL = 0,
    PURSUIT = 1,
    IDLE = 2,
};

// this class defines basic behavior upon we can call using an patent class
public class Enemy : MonoBehaviour
{
    public static GameObject _player;
    public EnemyAIStats _enemyAiStats;
    public CharacterStats _enemyStats;
    protected EnemyState _state;

    protected SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody;
    private EnemyWeapon _enemyWeapon;
    private int _health;

    private float _nextMovement;
    private float _timeSinceLastMovement;

    protected void ChangeState(EnemyState state)
    {
        _state = state;
    }

    protected bool InView()
    {
        return _spriteRenderer.isVisible;
    }

    protected bool PlayerInView()
    {
        // get direction toward player
        Vector3 direction = (_player.transform.position - transform.position);
        RaycastHit2D[] hits = new RaycastHit2D[2];

        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(LayerMask.GetMask("mapedge", "player"));

        int collidersHit = Physics2D.Raycast(transform.position, direction, filter, hits, _enemyAiStats._minRangePursue);
        return collidersHit > 0 && hits[0].collider.gameObject.layer == LayerMask.NameToLayer("player");
    }

    protected virtual void Start()
    {
        if (_player == null)
        {
            _player = GameObject.FindWithTag("Player");
        }

        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _enemyWeapon = GetComponentInChildren<EnemyWeapon>();
        _enemyWeapon.SetPlayerStats(_player);

        _health = _enemyStats._health;
        _nextMovement = 0f;
        _timeSinceLastMovement = 0f;
     }

    protected void Update()
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

    protected void Patrol()
    {
        _timeSinceLastMovement += Time.deltaTime;

        if (_timeSinceLastMovement > _nextMovement)
        {
            // move around in a pattern around an area
            float randomAngleToMoveTowards = Random.Range(0, 359);
            float rads = randomAngleToMoveTowards * Mathf.Deg2Rad;

            // set movement variables up for character
            float timeToMoveFor = Random.Range(_enemyAiStats._movementTimeMin, _enemyAiStats._movementTimeMax);
            Vector2 _directionToTravel = new Vector2(Mathf.Cos(rads), Mathf.Sin(rads)).normalized;

            Move(_directionToTravel);
            Invoke("Stop", timeToMoveFor);

            // set variables up for the next movment
            _nextMovement = timeToMoveFor + Random.Range(_enemyAiStats._movementFrequencyMin, _enemyAiStats._movementFrequencyMax);
            _timeSinceLastMovement = 0f;
        }
    }

    protected void Attack()
    {
        _enemyWeapon.FireWeaponAtPlayer(_enemyAiStats._numOfProjectiles);
    }

    protected void Pursue()
    {
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

    public void TakeDamage(int damage, float bulletAngleAroundZAxis)
    {
        _health += damage;
        BloodSplatterEffect._bloodSplatterEffect.SpawnBloodSplatter(transform.position, bulletAngleAroundZAxis);

        if (_health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
