using System.Collections;
using UnityEngine;

public class Enemy : Starship
{
    [SerializeField] private bool _starShootOnStart = false;
    [SerializeField] private Vector2 _delayMinMax = new Vector2(1, 3);
    [SerializeField] private CollectableWeapon _dropOnDie;
    [SerializeField] private int _dropChance = 10;
    [SerializeField] private int _giveScore = 1;
    private Coroutine _startShootingCoroutine;
    private float _delayForStartShoot;

    protected override void Start()
    {
        base.Start();
    }
    private void OnEnable()
    {
        if (!_starShootOnStart)
        {
            _delayForStartShoot = Random.Range(_delayMinMax.x, _delayMinMax.y);
        }
        else
        {
            _delayForStartShoot = 0;
        }
        _startShootingCoroutine = StartCoroutine(StartShooting());
    }
    private void OnDisable()
    {
        if (shootCoroutine != null)
        {
            StopCoroutine(shootCoroutine);
            shootCoroutine = null;
        }

        if (_startShootingCoroutine != null)
        {
            StopCoroutine(_startShootingCoroutine);
            _startShootingCoroutine = null;
        }
    }
    private IEnumerator StartShooting()
    {
        yield return new WaitForSeconds(_delayForStartShoot);
        if (shootCoroutine == null)
        {
            shootCoroutine = StartCoroutine(Shoot());
        }
    }
    public void DeactivateEnemy()
    {
        gameObject.SetActive(false);
    }
    public override void GetDamage(int damage)
    {
        base.GetDamage(damage);
        if (currentHealth < 1)
        {
            InGameManager.instance.ChangeScore(_giveScore);
            InGameManager.instance.EnemyKillCounter();
            CalculateDrop();
        }
    }
    private void CalculateDrop()
    {
        int roll = Random.Range(0, 100);
        if (roll < _dropChance)
        {
            Instantiate(_dropOnDie, transform.position, Quaternion.identity, InGameManager.instance.dropTransform);
        }
    }
}
