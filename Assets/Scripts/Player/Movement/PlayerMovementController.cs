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
    public bool onWallRight;
    public bool onWallLeft;
    public bool onWall;

    public bool wallJump;

    public LayerMask wallMask;
    public float wallGravity;

    // Start is called before the first frame update
    void Start()
    {
        //get the players rigidbody2D
         rb = this.GetComponent<Rigidbody2D>();
    }

    //on collision with something
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if it collides with a wall reset velocity
        if(collision.gameObject.tag == "Wall")
        {
            rb.velocity = new Vector2(0, 0);
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

        //Movement
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (wallJump)
            {
                rb.velocity = new Vector2(0, 0);
                wallJump = false;
            }
            if(!onWallLeft)
            {
                float dirX = Input.GetAxis("Horizontal");
                rb.velocity = new Vector2(dirX * speed, rb.velocity.y);
            }
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            if (wallJump)
            {
                rb.velocity = new Vector2(0, 0);
                wallJump = false;
            }
            if (!onWallRight)
            {
                float dirX = Input.GetAxis("Horizontal");
                rb.velocity = new Vector2(dirX * speed, rb.velocity.y);
            }
        }

        //Jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpheight);
        }

        //walljump
        if (onWall)
        {
            rb.gravityScale = wallGravity;

            if(Input.GetKeyDown(KeyCode.Space) && onWallRight)
            {
                wallJump = true;
                rb.velocity = new Vector2(-distance, distance);
            }
            if (Input.GetKeyDown(KeyCode.Space) && onWallLeft)
            {
                wallJump = true;
                rb.velocity = new Vector2(distance, distance);
            }
        }
        else
        {
            rb.gravityScale = 1;
        }
    }
}
