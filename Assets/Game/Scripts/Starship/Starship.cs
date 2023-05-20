using System.Collections;
using UnityEngine;

public abstract class Starship : MonoBehaviour
{
    [SerializeField] protected int maximumHealth = 5;
    [SerializeField] protected float moveSpeed = 5;
    [SerializeField] protected Weapon _weaponPrefab;
    protected int currentHealth;
    protected Coroutine shootCoroutine;
    protected Weapon currentWeapon;
    protected Animator animator;
    private float _damageSecondsCooldown = 1;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }
    protected virtual void Start()
    {
        FullHealthSet();
        SelectCurrentWeapon();
    }
    protected IEnumerator Shoot()
    {
        yield return null;
        while (true)
        {
            if (!Player.instance.IsDead() && !IsDead() && currentWeapon != null)
            {
                currentWeapon.Shoot();
            }
            yield return new WaitForSeconds(_damageSecondsCooldown);
        }
    }
    protected void ChangeAnimatorState(Vector2 direction, bool isDead)
    {
        string triggerName = "Idle";
        if (isDead)
        {
            triggerName = "Death";
        }
        else
        {
            triggerName = direction == Vector2.zero ? "Idle" : "Move";
        }
        animator.SetTrigger(triggerName);
    }
    public void ChangeWeapon(Weapon weapon)
    {
        _weaponPrefab = weapon;
        SelectCurrentWeapon();
    }
    public float GetMoveSpeed()
    {
        return moveSpeed;
    }
    public void FullHealthSet()
    {
        currentHealth = maximumHealth;
        ChangeAnimatorState(Vector2.zero, false);
    }
    public bool IsDead()
    {
        return currentHealth < 1;
    }
    public virtual void GetDamage(int damage)
    {
        if (currentHealth < 1) return;

        currentHealth = damage < currentHealth ? currentHealth - damage : 0;
        if (currentHealth < 1)
        {
            if (shootCoroutine != null)
            {
                StopCoroutine(shootCoroutine);
            }
            ChangeAnimatorState(Vector2.zero, true);
        }
    }
    private void SelectCurrentWeapon()
    {
        if (currentWeapon != null)
        {
            Destroy(currentWeapon.gameObject);
        }
        currentWeapon = Instantiate(_weaponPrefab, transform);
        _damageSecondsCooldown = currentWeapon.GetShootCooldown();
        if (shootCoroutine != null)
        {
            StopCoroutine(shootCoroutine);
            shootCoroutine = StartCoroutine(Shoot());
        }
    }
}
