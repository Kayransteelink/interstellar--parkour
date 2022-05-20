using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    Rigidbody2D rb;

    //movement speed
    public int jumpheight;
    public float speed;

    public float distance;

    //ground checking
    public Transform groundCheck;
    public bool isGrounded;

    public LayerMask groundMask;

    //wall jumping
    public Transform[] wallCheck;
    bool onWallRight;
    bool onWallLeft;
    public bool onWall;

    public bool wallJump;

    public LayerMask wallMask;
    public float wallGravity;

    public float climbSeconds;

    //ceiling climbing
    public Transform ceilingCheck;
    public bool OnCeiling;

    public LayerMask ceilingMask;

    // Start is called before the first frame update
    void Start()
    {
        //get the players rigidbody2D
         rb = this.GetComponent<Rigidbody2D>();
    }

    //on collision with something
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "MovingPlatform")
        {
            this.transform.parent = collision.transform.parent;
        }
    
        //if it collides with a wall reset velocity
        if(collision.gameObject.tag == "Wall")
        {
            rb.velocity = new Vector2(0, 0);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "MovingPlatform")
        {
            this.transform.parent = null;
        }
    }
    // Update is called once per frame
    void Update()
    {
        //Check if on ground
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.5f, groundMask);

        //check if on a wall
        onWallRight = Physics2D.OverlapCircle(wallCheck[0].position, 0.1f, wallMask);
        onWallLeft = Physics2D.OverlapCircle(wallCheck[1].position, 0.1f, wallMask);
        if (onWallRight || onWallLeft)
        {
            onWall = true;
        }
        else
        {
            onWall = false;
        }

        if (isGrounded)
        {
            wallJump = false;
        }

        //check if on ceiling
        OnCeiling = Physics2D.OverlapCircle(ceilingCheck.position, 0.1f, ceilingMask);


        //Movement
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            float dirX = Input.GetAxis("Horizontal");
            if (wallJump)
            {
                rb.velocity = new Vector2(0, 0);
                wallJump = false;
            }
            if(!onWallLeft && !OnCeiling)
            {
                rb.velocity = new Vector2(dirX * speed, rb.velocity.y);
            }
            if(OnCeiling)
            {
                rb.velocity = new Vector2(dirX * (speed / 2.5f), rb.velocity.y);
            }
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            float dirX = Input.GetAxis("Horizontal");
            if (wallJump)
            {
                rb.velocity = new Vector2(0, 0);
                wallJump = false;
            }
            if (!onWallRight && !OnCeiling)
            {
                rb.velocity = new Vector2(dirX * speed, rb.velocity.y);
            }
            if (OnCeiling)
            {
                rb.velocity = new Vector2(dirX * (speed / 2.5f), rb.velocity.y);
            }
        }

        //Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpheight);
            }

            //walljump
            if (onWall)
            {
                rb.gravityScale = wallGravity;

                if (onWallRight)
                {
                    wallJump = true;
                    rb.velocity = new Vector2(-distance, distance);
                }
                if (onWallLeft)
                {
                    wallJump = true;
                    rb.velocity = new Vector2(distance, distance);
                }
            }
            if(OnCeiling)
            {
                rb.velocity = new Vector2(0, -jumpheight / 2);
            }
            else
            {
                rb.gravityScale = 1;
            }
        }

        //ceiling climbing
        if(OnCeiling)
        {
            rb.gravityScale = -1;
            StartCoroutine(Check());
        }
        else
        {
            rb.gravityScale = 1;
        }
        IEnumerator Check()
        {
            Vector3 lastPos = this.transform.position;
            float seconds = climbSeconds;
            while (seconds > 0)
            {
                seconds -= Time.deltaTime;
                if (lastPos != this.transform.position)
                {
                    yield break;
                }
                lastPos = this.transform.position;
                yield return new WaitForEndOfFrame();
            }
            rb.velocity = new Vector2(0, -jumpheight / 2);
        }
    }
}
