using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    

    public float speed = 120f;
    public float nextWaypointDistance = 3f;
    
    public GameObject player;


    public Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    public bool IsChasing = false;
    public bool IsPlayerNearby = false;
    public float distanceToBreakChasing = 3;
   

    Seeker seeker;
    Rigidbody2D rb;
    SpriteRenderer sr;
    Enemy_Instant enemy;
    


    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        enemy = GetComponent<Enemy_Instant>();
         
    }
    void UpdatePath()
    {
        

        if (seeker.IsDone() && IsChasing)
        {
            Vector2 StartPathPoint = rb.position;

            seeker.StartPath(StartPathPoint, player.transform.position, OnPathComplete);

    }
    }
    void OnPathComplete(Path p)
    {
        float distanceToPlayer = Vector2.Distance(rb.position, player.transform.position);
        if (!p.error)
        {
            path = p;
            if (distanceToPlayer > 1f)
                currentWaypoint = 1;
            else
                currentWaypoint = 0;

        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
     
        if (path == null || enemy.animator.GetCurrentAnimatorStateInfo(0).IsName("Hurt"))
                return;
        

        if (currentWaypoint >= path.vectorPath.Count)
            {
                reachedEndOfPath = true;
                return;

            }
            else
            {
                reachedEndOfPath = false;
            }

        float distanceToWayPoint = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        float distanceToPlayer = Vector2.Distance(rb.position, player.transform.position);

       

        if (IsChasing && distanceToPlayer>=distanceToBreakChasing&&!IsPlayerNearby)
                {
                  CancelInvoke("UpdatePath");
                  IsChasing = false;
                }

        if (distanceToPlayer < 0.6f)
        {
            return;
        }
       
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 force = direction * speed;
            force.y = 0f;
           
                rb.velocity = force;
            
           

        if (distanceToWayPoint < nextWaypointDistance)
            {
                currentWaypoint++;
            }

        
    }

   
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" )
        {
            IsPlayerNearby = true;
            if(!IsChasing)
            {
                IsChasing = true;
                InvokeRepeating("UpdatePath", 0f, 0.5f);
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

}
