using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private AudioSource auodioSource;

    private bool isGrounded = false;
    public float speed = 20f;

    public float jumpSpeed = 5f;
    public int maxJump = 1;
    private int jumpCount = 1;

    bool isJumping = false;
    private float jumpTimer = 0;
    public float jumpDuration = 0.25f;

    public GameObject textY;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        auodioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        textY = GameObject.Find("Square");
    }

    private void Update()
    {
        if (jumpCount > 0 && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            auodioSource.Play();
            jumpCount--;
            isJumping = true;
            rigidBody.velocity = new(rigidBody.velocity.x, 0);
            rigidBody.AddForce(rigidBody.transform.up * jumpSpeed, ForceMode2D.Impulse);
            rigidBody.gravityScale = 0.25f;
            if (!isGrounded)
            {
                animator.SetTrigger("JumpAgain");
            }
        }


        if (isJumping)
        {
            jumpTimer += Time.deltaTime;
            if (jumpTimer >= jumpDuration || Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonUp(0))
            {
                jumpTimer = 0;
                rigidBody.gravityScale = 1f;
                isJumping = false;
            }
        }



        rigidBody.velocity = new(Input.GetAxisRaw("Horizontal") * speed, rigidBody.velocity.y);

        if (rigidBody.velocity.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (rigidBody.velocity.x < 0)
        {
            spriteRenderer.flipX = true;
        }

        animator.SetBool("Grounded", isGrounded);
        textY.transform.position = new(transform.position.x, textY.transform.position.y);
        if (transform.position.y + transform.up.y > textY.transform.position.y)
        {
            textY.transform.position = transform.position + transform.up;
            textY.GetComponent<TextMeshPro>().text = $"Y : {transform.position.y:N2}";
        }
        else
        {
            textY.transform.position -= transform.up * Time.deltaTime;
        }
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DeadZone"))
        {
            OnDie();
        }
    }

    public void OnDie()
    {
        gameObject.SetActive(false);
        SceneManager.LoadScene("Main");
    }
}
