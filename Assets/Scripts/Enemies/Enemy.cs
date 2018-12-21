using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject _player;
    public float _speed;

    protected enum EnemyState
    {
        PATROL = 0,
        ATTACK = 1,
        PURSUIT = 2,
    };
    protected EnemyState _state;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;

    private float _nextMovement;
    private float _timeNotMovedFor;

    private Vector2 _directionToTravel;
    private float _timeInMovement;
    private float _timeToMoveFor;

    protected void ChangeState(EnemyState state)
    {
        _state = state;
    }

    protected bool InView()
    {
        return _spriteRenderer.isVisible;
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _timeNotMovedFor = 0f;
        _nextMovement = 5f;

        _state = EnemyState.PATROL;
	}

    private void Update()
    {
        Move();

        switch (_state)
        { 
            case EnemyState.PATROL:
                Patrol();
                break;
            case EnemyState.ATTACK:
                break;
            case EnemyState.PURSUIT:
                break;
        }
    }

    private void Patrol()
    {
        _timeNotMovedFor += Time.deltaTime;

        if (_timeNotMovedFor >= _nextMovement)
        { 
            // move around in a pattern around an area
            float randomAngleToMoveTowards = Random.Range(0, 359);
            float rads = randomAngleToMoveTowards * Mathf.Deg2Rad;

            // set movement variables up for character
            _timeInMovement = 0f;
            _timeToMoveFor = Random.Range(1f, 3f);
            _directionToTravel = new Vector2(Mathf.Cos(rads), Mathf.Sin(rads)).normalized;

            // set variables up for the next movment
            _timeNotMovedFor = 0f;
            _nextMovement = _timeToMoveFor + Random.Range(3f, 8f);
        }
    }

    private void Attack()
    { 
        // if within some radius, attack at a semi random interval

        // dodging movement or some type of movement
    }

    private void Pursue()
    { 
        // move toward player
    }


    private void Move()
    {
        _timeInMovement += Time.deltaTime;

        if (_timeInMovement < _timeToMoveFor)
        {
            Vector2 movementInDirection = _directionToTravel * Time.deltaTime * _speed;
            Vector2 newPosition = (Vector2)transform.position + movementInDirection;
            _rigidbody.MovePosition(newPosition);
        }
    }
}
