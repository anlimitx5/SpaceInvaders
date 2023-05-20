using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Enemy _enemy;
    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
    }
    private void Update()
    {
        if (_enemy == null) return;
        StandartMove();
        DeactiveLogic();
    }
    private void DeactiveLogic()
    {
        if (transform.position.y < InGameManager.instance.GetGameZoneYForEnemies().x)
        {
            gameObject.SetActive(false);
        }
    }
    private void StandartMove()
    {
        if (_enemy.IsDead()) return;
        transform.Translate(Vector2.up * _enemy.GetMoveSpeed() * Time.deltaTime);
    }
}
