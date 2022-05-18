using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public int jumpheight;
    public float speed;

    public float distance;


    Rigidbody2D rb;

    public Transform groundCheck;
    public bool isGrounded;
    public LayerMask groundMask;

    public Transform[] wallCheck;
    public bool onWallright;
    public bool onWallLeft;
    public bool onWall;
    public LayerMask wallMask;
    public float wallGravity;

    // Start is called before the first frame update
    void Start()
    {
         rb = this.GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
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
        onWallright = Physics2D.OverlapCircle(wallCheck[0].position, 0.1f, wallMask);
        onWallLeft = Physics2D.OverlapCircle(wallCheck[1].position, 0.1f, wallMask);
        if (onWallright || onWallLeft)
        {
            onWall = true;
        }
        else
        {
            onWall = false;
        }

        //Movement
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            this.transform.position -= this.transform.right * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.position += this.transform.right * speed * Time.deltaTime;
        }

        //Jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(0, jumpheight);
        }

        //walljump
        if (onWall)
        {
            rb.gravityScale = wallGravity;

            if(Input.GetKeyDown(KeyCode.Space) && onWallright)
            {
                rb.velocity = new Vector2(-distance, distance);
            }
            if (Input.GetKeyDown(KeyCode.Space) && onWallLeft)
            {
                rb.velocity = new Vector2(distance, distance);
            }
        }
        else
        {
            rb.gravityScale = 1;
        }
    }
}
