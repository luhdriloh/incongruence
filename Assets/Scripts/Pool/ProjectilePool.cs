using System.Collections.Generic;
using UnityEngine;

using ActionGameFramework.Projectiles;

public delegate void ReturnToPool(LinearProjectile projectile);

public enum ProjectileType
{
    UNKNOWN = 0,
    BULLET,
    SHELL,
    SNIPER,
    ENEMY
};

public class ProjectilePool : MonoBehaviour
{
    public static Dictionary<ProjectileType, ProjectilePool> _projectilePool = new Dictionary<ProjectileType, ProjectilePool>();

    public GameObject _weaponProjectile;
    public ProjectileType _projectileType;
    private Stack<LinearProjectile> _projectilesNotInUse;

    private void Awake()
    {
        if (!_projectilePool.ContainsKey(_projectileType))
        {
            _projectilePool.Add(_projectileType, this);
        }

        _projectilesNotInUse = new Stack<LinearProjectile>();
        AddProjectilesToPool(30);
    }

    public void HandleReturnToPool(LinearProjectile projectile)
    {
        projectile.gameObject.SetActive(false);
        _projectilesNotInUse.Push(projectile);
    }


    public LinearProjectile GetObjectFromPool()
    {
        if (_projectilesNotInUse.Count == 0)
        {
            AddProjectilesToPool(3);
        }

        LinearProjectile projectileToReturn = _projectilesNotInUse.Pop();
        projectileToReturn.gameObject.SetActive(true);
        return projectileToReturn;
    }

    private void AddProjectilesToPool(int amountToAdd)
    {
        ReturnToPool delegateToUse = HandleReturnToPool;

        for (int i = 0; i < amountToAdd; i++)
        {
            GameObject newGameObject = Instantiate(_weaponProjectile, transform.position, Quaternion.identity);
            LinearProjectile projectile = newGameObject.GetComponent<LinearProjectile>();
            projectile._returnToPool = delegateToUse;
            _projectilesNotInUse.Push(projectile);

            // turn projectile off
            newGameObject.SetActive(false);
        }
    }
}
