using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private int _initialPoolSize = 10;
    [SerializeField] private Transform _poolTransform;
    private List<Projectile> _pool = new List<Projectile>();

    private void Start()
    {
        InitializePool();
        InGameManager.instance.OnVictory += DeactivatePool;
    }
    private void InitializePool()
    {
        for (int i = 0; i < _initialPoolSize; i++)
        {
            Projectile projectile = InstantiateProjectile();
            projectile.gameObject.SetActive(false);
            _pool.Add(projectile);
        }
    }
    private Projectile InstantiateProjectile()
    {
        return Instantiate(_projectilePrefab, Vector3.zero, Quaternion.Euler(0, 0, 180), _poolTransform);
    }
    public Projectile GetProjectileFromPool()
    {
        foreach (Projectile projectile in _pool)
        {
            if (!projectile.gameObject.activeInHierarchy)
            {
                projectile.gameObject.SetActive(true);
                return projectile;
            }
        }
        Projectile newProjectile = InstantiateProjectile();
        _pool.Add(newProjectile);
        return newProjectile;
    }
    public void DeactivatePool()
    {
        foreach (Projectile projectile in _pool)
        {
            projectile.gameObject.SetActive(false);
        }
    }
    private void OnDisable()
    {
        InGameManager.instance.OnVictory -= DeactivatePool;
    }
}
