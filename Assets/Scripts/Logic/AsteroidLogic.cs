using System;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent (typeof(Animator))]
public class AsteroidLogic : MonoBehaviour
{
    private const string TAG_PLAYER = "Player";
    private const string TAG_PROJECTILE = "Projectile";
    private const float MIN_AFFECTED_GRAVITY_SCALE = 10f;

    [Serializable]
    private struct Properties
    {
        public string name; 
        public string description;

        public float health;
        public float damage;
        public float maxSpeed;

        public float attackDelayTimer;

        public GameObject positionIndicatorPrefab;

        public List<AudioClip> deathSFX;
    }
    [SerializeField] private Properties properties;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] private List<Target> targets;

    // Velocity before paused
    private Vector2 previousVelocity;
    private PositionIndicatorLogic positionIndicatorLogic;

    // Attack delay
    private float timerCount;
    private bool isTimerOn = false;

    [SerializeField] private bool _isPaused = false;
    public bool IsPaused
    {
        get { return _isPaused; }
        private set { _isPaused = value; }
    }

    [SerializeField]
    bool _isAlive = true;
    public bool IsAlive
    {
        get { return _isAlive; }
        private set
        {
            _isAlive = value;
            anim.SetBool("isAlive", value);
        }
    }

    [Serializable]
    struct Target
    {
        public GameObject obj;
        public float gravityScale;
        internal Target(GameObject obj, float gScale, float dist)
        {
            this.obj = obj;
            this.gravityScale = gScale;
        }
    }
    private Vector2 targetDirection;
    private float gravityScale;


    private void Awake()
    {
        StateManager.OnGameStateChanged += OnGameStateChangedHandler;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    private void OnDestroy() { StateManager.OnGameStateChanged -= OnGameStateChangedHandler; }

    private void OnGameStateChangedHandler(GameStates state)
    {
        switch (state)
        {
            case GameStates.GamePause:
                GamePauseHandler();
                break;
            case GameStates.GameResume:
                GameResumeHandler();
                break;
            case GameStates.PlayerDeath:
                PlayerDeathHandler();
                break;
        }
    }
    private void GamePauseHandler()
    {
        IsPaused = true;
        rb.velocity = Vector2.zero;
        previousVelocity = rb.velocity;
    }
    private void GameResumeHandler()
    {
        IsPaused = false;
        rb.velocity = previousVelocity;
    }
    private void PlayerDeathHandler()
    {
        IsAlive = false;
        DeathHandler();
    }

    private void Start()
    {        
        // Create Position Indicator
        positionIndicatorLogic = Instantiate(properties.positionIndicatorPrefab).GetComponent<PositionIndicatorLogic>();
        positionIndicatorLogic.SetFollowTarget(this.gameObject);

        // Add Player
        Target player = new() { obj = GameObject.FindGameObjectWithTag(TAG_PLAYER) };
        if(player.obj != null )
        {
            player.gravityScale = player.obj.GetComponent<PlayerController>().GetGravityScale();
            targets.Add(player);
        }
    }

    private void Update()
    {
        if (isTimerOn)
        {
            timerCount -= Time.deltaTime;
            if (timerCount <= 0f) { isTimerOn = false; }
        }
        else
        {
            if (targets.Count > 0)
            {
                gravityScale = 0;
                for (int i = 0; i < targets.Count; i++)
                {
                    targetDirection = new Vector2((targetDirection.x + (targets[i].obj.transform.position.x - transform.position.x) * targets[i].gravityScale),
                                                (targetDirection.y + (targets[i].obj.transform.position.y - transform.position.y) * targets[i].gravityScale));

                    float distanceFromEnemy = GetDistanceFromEnemy(targets[i].obj);
                    gravityScale += (targets[i].gravityScale / (distanceFromEnemy + 1f)) + MIN_AFFECTED_GRAVITY_SCALE;
                }

                MathF.Abs(gravityScale);
                targetDirection = targetDirection.normalized;
            }
        }
    }
    private float GetDistanceFromEnemy(GameObject closestTarget)
    {
        return MathF.Sqrt(MathF.Pow((closestTarget.transform.position.x - transform.position.x), 2) +
            MathF.Pow((closestTarget.transform.position.y - transform.position.y), 2));
    }

    private void FixedUpdate()
    {
        if (!IsPaused)
        {
            if (!isTimerOn)
            {
                rb.AddForce(targetDirection * gravityScale);
                rb.velocity = Vector2.ClampMagnitude(rb.velocity, properties.maxSpeed);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isPaused)
        {
            if (collision.CompareTag(TAG_PLAYER))
            {
                if (collision.gameObject.TryGetComponent<PlayerController>(out PlayerController component))
                {
                    component.ApplyDamage(properties.damage);

                    // Set and activate attack delay timer
                    timerCount = properties.attackDelayTimer;
                    isTimerOn = true;
                }
            }
            else if (collision.CompareTag(TAG_PROJECTILE))
            {
                if (collision.gameObject.TryGetComponent<ProjectileLogic>(out ProjectileLogic projectile))
                {
                    projectile.PlayHitSFX();
                    ApplyDamage(projectile.GetDamage());
                    Destroy(collision.gameObject);
                }
            }
        }
    }

    void OnBecameInvisible() { if (positionIndicatorLogic) { positionIndicatorLogic.SetActive(true); } }
    void OnBecameVisible() { if (positionIndicatorLogic) { positionIndicatorLogic.SetActive(false); } }

    public void ApplyDamage(float damage)
    {
        if (IsAlive)
        {
            properties.health -= damage;
            IsAlive = properties.health > 0;

            if (!IsAlive) { DeathHandler(); }
        }
    }

    private void DeathHandler()
    {
        if (positionIndicatorLogic.gameObject) { Destroy(positionIndicatorLogic.gameObject); }
        AudioManager.instance.PlayEnemySFX(properties.deathSFX);
        StateManager.instance.UpdateGameState(GameStates.EnemyDeath);
    }

    public void AnimEventHandlerDeath() { Destroy(this.gameObject); }
}