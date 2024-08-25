using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Timeline;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyLogic : MonoBehaviour
{
    [SerializeField]
    Vector2 velocity;

    // Components
    [SerializeField]
    Rigidbody2D rb;
    [SerializeField]
    Camera cam;
    [SerializeField]
    GameObject enemyMarker;
    [SerializeField]
    List<Target> targets;
    int numTargets = 0;

    [SerializeField]
    Vector2 initialVelocity;

    bool play = false;

    GameObject marker;
    bool enemyMarkerActive = false;

    [SerializeField]
    Vector2 camPos;
    [SerializeField]
    Vector2 camSize;

    [Serializable]
    struct Target
    {
        public GameObject obj;
        public float gravityScale;
        public float distanceFromEnemy;
        internal Target(GameObject obj, float gScale, float dist)
        {
            this.obj = obj;
            this.gravityScale = gScale;
            this.distanceFromEnemy = dist;
        }

    }

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
    Vector2 targetDirection;
    [SerializeField]
    float gravityScale;

    // Unity Built-In Methods

    private void OnDestroy()
    {
        StateManager.OnGameStateChanged -= OnGameStateChangedHandler;
    }

    private void OnGameStateChangedHandler(GameStates state)
    {
        play = (state == GameStates.Play);
    }

    private void Awake()
    {
        StateManager.OnGameStateChanged += OnGameStateChangedHandler;
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    private void Start()
    {
        marker = Instantiate(enemyMarker, camPos, Quaternion.Euler(0, 0, 0));
        camSize = new Vector2(cam.orthographicSize * 2f * cam.aspect, cam.orthographicSize * 2f);
        marker.SetActive(false);

        // Add Player
        Target player = new Target();
        player.obj = GameObject.FindGameObjectWithTag(TagNames.Player);

        if (play)
        {
            while (player.obj == null)
            {
                player.obj = GameObject.FindGameObjectWithTag(TagNames.Player);
            }
        }
        if(player.obj != null )
        {
            player.gravityScale = player.obj.GetComponent<PlayerController>().GetGravityScale();
            player.distanceFromEnemy = GetDistanceFromEnemy(player.obj);
            targets.Add(player);
            numTargets++;
        }

        GameObject[] gravityPoints = GameObject.FindGameObjectsWithTag(TagNames.GravityPoint);

        for (int i = 0; i < gravityPoints.Length; i++)
        {
            Target gravityPoint = new Target();
            gravityPoint.obj = gravityPoints[i];
            gravityPoint.gravityScale = gravityPoints[i].GetComponent<GravityPointLogic>().GetGravityScale();
            gravityPoint.distanceFromEnemy = GetDistanceFromEnemy(gravityPoint.obj);

            targets.Add(gravityPoint);
            numTargets++;
        }

        rb.velocity = initialVelocity;
    }

    private void Update()
    {
        gravityScale = 0;
        if (numTargets > 0)
        {
            for (int i = 0; i < numTargets; i++)
            {
                targets[i] = new Target(targets[i].obj, targets[i].gravityScale, GetDistanceFromEnemy(targets[i].obj));

                targetDirection = new Vector2((targetDirection.x + (targets[i].obj.transform.position.x - transform.position.x) * targets[i].gravityScale),
                                            (targetDirection.y + (targets[i].obj.transform.position.y - transform.position.y) * targets[i].gravityScale));;
                if(targets[i].distanceFromEnemy <= 0.5)
                {
                    gravityScale += targets[i].gravityScale / (targets[i].distanceFromEnemy + 0.5f);
                }
                else
                {
                    gravityScale += targets[i].gravityScale / targets[i].distanceFromEnemy;
                }

            }

            MathF.Abs(gravityScale);
            targetDirection = targetDirection.normalized;
        }

        if (enemyMarkerActive)
        {
            camPos = cam.transform.position;
            float camLeftEdge = camPos.x - (camSize.x / 2);
            float camRightEdge = camPos.x + (camSize.x / 2);
            float camTopEdge = camPos.y + (camSize.y / 2);
            float camBottomEdge = camPos.y - (camSize.y / 2);

            //TOP
            if (transform.position.y > (camPos.y + camSize.y / 2))
            {
                //Debug.Log("Top");
                float x = Math.Clamp(this.transform.position.x, camLeftEdge, camRightEdge);
                marker.transform.position = new Vector2(x, camPos.y + 4.87f);
                marker.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            // Bottom
            else if (transform.position.y < (camPos.y - camSize.y / 2))
            {
                //Debug.Log("Bottom");
                float x = Math.Clamp(this.transform.position.x, camLeftEdge, camRightEdge);
                marker.transform.position = new Vector2(x, camPos.y -4.87f);
                marker.transform.rotation = Quaternion.Euler(0, 0, 180);
            }
            // Left
            else if (transform.position.x < (camPos.x - camSize.x / 2))
            {
                //Debug.Log("Left");
                float y = Math.Clamp(this.transform.position.y, camBottomEdge, camTopEdge);
                marker.transform.position = new Vector2(camPos.x -8.75f, y);
                marker.transform.rotation = Quaternion.Euler(0, 0, 90);
            }
            // Right
            else if (transform.position.x > (camPos.x + camSize.x / 2))
            {
                //Debug.Log("Right");
                float y = Math.Clamp(this.transform.position.y, camBottomEdge, camTopEdge);
                marker.transform.position = new Vector2(camPos.x + 8.75f, y);
                marker.transform.rotation = Quaternion.Euler(0, 0, -90);
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
        if (!StateManager.instance.IsPaused)
        {
            rb.AddForce(targetDirection * gravityScale);
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, properties.maxSpeed);
        }
        else
        {
            rb.velocity = Vector2.zero;
        }

        rb.AddForce(targetDirection * gravityScale);
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, properties.maxSpeed);

        velocity = rb.velocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!StateManager.instance.IsPaused)
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
    }

    void OnBecameInvisible()
    {
        enemyMarkerActive = true;
        if (marker) { marker.SetActive(true); }
        //Debug.Log("VISIBLE: false");
    }

    void OnBecameVisible()
    {
        enemyMarkerActive = false;
        if (marker) { marker.SetActive(false); }
        marker.transform.position = Vector3.zero;
        //Debug.Log("VISIBLE: true");
    }

    // Getters And Setters
    public void ApplyDamage(float damage)
    {
        if (!StateManager.instance.IsPaused)
        {
            properties.health -= damage;

            if (properties.health <= 0)
            {
                GlobalMethods.DestroyObject(marker);
                GlobalMethods.DestroyObject(this.gameObject);
                StateManager.instance.UpdateGameState(GameStates.EnemyKilled);
            }
        }
    }
}