using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    

    public float speed = 120f;
    public float nextWaypointDistance = 3f;
    
    public GameObject player;


    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    public bool IsChasing = false;
    public float distanceToBreakChasing = 3;


    Seeker seeker;
    Rigidbody2D rb;
    SpriteRenderer sr;
    


    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
       
        
        
        
    }
    void UpdatePath()
    {
        if(seeker.IsDone() && IsChasing)
        {
            seeker.StartPath(rb.position, player.transform.position, OnPathComplete);
        }
    }
    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
     
        if (path == null)
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
            
            if(IsChasing && distanceToPlayer>distanceToBreakChasing)
                {
                  CancelInvoke();
                  IsChasing = false;
                }

            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 force = direction * speed * Time.deltaTime;

            rb.AddForce(force);

            if (distanceToWayPoint < nextWaypointDistance)
            {
                currentWaypoint++;
            }

            if (rb.velocity.x >= 0.01f)
            {
                sr.flipX = true;
            }
            else if (rb.velocity.x <= -0.01f)
            {
                sr.flipX = false;
            }
        
       
    }

    void Attack()
    {
        if(reachedEndOfPath && IsChasing)
        {

        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if(!IsChasing)
            {
                IsChasing = true;
                InvokeRepeating("UpdatePath", 0f, .5f);
            }
           
        }
    }
}
