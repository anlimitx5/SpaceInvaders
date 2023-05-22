using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    public static InGameManager instance;
    public event Action OnVictory;
    public EnemyPool enemyPool;
    public ProjectilePool[] projectilePool;
    public Transform dropTransform;
    [SerializeField] private Vector2 _gameZoneOffset = new Vector2();
    [SerializeField] private ParticleSystem _stars;
    [SerializeField] private float _spawnEnemiesEverySeconds = 2;
    private Vector2 _gameZoneX = new Vector2();
    private Vector2 _gameZoneY = new Vector2();
    private Vector2 _playerStartPosition;
    private List<Vector2> _enemysStartPositions = new List<Vector2>();
    private int _enemyCounter;

    private void Awake()
    {
        Application.targetFrameRate = 120;
        Singleton();
        SetupGameZone();
        SetupEnemyStartPositions();
        _playerStartPosition = new Vector2(0, _gameZoneY.x + 3);
    }
    private void Start()
    {
        Invoke(nameof(LevelStart), 3);
    }
    void LevelStart()
    {
        StartCoroutine(SpawnEnemies());
        _enemyCounter = LevelManager.instance.levels[GameVariables.currentLevel].targetEnemiesCount;
        Player.instance.StartShoot();
    }
    private IEnumerator SpawnEnemies()
    {
        while (true && !Player.instance.IsDead())
        {
            int enemiesCount = UnityEngine.Random.Range(0, _enemysStartPositions.Count);
            List<Vector2> freePositions = new List<Vector2>(_enemysStartPositions);
            for (int i = 0; i < enemiesCount; i++)
            {
                int randomFreePosition = UnityEngine.Random.Range(0, freePositions.Count);
                Vector2 startPosition = freePositions[randomFreePosition];
                freePositions.RemoveAt(randomFreePosition);
                Enemy enemy = enemyPool.GetEnemyFromPool();
                enemy.FullHealthSet();
                enemy.transform.position = startPosition;
            }
            yield return new WaitForSeconds(_spawnEnemiesEverySeconds);
        }
    }
    private void Singleton()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance == this)
        {
            Destroy(gameObject);
        }
    }
    public void EnemyKillCounter()
    {
        _enemyCounter -= 1;
        InGameUI.instance.ChangeEnemiesCounter(_enemyCounter);
        
        if (_enemyCounter <1)
        {
            Victory();
        }
    }
    private void Victory()
    {
        OnVictory?.Invoke();
        StopAllCoroutines();
        InGameUI.instance.ShowVictory();
    }
    private void SetupEnemyStartPositions()
    {
        int startPositionsNumber = Mathf.FloorToInt(_gameZoneX.y);
        int startX = -(startPositionsNumber - 1);
        float startY = _gameZoneY.y + 2;
        _enemysStartPositions.Clear();
        for (int i = startX; i <= -startX; i += 2)
        {
            Vector2 pos = new Vector2(i, startY);
            _enemysStartPositions.Add(pos);
        }
    }
    private void SetupGameZone()
    {
        float cameraHeight = 2f * Camera.main.orthographicSize;
        float cameraWidth = cameraHeight * Camera.main.aspect;

        float cameraX = Camera.main.transform.position.x;
        float cameraY = Camera.main.transform.position.y;

        _gameZoneX = new Vector2(cameraX - cameraWidth / 2f + _gameZoneOffset.x, cameraX + cameraWidth / 2f - _gameZoneOffset.x);
        _gameZoneY = new Vector2(cameraY - cameraHeight / 2f + _gameZoneOffset.y, cameraY + cameraHeight / 2f - _gameZoneOffset.y);
    }
    public void ChangeScore(int score)
    {
        GameVariables.fullScore += score;
        InGameUI.instance.ChangeScore(GameVariables.fullScore);
    }
    private void ChangeStarsSpeed()
    {
        float directionY = PlayerController.instance.GetInputDirection().y;
        float speed = directionY > 0 ? -3f : (directionY < 0 ? -1f : -2f);

        ParticleSystem.VelocityOverLifetimeModule velocityModule = _stars.velocityOverLifetime;
        velocityModule.y = new ParticleSystem.MinMaxCurve(speed);
        velocityModule.enabled = true;
    }

    private void OnEnable()
    {
        if (PlayerController.instance == null) return;
        PlayerController.instance.OnStartMove += ChangeStarsSpeed;
        PlayerController.instance.OnContinueMove += ChangeStarsSpeed;
        PlayerController.instance.OnFinishMove += ChangeStarsSpeed;
    }
    private void OnDisable()
    {
        if (PlayerController.instance == null) return;
        PlayerController.instance.OnStartMove -= ChangeStarsSpeed;
        PlayerController.instance.OnContinueMove -= ChangeStarsSpeed;
        PlayerController.instance.OnFinishMove -= ChangeStarsSpeed;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector2(_gameZoneX[0], _gameZoneY[1]), new Vector2(_gameZoneX[1], _gameZoneY[1]));
        Gizmos.DrawLine(new Vector2(_gameZoneX[1], _gameZoneY[1]), new Vector2(_gameZoneX[1], _gameZoneY[0]));
        Gizmos.DrawLine(new Vector2(_gameZoneX[1], _gameZoneY[0]), new Vector2(_gameZoneX[0], _gameZoneY[0]));
        Gizmos.DrawLine(new Vector2(_gameZoneX[0], _gameZoneY[0]), new Vector2(_gameZoneX[0], _gameZoneY[1]));
    }

    #region GET
    public Vector2 GetPlayerStartPosition()
    {
        return _playerStartPosition;
    }
    public Vector2 GetGameZoneYForEnemies()
    {
        return new Vector2(_gameZoneY.x - 2, _gameZoneY.y + 2);
    }
    public Vector4 GetGameZone()
    {
        return new Vector4(_gameZoneX.x, _gameZoneX.y, _gameZoneY.x, _gameZoneY.y);
    }
    #endregion
}