using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyLogic : MonoBehaviour
{
    // Components
    Rigidbody2D rb;
    GameObject target;

    // Properties
    [Serializable]
    struct Properties
    {
        public string name;
        public string description;

        public float health;
        public float damage;
        public float maxSpeed;
    }
    [SerializeField]
    Properties properties;

    // Private
    [SerializeField]
    bool inRange = false;

    Vector2 targetDirection;
    float targetGravityScale;

    // Unity Built-In Methods
    private void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");
        targetGravityScale = target.GetComponent<PlayerController>().GetGravityScale();
    }

    private void Start() { targetDirection = (target.transform.position - transform.position).normalized; }

    private void Update() 
    {
        targetDirection = (target.transform.position - transform.position).normalized;
    }

    private void FixedUpdate()
    {
        rb.AddForce(Vector2.ClampMagnitude(((targetDirection * targetGravityScale) / targetDirection.magnitude), properties.maxSpeed));
    }
}
