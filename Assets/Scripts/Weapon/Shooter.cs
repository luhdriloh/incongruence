using System.Collections.Generic;
using UnityEngine;

using ActionGameFramework.Projectiles;

public class Shooter : MonoBehaviour
{
    public GameObject _firepointGameObject;
    public ShooterStats _shooterStats;
    private ProjectilePool _projectiles;
    private Camera _camera;

    private float _fireDelay;
    private float _tapFireDelay;
    private float _currentTimeBetweenShotFired;
    private Transform _firepoint;

	private void Start ()
    {
        _fireDelay = 60f / _shooterStats._rpmMax;
        _tapFireDelay = 60f / _shooterStats._rpmTapMax;

        _currentTimeBetweenShotFired = 100f;

        _projectiles = ProjectilePool._projectilePool[_shooterStats._projectileType];
        _camera = Camera.main;
        _firepoint = _firepointGameObject.transform;
    }

    private void Update ()
    {
        // get mouse position then rotate sniper dude
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

        if (Input.GetMouseButtonDown(0) && _currentTimeBetweenShotFired >= _tapFireDelay)
        {
            FireWeapon(target, angle);
        }

        if (Input.GetMouseButton(0) && _currentTimeBetweenShotFired >= _fireDelay)
        {
            FireWeapon(target, angle);
        }
    }

    private void FireWeapon (Vector3 target, float angle)
    {
        LinearProjectile projectile = _projectiles.GetObjectFromPool();
        float bulletAngleOfTravel = Random.Range(-_shooterStats._recoilInDegrees, _shooterStats._recoilInDegrees) + angle;

        projectile.FireInDirection(_firepoint.position, BulletTravelVector(bulletAngleOfTravel), bulletAngleOfTravel);
        _currentTimeBetweenShotFired = 0f;
    }

    private Vector3 BulletTravelVector(float bulletAngleOfTravel)
    {
        return new Vector3(Mathf.Cos(Mathf.Deg2Rad * bulletAngleOfTravel), Mathf.Sin(Mathf.Deg2Rad * bulletAngleOfTravel), 0);
    }
}
