using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private AudioSource auodiSource;
    public AudioClip deathClip;
    private GameManager gameManager;

    private bool isDead = false;
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
        auodiSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        textY = GameObject.Find("Square");
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
    }

    private void Update()
    {
        if (jumpCount > 0 && (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1")))
        {
            auodiSource.time = 0.12f;
            auodiSource.Play();
            jumpCount--;
            isJumping = true;
            rigidBody.velocity = new(rigidBody.velocity.x, 0);
            rigidBody.AddForce(rigidBody.transform.up * jumpSpeed, ForceMode2D.Impulse);
            rigidBody.gravityScale *= 0.25f;
            if (!isGrounded)
            {
                animator.SetTrigger("JumpAgain");
            }
        }


        if (isJumping)
        {
            jumpTimer += Time.deltaTime;
            if (jumpTimer >= jumpDuration || Input.GetKeyUp(KeyCode.Space) || Input.GetButtonUp("Fire1"))
            {
                jumpTimer = 0;
                rigidBody.gravityScale *= 4f;
                isJumping = false;
            }
        }
        animator.SetBool("Grounded", isGrounded);
    }

    private void FixedUpdate()
    {
        textY.transform.position = new(transform.position.x, textY.transform.position.y);
        textY.transform.position -= transform.up * Time.deltaTime;

        if (transform.position.y + transform.up.y > textY.transform.position.y)
        {
            textY.transform.position = transform.position + transform.up;
            textY.GetComponent<TextMeshPro>().text = $"Y : {transform.position.y:N2}";
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Platform")
            && collision.contacts[0].normal == Vector2.up)
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
        if (!isDead && collision.CompareTag("DeadZone"))
        {
            OnDie();
        }
    }

    public void OnDie()
    {
        isDead = true;
        rigidBody.velocity = new(0f, rigidBody.velocity.y);
        animator.SetTrigger("Die");
        auodiSource.PlayOneShot(deathClip);

        gameManager.EndGame();
    }
}
