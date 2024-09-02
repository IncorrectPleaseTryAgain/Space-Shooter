using System;
using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent (typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Serializable]
    private struct Properties
    {
        public string name; 
        public string description;

        public float health;
        public float maxSpeed;
        public float acceleration;
        public float gravityScale;


        public List<AudioClip> deathSFX;
        public List<AudioClip> hitSFX;
    }
    [SerializeField] private Properties properties;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject projectileSpawner;
    [SerializeField] private CinemachineImpulseSource impulseSource;

    private Vector2 moveInput;

    [SerializeField] private bool _isMoving = false;
    public bool IsMoving
    {
        get { return _isMoving; }
        private set
        {
            if (_isMoving != value)
            {
                _isMoving = value;
            }
        }
    }

    [SerializeField] private bool _isAlive = true;
    public bool IsAlive
    {
        get { return _isAlive; }
        private set
        {
            _isAlive = value;
            anim.SetBool("isAlive", value); 
        }
    }

    [SerializeField] private bool _isPaused = false;
    public bool IsPaused
    {
        get { return _isPaused; }
        private set{ _isPaused = value; }
    }

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
                IsPaused = true;
                break;
            case GameStates.GameResume:
                IsPaused = false;
                break;
        }
    }

    private void Update()
    {
        if (IsAlive)
        {
            RotateToMousePosition();
        }
    }
    private void FixedUpdate()
    {
        if (IsAlive && !IsPaused)
        {
            rb.velocity = Vector2.ClampMagnitude(rb.velocity + (moveInput * properties.acceleration), properties.maxSpeed);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!IsPaused && IsAlive)
        {
            moveInput = context.ReadValue<Vector2>();
            IsMoving = moveInput != Vector2.zero;
        }
    }

    private void RotateToMousePosition()
    {
        // Get mouse position
        Vector2 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Rotate spaceship towards mouse position
        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        transform.up = direction;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!IsPaused && IsAlive)
        {
            if (context.performed) 
            { 
                projectileSpawner.GetComponent<ProjectileSpawnerLogic>().SpawnProjectile(projectilePrefab, rb.velocity);
            }
        }
    }

    public void ApplyDamage(float damage) 
    {
        if (!IsPaused && IsAlive)
        {
            properties.health -= damage;
            IsAlive = properties.health > 0;
            if (IsAlive)
            {
                impulseSource.GenerateImpulseWithForce(1f);
                AudioManager.instance.PlayPlayerSFX(properties.hitSFX);
            }
            else 
            {
                AudioManager.instance.PlayPlayerSFX(properties.deathSFX);
                StateManager.instance.UpdateGameState(GameStates.PlayerDeath);
            }
        }
    }


    //public Vector2 GetVelocity() { return rb.velocity; }
    public float GetGravityScale() { return properties.gravityScale; }
}
