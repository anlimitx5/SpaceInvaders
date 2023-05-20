using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] private List<Enemy> _enemyPrefab = new List<Enemy>();
    [SerializeField] private int _initialPoolSize = 10;
    [SerializeField] private Transform _poolTransform;
    private List<Enemy> _pool = new List<Enemy>();

    private void Start()
    {
        InitializePool();
        InGameManager.instance.OnVictory += DeactivatePool;
    }
    private void InitializePool()
    {
        for (int i = 0; i < _initialPoolSize; i++)
        {
            Enemy enemy = InstantiateEnemy();
            enemy.gameObject.SetActive(false);
            _pool.Add(enemy);
        }
    }
    private Enemy InstantiateEnemy()
    {
        int randomEnemyPrefab = Random.Range(0, _enemyPrefab.Count);
        return Instantiate(_enemyPrefab[randomEnemyPrefab],Vector3.zero,Quaternion.Euler(0,0,180), _poolTransform);
    }
    public Enemy GetEnemyFromPool()
    {
        foreach (Enemy enemy in _pool)
        {
            if (!enemy.gameObject.activeInHierarchy)
            {
                enemy.gameObject.SetActive(true);
                return enemy;
            }
        }
        Enemy newEnemy = InstantiateEnemy();
        _pool.Add(newEnemy);
        return newEnemy;
    }
    public void DeactivatePool()
    {
        foreach (Enemy enemy in _pool)
        {
            enemy.gameObject.SetActive(false);
        }
    }   
    private void OnDisable()
    {
        InGameManager.instance.OnVictory -= DeactivatePool;
    }

}
