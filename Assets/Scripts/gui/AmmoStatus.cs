using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoStatus : MonoBehaviour
{
    public RectTransform _bulletAmmo;
    public RectTransform _shellAmmo;
    public RectTransform _sniperAmmo;

    private float ammoBakgroundHeight;
    private float _maskHeight;
    private float _startY;

	private void Start ()
    {
        GameObject player = GameObject.FindWithTag("Player");

        _maskHeight = _bulletAmmo.rect.height;
        _startY = _bulletAmmo.transform.position.y;

        // update ammo carry stats
        Player playerScript = player.GetComponent<Player>();
        playerScript.AddAmmoChangeSubscriber(UpdateAmmoCount);
        UpdateAmmoCount(playerScript._playerStats._ammo, playerScript._playerStats._ammoMaxCarry);
    }

    public void UpdateAmmoCount (Dictionary<ProjectileType, int> ammoCount, Dictionary<ProjectileType, int> maxAmmoCount)
    {
        foreach (ProjectileType type in maxAmmoCount.Keys)
        {
            switch (type)
            { 
                case ProjectileType.BULLET:
                    UpdateAmmoPercent((float)ammoCount[ProjectileType.BULLET] / maxAmmoCount[ProjectileType.BULLET], _bulletAmmo);
                    break;
                case ProjectileType.SHELL:
                    UpdateAmmoPercent((float)ammoCount[ProjectileType.SHELL] / maxAmmoCount[ProjectileType.SHELL], _shellAmmo);
                    break;
                case ProjectileType.SNIPER:
                    UpdateAmmoPercent((float)ammoCount[ProjectileType.SNIPER] / maxAmmoCount[ProjectileType.SNIPER], _sniperAmmo);
                    break;
            }
        }
	}

    private void UpdateAmmoPercent(float percent, RectTransform toTransform)
    {
        toTransform.transform.position = new Vector3(toTransform.transform.position.x, _startY + (percent * _maskHeight), toTransform.transform.position.z);
    }
}
