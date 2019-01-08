using UnityEngine;

public class PlayerWeapon : Shooter
{
    private BoxCollider2D _boxCollider;
    private bool _inUse;

    private static Player _player;
    private float _timeForRecoil;

    protected override void Start()
    {
        base.Start();

        _boxCollider = GetComponent<BoxCollider2D>();

        if (_player == null)
        {
            _player = GameObject.FindWithTag("Player").GetComponent<Player>();
        }

        _timeForRecoil = Mathf.Max(_tapFireDelay, _fireDelay) / 2;
    }

    public void SetAsActive()
    {
        SetActiveStatus(true);
    }

    public void SetAsInactive()
    {
        SetActiveStatus(false);
    }

    private void SetActiveStatus(bool status)
    {
        gameObject.SetActive(status);
        _inUse = status;
    }

    public void DropOnGround()
    {
        // set in a random rotation
        _inUse = false;
        _boxCollider.gameObject.SetActive(true);
        transform.localPosition = new Vector3(0f, 0f, 0f);
        transform.parent = null;
        gameObject.SetActive(true);
    }

    public void Pickup(Transform playerTransform)
    {
        _inUse = true;
        _boxCollider.gameObject.SetActive(false);
        transform.SetParent(playerTransform);
        transform.localPosition = new Vector3(0f, 0f, -1);
        SetAsActive();
    }


    private void Update ()
    {
        if (!_inUse)
        {
            return;
        }

        Vector3 target = _camera.ScreenToWorldPoint(Input.mousePosition);

        // get mouse position then rotate sniper dude
        Vector3 direction = target - transform.position;

        // get player position, move and rotate towards that position
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = rotation;

        // switch orientation of weapon depending on where you are pointing
        if (Mathf.Abs(angle) >= 90)
        {
            transform.eulerAngles = new Vector3(0f, 180f, -transform.eulerAngles.z + 180f);
        }
        else
        {
            transform.eulerAngles = new Vector3(0f, 0f, transform.eulerAngles.z);
        }

        _currentTimeBetweenShotFired += Time.deltaTime;

        if (_player.GetAmmoAmount(_shooterStats._projectileType) <= 0)
        {
            return;
        }

        // shoot if we are tapping fast enough or auto fire
        if (Input.GetMouseButtonDown(0) && _currentTimeBetweenShotFired >= _tapFireDelay)
        {
            //CinemachineCameraFunctions._cameraFunctions.StartCameraShake(_timeForRecoil);
            FireWeapon(angle);
            _player.AmmoChange(_shooterStats._projectileType, -1);
        }
        else if (Input.GetMouseButton(0) && _currentTimeBetweenShotFired >= _fireDelay)
        {
            //CinemachineCameraFunctions._cameraFunctions.StartCameraShake(_timeForRecoil);
            FireWeapon(angle);
            _player.AmmoChange(_shooterStats._projectileType, -1);
        }
    }
}
