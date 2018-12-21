using System.Collections.Generic;

using UnityEngine;

public class Player : MonoBehaviour
{
    public List<PlayerWeapon> _shooters = new List<PlayerWeapon>(2);
    public float _speed;

    private Rigidbody2D _rigidbody;
    private int _shooterActive;

    private bool _standingOnWeapon;
    private PlayerWeapon _weaponToPickup;

    private void Start ()
    {
        _shooterActive = 0;
        _rigidbody = GetComponent<Rigidbody2D>();
        transform.position += new Vector3(2f, 2f, 0f);
	}
	
	private void Update () 
    {
        PickupWeapon();
        SwitchWeapon();
        Move();
	}

    private void Move()
    {
        // get axis for sniper dude and move him about
        Vector2 movementInDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * Time.deltaTime * _speed;
        Vector2 newPosition = (Vector2)transform.position + movementInDirection;
        _rigidbody.MovePosition(newPosition);
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


    private void PickupWeapon()
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
    private void SwitchWeapon()
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
}
