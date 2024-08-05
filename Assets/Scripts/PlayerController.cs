using System;
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

    [Serializable]
    public struct Properties
    {
        public string name;
        public string description;

        public float health;
        public float maxSpeed;
        public float acceleration;
        public float gravityScale;
    }
    [SerializeField]
    Properties properties;

    // Getters And Setters

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
            if(_isMoving != value)
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
        }
    }

    // Unity Built-In Methods
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (_isMoving)
        {
            rb.velocity = Vector2.ClampMagnitude(rb.velocity + (moveInput * properties.acceleration), properties.maxSpeed);
        }
    }

    private void Update()
    {
        // Get mouse position
        Vector2 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Point player to mouse position
        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        transform.up = direction;
    }


    // Unity Event Methods
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        IsMoving = moveInput != Vector2.zero;
    }
}
