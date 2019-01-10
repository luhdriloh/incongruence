using System;
using System.Collections.Generic;

using UnityEngine;

public delegate void HealthChangeHandler(int newHealthValue, int maxHealthValue);
public delegate void AmmoChangeHandler(Dictionary<ProjectileType, int> ammoCount, Dictionary<ProjectileType, int> maxCarry);

public class Player : MonoBehaviour
{
    public event HealthChangeHandler _healthChangedEvent;
    public event AmmoChangeHandler _ammoChangedEvent;

    public PlayerStats _playerStats;
    public List<PlayerWeapon> _shooters = new List<PlayerWeapon>(2);

    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private int _shooterActive;

    private bool _standingOnWeapon;
    private bool _walking;
    private PlayerWeapon _weaponToPickup;

    private void Awake()
    {
        // set up player ammo
        _playerStats._ammo = new Dictionary<ProjectileType, int>();
        foreach (ProjectileType type in Enum.GetValues(typeof(ProjectileType)))
        {
            _playerStats._ammo.Add(type, 0);
        }

        _playerStats._ammo[ProjectileType.BULLET] = 56;
        _playerStats._ammo[ProjectileType.SHELL] = 32;
        _playerStats._ammo[ProjectileType.SNIPER] = 12;
        _playerStats._ammoMaxCarry = new Dictionary<ProjectileType, int>
        {
            { ProjectileType.BULLET, 256 },
            { ProjectileType.SHELL, 56 },
            { ProjectileType.SNIPER, 36 },
        };

        // you get a pistol as default
        _shooterActive = 0;
        _shooters[0] = GetComponentInChildren<PlayerWeapon>();
        _shooters[_shooterActive].SetAsActive();

        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerStats._health = _playerStats._maxHealth;
        _walking = false;
    }
	
	private void Update () 
    {
        PickupItem();
        SwitchToNextItem();
        Move();
	}

    private void Move()
    {
        float xMovement = Input.GetAxisRaw("Horizontal");

        if (Mathf.Abs(xMovement) <= Mathf.Epsilon && _walking == true)
        {
            _spriteRenderer.flipX = false;
            _animator.SetBool("walking", false);
            _walking = false;
        }
        else if (Mathf.Abs(xMovement) > Mathf.Epsilon && _walking == false)
        {
            if (xMovement < 0)
            {
                _spriteRenderer.flipX = true;
            }

            _animator.SetBool("walking", true);
            _walking = true;
        }

        Vector2 movementInDirection = new Vector2(xMovement, Input.GetAxisRaw("Vertical")).normalized * _playerStats._movementSpeed;
        _rigidbody.velocity = movementInDirection;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("transporter"))
        {
            // get the transporter coordinates
            Transporter transporter = collision.gameObject.GetComponent<Transporter>();
            transform.position = transporter.GetTransporterCoordinates();
        }
    }


    private void PickupItem()
    {
        if (Input.GetKeyDown(KeyCode.E) && _standingOnWeapon)
        {
            int emptyWeaponSlot = -1;
            int numWeapons = 0;

            for (int i = _shooters.Count - 1; i >= 0; i--)
            {
                if (_shooters[i] != null)
                {
                    numWeapons++;
                }

                if (_shooters[i] == null)
                {
                    emptyWeaponSlot = i;
                }
                else if (_weaponToPickup == _shooters[i])
                {
                    return;
                }
            }

            // cases where we have empty slots or we have full slots
            if (emptyWeaponSlot >= 0)
            {
                // set the weapon into the empty slot
                _shooters[emptyWeaponSlot] = _weaponToPickup;
                _weaponToPickup.Pickup(transform);

                if (numWeapons > 0)
                {
                    _weaponToPickup.SetAsInactive();
                }
            }
            else
            {
                // drop current weapon on ground and pickup weapon
                _shooters[_shooterActive].DropOnGround();
                _shooters[_shooterActive] = _weaponToPickup;
                _shooters[_shooterActive].Pickup(transform);
            }
        }
    }

    // change to agnostic 'item' instead of weapon
    private void SwitchToNextItem()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _shooters[_shooterActive] != null)
        {
            int nextNonEmptySlot = _shooterActive;

            // find the next non empty slot
            do
            {
                nextNonEmptySlot = (nextNonEmptySlot + 1) % _shooters.Count;
            }
            while (_shooters[nextNonEmptySlot] == null);

            // if we only have 1 weapon no need to switch, return
            if (nextNonEmptySlot == _shooterActive)
            {
                return;
            }
            else
            {
                _shooters[_shooterActive].SetAsInactive();

                _shooterActive = nextNonEmptySlot;
                _shooters[_shooterActive].SetAsActive();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _standingOnWeapon = true;
        _weaponToPickup = collision.GetComponent<PlayerWeapon>();
    }

    private void OnTriggerExit2D()
    {
        _standingOnWeapon = false;
        _weaponToPickup = null;
    }

    public void AddHealthChangeSubscriber(HealthChangeHandler healthChangeHandler)
    {
        _healthChangedEvent += healthChangeHandler;
    }

    public void AddAmmoChangeSubscriber(AmmoChangeHandler ammoChangeHandler)
    {
        _ammoChangedEvent += ammoChangeHandler;
    }

    public void HealthChange(int healthAmount)
    {
        // change health based on if health is added or subtracted
        _playerStats._health = healthAmount < 0
            ? Mathf.Max(_playerStats._health + healthAmount, 0)
            : Mathf.Min(_playerStats._health + healthAmount, _playerStats._maxHealth);

        OnHealthChange();

        if (_playerStats._health <= 0)
        {
            // death animation
            GameController._gameController.GameOver();
            Destroy(this);
        }
    }

    public int GetAmmoAmount(ProjectileType ammoType)
    {
        return _playerStats._ammo[ammoType];
    }

    public void AmmoChange(ProjectileType ammoType, int changeAmount)
    {
        int newAmmoAmount = _playerStats._ammo[ammoType] + changeAmount;
        _playerStats._ammo[ammoType] = changeAmount < 0 ?
            Mathf.Max(newAmmoAmount, 0) :
            Mathf.Min(newAmmoAmount, _playerStats._ammoMaxCarry[ammoType]);

        OnAmmoChange();
    }

    protected virtual void OnHealthChange()
    {
        if (_healthChangedEvent != null)
        {
            _healthChangedEvent(_playerStats._health, _playerStats._maxHealth);
        }
    }

    protected virtual void OnAmmoChange()
    {
        if (_ammoChangedEvent != null)
        {
            _ammoChangedEvent(_playerStats._ammo, _playerStats._ammoMaxCarry);
        }
    }
}
