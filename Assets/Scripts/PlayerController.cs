using System;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent (typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    // Components
    [SerializeField]
    Rigidbody2D rb;
    [SerializeField]
    Animator anim;
    [SerializeField]
    GameObject projectileSpawner;
    [SerializeField]
    CinemachineImpulseSource impulseSource;

    [SerializeField]
    bool isSoundOn = true;

    // Properties
    [Serializable]
    struct Properties
    {
        public string name;
        public string description;

        public float health;
        public float maxSpeed;
        public float acceleration;
        public float gravityScale;

        public GameObject _projectile;
    }
    [SerializeField]
    Properties properties;

    // Private
    Vector2 moveInput;

    // Properties
    [SerializeField]
    bool _isMoving = false;
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

    [SerializeField]
    bool _isAlive = true;
    public bool IsAlive
    {
        get
        {
            return _isAlive;
        }
        private set
        {
            _isAlive = value;
            anim.SetBool("isAlive", value); 
        }
    }

    // Unity Built-In Methods
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if (IsAlive)
        {

            // Get mouse position
            Vector2 mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            // Point player to mouse position
            Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
            transform.up = direction;
        }
    }

    private void PlayerDeathHandler()
    {
        Debug.Log("DEAD");
        if (isSoundOn)
        {
            int rand = UnityEngine.Random.Range(1, 1000);
            if (rand == 1) { SoundFXManager.instance.PlaySoundFXClip(Sounds.instance.PlayerDeathRare, transform, 1.5f); }
            else { SoundFXManager.instance.PlaySoundFXClip(Sounds.instance.PlayerDeath, transform, 1.5f); }
        }

        StateManager.instance.UpdateGameState(GameStates.Dead);
    }

    private void FixedUpdate()
    {
        //if (_isMoving && !StateManager.instance.IsPaused)
        if(_isAlive && !StateManager.instance.IsPaused)
        {
            rb.velocity = Vector2.ClampMagnitude(rb.velocity + (moveInput * properties.acceleration), properties.maxSpeed);
        }
    }

    // Unity Event Methods
    public void OnMove(InputAction.CallbackContext context)
    {
        if (!StateManager.instance.IsPaused && IsAlive)
        {
            moveInput = context.ReadValue<Vector2>();
            IsMoving = moveInput != Vector2.zero;
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!StateManager.instance.IsPaused && IsAlive)
        {
            Debug.Log("At");
            // Check that action is complete
            // Action only happens once per input
            if (context.performed)
            {
                Instantiate(properties._projectile, projectileSpawner.transform.position, transform.rotation);
                if (isSoundOn) { SoundFXManager.instance.PlaySoundFXClip(Sounds.instance.PlayerShoot, transform, 0.1f); }
            }
        }
    }

    // Getters And Setters
    public Vector2 GetVelocity() { return rb.velocity; }
    public float GetGravityScale() { return properties.gravityScale; }

    public void ApplyDamage(float damage) 
    {
        if (!StateManager.instance.IsPaused && IsAlive)
        {
            properties.health -= damage;
            IsAlive = properties.health > 0;
            if (IsAlive)
            {
                impulseSource.GenerateImpulseWithForce(1f);
                if (isSoundOn) { SoundFXManager.instance.PlaySoundFXClip(Sounds.instance.PlayerHit, transform, 0.3f); }
            }
            else
            {
                PlayerDeathHandler();
            }
        }
    }
}
