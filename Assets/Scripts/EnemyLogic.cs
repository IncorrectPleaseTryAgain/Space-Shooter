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
    Vector2 targetDirection;
    float targetGravityScale;

    // Unity Built-In Methods
    private void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start() 
    { 
        targetGravityScale = target.GetComponent<PlayerController>().GetGravityScale();
        targetDirection = (target.transform.position - transform.position).normalized; 
    }

    private void Update() { targetDirection = (target.transform.position - transform.position).normalized; }

    private void FixedUpdate()
    {
        rb.AddForce(((targetDirection * targetGravityScale) / targetDirection.magnitude));
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, properties.maxSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;

        // Object == Player
        if (tag.Equals(TagNames.Player))
        {
            PlayerController player = GameObject.FindGameObjectWithTag(TagNames.Player).GetComponent<PlayerController>();
            if (player != null)
            {
                player.ApplyDamage(properties.damage);
            }
            else
            {
                throw new System.NullReferenceException();
            }
        }
        // Object == Projectile
        else if (tag.Equals(TagNames.Projectile))
        {
            ProjectileLogic projectile = GameObject.FindGameObjectWithTag(TagNames.Projectile).GetComponent<ProjectileLogic>();

            if (projectile != null)
            {
                ApplyDamage(projectile.GetDamage());
                GlobalMethods.DestroyObject(collision.gameObject);
            }
            else
            {
                throw new System.NullReferenceException();
            }
        }
    }

    // Getters And Setters
    public void ApplyDamage(float damage)
    {
        properties.health -= damage;

        if(properties.health <= 0) { GlobalMethods.DestroyObject(this.gameObject); }
    }
}
