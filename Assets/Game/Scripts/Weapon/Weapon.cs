using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField][Range(0,2)] protected int _bulletNumber =0;
    [SerializeField] protected float _relativeProjectileSpeed = 2;
    [SerializeField] protected float _shootCooldown = 2;

    private void Awake()
    {
        User = GetComponentInParent<Starship>();
    }
    protected Starship User;
    public float GetShootCooldown()
    {
        return _shootCooldown;
    }
    public abstract void Shoot();
    protected Projectile GetProjectile()
    {
        return InGameManager.instance.projectilePool[_bulletNumber].GetProjectileFromPool();
    }
}
