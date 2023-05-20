using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public event Action OnStartMove;
    public event Action OnContinueMove;
    public event Action OnFinishMove;

    private Player _player;
    private Vector2 direction = Vector2.zero;
    private Vector2 _previousDirection = Vector2.zero;
    private bool _canMove = true;

    private void Awake()
    {
        _player = GetComponent<Player>();
        Singleton();
    }
    private void Start()
    {
        InGameManager.instance.OnVictory += StopAll;
    }
    private void OnDisable()
    {
        InGameManager.instance.OnVictory -= StopAll;
    }
    private void Update()
    {
        Input();
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
    private void Input()
    {
        if (_player == null || _player.IsDead() || InGameUI.instance. joystick == null || !_canMove) return;

        direction = new Vector2(InGameUI.instance.joystick.Horizontal, InGameUI.instance.joystick.Vertical);

        if (direction != _previousDirection)
        {
            if (_previousDirection == Vector2.zero && direction != Vector2.zero)
            {
                OnStartMove?.Invoke();
            }
            else if (_previousDirection != Vector2.zero && direction == Vector2.zero)
            {
                OnFinishMove?.Invoke();
            }
            else
            {
                OnContinueMove?.Invoke();
            }
        }
        _previousDirection = direction;

        Move(direction);
    }
    private void Move(Vector2 direction)
    {
        if (_player == null || _player.IsDead() || InGameUI.instance.joystick == null || !_canMove) return;
        Vector2 clampedDirection = Vector2.ClampMagnitude(direction.normalized * _player.GetMoveSpeed() * Time.deltaTime, _player.GetMoveSpeed());
        transform.Translate(clampedDirection);
        MoveClamp();
    }
    private void MoveClamp()
    {
        if (_player == null || _player.IsDead() || InGameUI.instance.joystick == null || !_canMove) return;
        Vector4 positionLimit = InGameManager.instance.GetGameZone();
        float clampedPositionX = Mathf.Clamp(transform.position.x, positionLimit.x, positionLimit.y);
        float clampedPositionY = Mathf.Clamp(transform.position.y, positionLimit.z, positionLimit.w);

        Vector2 clampedPlayerPos = new Vector2(clampedPositionX, clampedPositionY);
        transform.position = clampedPlayerPos;
    }
    public Vector2 GetInputDirection()
    {
        return direction;
    }
    private void StopAll()
    {
        _canMove= false;
        StopAllCoroutines();
    }
}
