using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{

    public float jumpForce = 15f;
    float movSpeed = 7f;
    float dirX;
    public LayerMask jumpableGround;

    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sprite;
    CapsuleCollider2D coll;

        
    public AudioSource jumpSoundEffect;
    public AudioSource fruitsCollectEffect;
    public AudioSource deathSoundEffect;

    enum MovementState { idle, running, jumping, falling}


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<CapsuleCollider2D>();
    }


    void Update()
    {
        //dirX = Input.GetAxis("Horizontal");   for touch and keyboard
        rb.velocity = new Vector2(dirX * movSpeed, rb.velocity.y);

        //if(Input.GetKeyDown("space") && isGrounded())
        //{
        //    jumpSoundEffect.Play();
        //    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        //}

        AnimationUpdate();
    }

    void AnimationUpdate()
    {
        MovementState state;

        if (dirX > 0)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }

        else if (dirX < 0)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }

        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
            

        else if(rb.velocity.y < -.1f)
            state = MovementState.falling;

        anim.SetInteger("state", (int)state);
    }

    bool isGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    private void OnTriggerEnter2D(Collider2D collision)          // check cheery collid with player or not 
    {
        if(collision.gameObject.CompareTag("cherry"))
        {
            fruitsCollectEffect.Play();
            Destroy(collision.gameObject);
        }

        if(collision.gameObject.CompareTag("orange"))
        {
            fruitsCollectEffect.Play();
            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Trap"))
        {
            Die();
        }
    }
     
    void Die()
    {
        deathSoundEffect.Play();
        rb.bodyType = RigidbodyType2D.Static;   // for stop the player movement after collid with Trap 
        anim.SetTrigger("death");
        //Invoke("RestartLevel", 2f);
    }

    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void onPointerEnter_Right()
    {
        dirX = 1;
    }

    public void onPointerExit()
    {
        dirX = 0;
    }

    public void onPointerEnter_Left()
    {
        dirX = -1;
    }

    public void onPointerUp()
    {
        if(isGrounded())
        {
            jumpSoundEffect.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    public void onPointerDown()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
    }
}
