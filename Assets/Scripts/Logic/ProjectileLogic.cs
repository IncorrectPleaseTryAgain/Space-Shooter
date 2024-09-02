using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ProjectileLogic : MonoBehaviour
{
    [Serializable]
    public struct Properties
    {
        public string name;
        public string description;

        public float speed;
        public float lifetime; // Seconds
        public float damage;
        public List<AudioClip> shotSFX;
        public List<AudioClip> hitSFX;
    }
    [SerializeField] private Properties properties;
    [SerializeField] private Rigidbody2D rb;

    private void Awake() { rb = GetComponent<Rigidbody2D>(); }
    public void Shoot(Vector2 velocity)
    {
        rb.velocity = velocity;
        rb.velocity += new Vector2(transform.up.x * properties.speed, transform.up.y * properties.speed);
        PlayShotSFX();
    }

    private void PlayShotSFX() { AudioManager.instance.PlayProjectileSFX(properties.shotSFX); }
    public float GetDamage() { return properties.damage; }
    public void PlayHitSFX() { AudioManager.instance.PlayProjectileSFX(properties.hitSFX); }
}
