using UnityEngine;

public class Player : Starship
{
    public static Player instance;
    [SerializeField] private ParticleSystem _getDamageParticle;
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
    protected override void Awake()
    {
        base.Awake();
        Singleton();
    }
    protected override void Start()
    {
        base.Start();
        PlayerController.instance.OnStartMove += FlyAnimation;
        PlayerController.instance.OnFinishMove += FlyAnimation;
        InGameManager.instance.OnVictory += StopAll;
        InGameUI.instance.ChangeHealth(currentHealth, maximumHealth);
        transform.position = InGameManager.instance.GetPlayerStartPosition();
        shootCoroutine = StartCoroutine(Shoot());
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Starship starship))
        {
            if (!IsDead() && !starship.IsDead())
            {
                starship.GetDamage(maximumHealth);
                StarshipCollision();
            }
        }
    }
    private void OnDisable()
    {
        PlayerController.instance.OnStartMove -= FlyAnimation;
        PlayerController.instance.OnFinishMove -= FlyAnimation;
        InGameManager.instance.OnVictory -= StopAll;
    }
    public override void GetDamage(int damage)
    {
        base.GetDamage(damage);
        InGameUI.instance.ChangeHealth(currentHealth, maximumHealth);
        _getDamageParticle.Play();
    }
    private void FlyAnimation()
    {
        ChangeAnimatorState(PlayerController.instance.GetInputDirection(), false);
    }
    private void StarshipCollision()
    {
        GetDamage(maximumHealth);
        InGameUI.instance.ShowDefeat();
    }
    private void StopAll()
    {
        StopAllCoroutines();
    }
}
