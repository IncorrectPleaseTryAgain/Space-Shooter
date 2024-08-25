using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ProjectileLogic : MonoBehaviour
{
    // Components
    [SerializeField]
    Rigidbody2D rb;

    // Properties
    [Serializable]
    public struct Properties
    {
        public string name;
        public string description;

        public float speed;
        public float damage;
    }
    [SerializeField]
    Properties properties;

    // Unity Built-In Methods
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Vector2 startVel = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().GetVelocity();

        rb.velocity = new Vector2(startVel.x + (transform.up.x * properties.speed), startVel.y + (transform.up.y * properties.speed));
    }

    private void OnBecameInvisible()
    {
        if (!GlobalMethods.DestroyObject(this.gameObject)) { Debug.LogError(LogErrors.DestroyFailed + this.gameObject); }
    }

    // Getters And Setters
    public float GetDamage() { return properties.damage; }
}
