using UnityEngine;

public class WeaponA : Weapon
{
    public override void Shoot()
    {
        Projectile bullet = GetProjectile();
        bullet.transform.position = User.transform.position;
        bullet.transform.rotation= Quaternion.identity;
        bullet.SetInitiator(User);
        bullet.SetDirection(transform.up);
        bullet.SetProjectileSpeed(User.GetMoveSpeed() + _relativeProjectileSpeed);
    }
}
