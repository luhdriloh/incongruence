using System.Linq;
using System.Collections.Generic;

using UnityEngine;

public class AmmoPack : MonoBehaviour
{
    private static List<ProjectileType> _weaponProjectileTypes = new List<ProjectileType>
    {
        ProjectileType.BULLET,
        ProjectileType.SHELL,
        ProjectileType.SNIPER
    };

    private static Dictionary<ProjectileType, int> _typeToAmmoAmount = new Dictionary<ProjectileType, int>
    {
        { ProjectileType.BULLET, 32 },
        { ProjectileType.SHELL, 12 },
        { ProjectileType.SNIPER, 8 },
    };


    private void OnTriggerEnter2D(Collider2D collision)
    {
        ProjectileType toAdd = ProjectileType.BULLET;

        // get the types of weapons the player has
        Player player = collision.GetComponent<Player>();
        Dictionary<ProjectileType, int> ammoAmount = player._playerStats._ammo;
        Dictionary<ProjectileType, int> maxAmmoCarry = player._playerStats._ammoMaxCarry;
        List<ProjectileType> weaponTypes = player.GetPlayerWeaponTypes();
        List<ProjectileType> leftOver = _weaponProjectileTypes.Except(weaponTypes).ToList();

        // chance to spawn ammo of the types of weapons you have
        float spawnValue = Random.value;
        if (spawnValue < .40 && weaponTypes[0] != ProjectileType.UNKNOWN && ammoAmount[weaponTypes[0]] < maxAmmoCarry[weaponTypes[0]])
        {
            toAdd = weaponTypes[0];
        }
        else if (spawnValue < .80 && weaponTypes[1] != ProjectileType.UNKNOWN && ammoAmount[weaponTypes[1]] < maxAmmoCarry[weaponTypes[1]])
        {
            toAdd = weaponTypes[1];
        }
        else
        {
            foreach (ProjectileType type in leftOver)
            {
                if (ammoAmount[type] < maxAmmoCarry[type])
                {
                    toAdd = type;
                    break;
                }
            }
        }

        player.AmmoChange(toAdd, _typeToAmmoAmount[toAdd]);
        Destroy(gameObject);
    }
}
