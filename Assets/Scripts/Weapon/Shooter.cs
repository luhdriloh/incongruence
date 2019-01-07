using UnityEngine;

using ActionGameFramework.Projectiles;
using ActionGameFramework.Helpers;

public class Shooter : MonoBehaviour
{
    public GameObject _firepointGameObject;
    public ShooterStats _shooterStats;
    public AudioClip _fireSfx;

    protected Camera _camera;
    protected float _fireDelay;
    protected float _tapFireDelay;
    protected float _currentTimeBetweenShotFired;

    private ProjectilePool _projectiles;
    private SoundEffectPlayer _soundEffectPlayer;
    private Transform _firepoint;
    private MuzzleFlash _muzzleFlashComponent;

    protected virtual void Start ()
    {
        _fireDelay = 60f / _shooterStats._rpmMax;
        _tapFireDelay = 60f / _shooterStats._rpmTapMax;

        _currentTimeBetweenShotFired = 100f;

        _soundEffectPlayer = SoundEffectPlayer._sfxPlayer;
        _projectiles = ProjectilePool._projectilePool[_shooterStats._projectileType];
        _camera = Camera.main;
        _firepoint = _firepointGameObject.transform;
        _muzzleFlashComponent = _firepointGameObject.GetComponent<MuzzleFlash>();
    }

    protected void FireWeapon (float angle)
    {
        _soundEffectPlayer.PlaySoundEffect(_fireSfx);
        _muzzleFlashComponent.Fire();

        for (int i = 0; i < _shooterStats._projectilesPerShot; i++)
        { 
            LinearProjectile projectile = _projectiles.GetObjectFromPool();
            float bulletAngleOfTravel = Random.Range(-_shooterStats._recoilInDegrees, _shooterStats._recoilInDegrees) + angle;

            projectile.FireInDirection(_firepoint.position, BulletTravelVector(bulletAngleOfTravel), bulletAngleOfTravel);
        }
        
        _currentTimeBetweenShotFired = 0f;
    }

    protected void FireLeadingShot(Vector2 target, Vector2 velocity)
    {
        _soundEffectPlayer.PlaySoundEffect(_fireSfx);
        for (int i = 0; i < _shooterStats._projectilesPerShot; i++)
        {
            LinearProjectile projectile = _projectiles.GetObjectFromPool();
            Vector3 targetLeadFireVector = Ballistics.CalculateLinearLeadingTargetPoint(transform.position, target, velocity, projectile._startSpeed, projectile._acceleration);

            projectile.FireAtPoint(_firepoint.position, targetLeadFireVector);
        }

        _currentTimeBetweenShotFired = 0f;
    }

    private Vector3 BulletTravelVector(float bulletAngleOfTravel)
    {
        return new Vector3(Mathf.Cos(Mathf.Deg2Rad * bulletAngleOfTravel), Mathf.Sin(Mathf.Deg2Rad * bulletAngleOfTravel), 0);
    }
}
