using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;

    private bool isGrounded = false;
    public float speed = 20f;

    public float jumpSpeed = 8f;
    public int maxJump = 1;
    private int jumpCount = 1;

    bool isJumping = false;
    private float jumpTimer = 0;
    public float jumpDuration = 0.25f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (jumpCount > 0 && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            jumpCount--;
            isJumping = true;
            rb.velocity = new(rb.velocity.x, 0);
            rb.AddForce(rb.transform.up * jumpSpeed, ForceMode2D.Impulse);
            rb.gravityScale = 0.5f;
            if(!isGrounded)
            {
                anim.SetTrigger("JumpAgain");
            }
        }


        if (isJumping)
        {
            jumpTimer += Time.deltaTime;
            if (jumpTimer >= jumpDuration || Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonUp(0))
            {
                jumpTimer = 0;
                rb.gravityScale = 1f;
                isJumping = false;
            }
        }



        rb.velocity = new(Input.GetAxisRaw("Horizontal") * speed, rb.velocity.y);

        if (rb.velocity.x > 0)
        {
            sr.flipX = false;
        }
        else if (rb.velocity.x < 0)
        {
            sr.flipX = true;
        }

        anim.SetBool("Grounded", isGrounded);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Platform"))
        {
            isGrounded = true;
            jumpCount = maxJump;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Platform"))
            isGrounded = false;
    }
}
