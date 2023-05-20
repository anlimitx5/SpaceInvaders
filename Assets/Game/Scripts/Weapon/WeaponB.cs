using UnityEngine;

public class WeaponB : Weapon
{
    private int _bulletsNumber = 12;
    public override void Shoot()
    {
        Vector3 initialDirection = User.transform.up;
        float angleIncrement = 360f / _bulletsNumber;

        for (int i = 0; i < _bulletsNumber; i++)
        {
            float angle = i * angleIncrement;
            Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
            Vector3 direction = rotation * initialDirection;

            Projectile bullet = GetProjectile();
            bullet.transform.position = transform.position;

            bullet.SetInitiator(User);
            bullet.SetDirection(direction);
            bullet.SetProjectileSpeed(User.GetMoveSpeed() + _relativeProjectileSpeed);
        }
    }
}
