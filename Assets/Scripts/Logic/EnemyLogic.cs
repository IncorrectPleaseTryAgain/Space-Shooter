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
    Animator anim;
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

    float timer = 0.5f;
    float timerCount;
    bool isTimerOn = false;

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

    private void OnDestroy()
    {
        StateManager.OnGameStateChanged -= OnGameStateChangedHandler;
    }

    private void OnGameStateChangedHandler(GameStates state)
    {
        play = (state == GameStates.Resume);
        if(state == GameStates.Dead)
        {
            DestroySelf();
            if (this.marker != null) { GlobalMethods.DestroyObject(this.marker); }
        }
    }

    private void DestroySelf()
    {
        GlobalMethods.DestroyObject(this.gameObject);
    }

    private void Awake()
    {
        StateManager.OnGameStateChanged += OnGameStateChangedHandler;
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        marker = Instantiate(enemyMarker, camPos, Quaternion.Euler(0, 0, 0));
        camSize = new Vector2(cam.orthographicSize * 2f * cam.aspect, cam.orthographicSize * 2f);
        marker.SetActive(true);

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
        if (isTimerOn)
        {
            timerCount -= Time.deltaTime;

            if (timerCount <= 0f)
            {
                isTimerOn = false;
                Debug.Log("Timer End");
            }
        }

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

        if (marker != null && marker.activeSelf)
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

            if (!isTimerOn)
            {
                rb.AddForce(targetDirection * gravityScale);
                rb.velocity = Vector2.ClampMagnitude(rb.velocity, properties.maxSpeed);
                velocity = rb.velocity;
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
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

                    timerCount = timer;
                    isTimerOn = true;
                    Debug.Log("Timer Start");
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
                    if (!StateManager.instance.IsPaused)
                    {
                        ApplyDamage(projectile.GetDamage());
                    }
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
        if (marker) { marker.SetActive(true); }
        //Debug.Log("VISIBLE: false");
    }

    void OnBecameVisible()
    {
        if (marker) { marker.SetActive(false); }
        marker.transform.position = Vector3.zero;
        //Debug.Log("VISIBLE: true");
    }

    // Getters And Setters
    public void ApplyDamage(float damage)
    {

        if (IsAlive)
        {
            properties.health -= damage;
            IsAlive = properties.health > 0;
            if (IsAlive)
            {
                SoundFXManager.instance.PlaySoundFXClip(Sounds.instance.EnemyHit, transform, 0.3f);
            }
            else
            {
                EnemyDeathHandler();
            }
        }

        //if (properties.health <= 0)
        //{
        //    GlobalMethods.DestroyObject(marker);
        //    DestroySelf();
        //    StateManager.instance.UpdateGameState(GameStates.EnemyKilled);
        //}
    }

    private void EnemyDeathHandler()
    {
        Debug.Log("Enemy KIlled");
        SoundFXManager.instance.PlaySoundFXClip(Sounds.instance.EnemyDeath, transform, 0.1f);
        GlobalMethods.DestroyObject(marker);
        StateManager.instance.UpdateGameState(GameStates.EnemyKilled);
    }
}