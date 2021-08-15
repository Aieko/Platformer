using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
    private enum Direction
    {
        Horizontal,
        Vertical,
        Diagonal
    }

    private Direction direct;

    Vector2 origPos;
    Vector2 finalPos;
    Vector2 currentPos;
    bool flip = false;
    Collider2D platformCollider;

    
    public float platformSpeed;
    public float xPosChange;
    public float yPosChange;
    

    private void Start()
    {
        if(xPosChange > 0 && yPosChange > 0)
        {
            direct = Direction.Diagonal;
        }
        else if(xPosChange > 0)
        {
            direct = Direction.Horizontal;
        }
        else if(yPosChange > 0)
        {
            direct = Direction.Vertical;
        }

        platformCollider = GetComponent<Collider2D>();
        origPos = transform.position;
        currentPos = transform.position;
        finalPos.x = transform.position.x + xPosChange;
        finalPos.y = transform.position.y + yPosChange;

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        CheckFlip();
        Move();

        currentPos = transform.position;
    }

    private void CheckFlip()
    {
        if (currentPos.x > finalPos.x || currentPos.y > finalPos.y)
            flip = true;
        else if(currentPos.x < origPos.x || currentPos.y < origPos.y)
            flip = false;

    }

    private void Move()
    {
        if(direct == Direction.Diagonal)
        {
            Moving(platformSpeed, platformSpeed);
        }
        if(direct == Direction.Horizontal)
        {
            Moving(platformSpeed, 0);
        }
        else if(direct == Direction.Vertical)
        {
            Moving(0, platformSpeed);
        }
       
    }

    private void Moving(float x, float y)
    {
        if (!flip)
        {
            transform.position = new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(transform.position.x - x, transform.position.y - y, transform.position.z);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            collision.collider.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.transform.SetParent(null);
        }
    }
}
