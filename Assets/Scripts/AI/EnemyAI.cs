using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{

    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;// How much to smooth out the movement
    [Range(0.1f, 1f)] [SerializeField] private float PathUpdate = 0.1f;
    public float speed = 120f;
    public float nextWaypointDistance = 3f;
    public float distanceToStopMove = 0.6f;
    [SerializeField] private Transform m_GroundCheck;
    [SerializeField] private LayerMask m_WhatIsGround;
    const float k_GroundCheckRadius = .2f; // Radius of the overlap circle to determine if grounded
    public GameObject player;


    public Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    public bool IsChasing { get; set; }
    public bool IsPlayerNearby { get; set; }
    private bool m_FacingRight = false;   // For determining which way the player is currently facing.
    [SerializeField] private bool CanChase = false;
    [SerializeField] private bool CanJump = true;


    bool IsPatrol = false;

    public float m_JumpForce = 200f;
    public float attackRate = 0.5f;
    float jumpRate = 0.8f;
    float nextAttackTime = 0f;
    float nextJumpTime = 0f;
    public float distanceToBreakChasing = 3;
    protected float distanceToPlayer;

    private Vector2 m_Velocity = Vector2.zero;

    Seeker seeker;
    Rigidbody2D rb;
    SpriteRenderer sr;
    Enemy_Instant enemy;
    Animator animator;



    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        enemy = GetComponent<Enemy_Instant>();
        animator = GetComponent<Animator>();

    }
    void UpdatePath()
    {
        if (seeker.IsDone() && IsChasing)
        {
            Vector2 StartPathPoint = rb.position;

            seeker.StartPath(StartPathPoint, player.transform.position, OnPathCompleteDelegate);

        }
    }
    void OnPathCompleteDelegate(Path p)
    {
        //float distanceToPlayer = Vector2.Distance(rb.position, player.transform.position);
        if (!p.error)
        {
            path = p;
            if (rb.position.y + 0.5f < player.transform.position.y)
                currentWaypoint = 2;
            else if (distanceToPlayer > 1f)
                currentWaypoint = path.vectorPath.Count - 1;

        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (path == null
         || enemy.animator.GetCurrentAnimatorStateInfo(0).IsName("Hurt")
         || currentWaypoint > path.vectorPath.Count)
            return;

        
        distanceToPlayer = Vector2.Distance(rb.position, player.transform.position);

        Targeting();

        if (IsChasing || IsPatrol
      && !animator.GetCurrentAnimatorStateInfo(0).IsName("Hurt")
      && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            {
                if (path.vectorPath[System.Math.Abs(currentWaypoint - 1)].x > transform.position.x && !m_FacingRight)
                {
                    Flip();
                }
                else if (path.vectorPath[System.Math.Abs(currentWaypoint - 1)].x < transform.position.x && m_FacingRight)
                {
                    Flip();
                }
            } 
        }

        if (Time.time >= nextAttackTime)
        {
            Attack();
        }

        if (CanJump && Time.time >= nextJumpTime)
        {
            Jump();
        }
    }

    void Jump()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundCheckRadius, m_WhatIsGround);

        if (player.transform.position.y - 0.5f > this.transform.position.y)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                rb.AddForce(new Vector2(0f, m_JumpForce), ForceMode2D.Impulse);

                enemy.Jump();
                nextJumpTime = Time.time + 1f / jumpRate;
            }
        }
        
    }

    void Attack()
    {
            if(IsChasing 
            && distanceToPlayer <= distanceToStopMove 
            && !animator.GetCurrentAnimatorStateInfo(0).IsName("Hurt"))
        {
                enemy.Attack();
                nextAttackTime = Time.time + 1f / attackRate;
        }
    }

   void Targeting()
    {   
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        if (IsChasing && distanceToPlayer >= distanceToBreakChasing && !IsPlayerNearby)
        {
            CancelInvoke("UpdatePath");
            IsChasing = false;
            return;
        }

        if (distanceToPlayer < distanceToStopMove || enemy.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            return;
        }

        float distanceToWayPoint = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (CanChase)
        {
            Chasing(distanceToWayPoint);
        }

        if (distanceToWayPoint < nextWaypointDistance)
        {
            currentWaypoint++;
        }

    }

    void Chasing(float distanceToWayPoint)
    {
        if (!reachedEndOfPath)
        {

            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 force = new Vector2(direction.x * speed, rb.velocity.y);

            rb.velocity = force;

        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!enabled)
            return;
        
        if (collision.gameObject.tag == "Player" )
        {
            IsPlayerNearby = true;
            if(!IsChasing)
            {
                IsChasing = true;
                InvokeRepeating("UpdatePath", 0f, PathUpdate);
            }
           
        }
        
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            IsPlayerNearby = false;
        }
    }
    private void Flip()
    {
        // Switch the way the enemy is labelled as facing.
        m_FacingRight = !m_FacingRight;
        // Multiply the enemy's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

    }

    public bool getFacingDir()
    {
        return m_FacingRight;
    }
}
