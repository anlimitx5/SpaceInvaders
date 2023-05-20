using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected Vector3 projectileDirection;
    [SerializeField] private bool _destroyOnCollision = true;
    [SerializeField] private int _damage;
    private bool isPlayerInitiator;
    private float projectileSpeed;

    private void Update()
    {
        ProjectileMove();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isPlayerInitiator && collision.TryGetComponent(out Player player) && !player.IsDead())
        {
            player.GetDamage(_damage);
            if (_destroyOnCollision)
            {
                gameObject.SetActive(false);
            }
        }
        else if (isPlayerInitiator && collision.TryGetComponent(out Enemy enemy) && !enemy.IsDead())
        {
            enemy.GetDamage(_damage);
            if (_destroyOnCollision)
            {
                gameObject.SetActive(false);
            }
        }

    }
    private void ProjectileMove()
    {
        transform.Translate(projectileDirection * projectileSpeed * Time.deltaTime);
        Vector4 gameZone = InGameManager.instance.GetGameZone();
        if (transform.position.x < gameZone.x || transform.position.x > gameZone.y
            || transform.position.y < gameZone.z || transform.position.y > gameZone.w)
        {
            gameObject.SetActive(false);
        }
    }
    public void SetInitiator(Starship starship)
    {
        isPlayerInitiator = starship == Player.instance;
    }
    public void SetDirection(Vector3 direction)
    {
        projectileDirection = direction;
    }
    public void SetProjectileSpeed(float speed)
    {
        projectileSpeed = speed;
    }
}
