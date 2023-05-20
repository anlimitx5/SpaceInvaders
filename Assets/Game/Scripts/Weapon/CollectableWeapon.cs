using UnityEngine;

public class CollectableWeapon : MonoBehaviour
{
    [SerializeField] private Weapon _weapon;
    private float _destroyOverSeconds = 15;

    private void Start()
    {
        Invoke(nameof(DestoyObject), _destroyOverSeconds);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            player.ChangeWeapon(_weapon);
            DestoyObject();
        }
    }
    private void DestoyObject()
    {
        Destroy(gameObject);
    }
}

