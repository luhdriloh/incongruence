using UnityEngine;

public class EnemyWeapon : Shooter
{
    private static Transform _playerTransform;
    private static Rigidbody2D _playerRidgidBody;
    private int _shotsLeft;

    public void SetPlayerStats(GameObject player)
    {
        _playerTransform = player.transform;
        _playerRidgidBody = player.GetComponent<Rigidbody2D>();
    }

    public void FireWeaponAtPlayer(int bullets)
    {
        _shotsLeft = bullets;
        Invoke("FireWeaponAtPlayer", _fireDelay);
    }

    private void FireWeaponAtPlayer()
    {
        if (_shotsLeft <= 0)
        {
            return;
        }

        _shotsLeft--;

        // shoot directly at player
        Vector3 direction = _playerTransform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        FireWeapon(angle);
        Invoke("FireWeaponAtPlayer", _fireDelay);
    }

    public void FireLeadingShotsAtPlayer(int bullets)
    {
        _shotsLeft = bullets;
        Invoke("FireLeadingShotsAtPlayer", _fireDelay);
    }

    private void FireLeadingShotsAtPlayer()
    {
        if (_shotsLeft < 0)
        {
            return;
        }

        _shotsLeft--;

        // shoot leading shots at player
        FireLeadingShot(_playerTransform.position, _playerRidgidBody.velocity);
        Invoke("FireLeadingShotsAtPlayer", _fireDelay);
    }
}
