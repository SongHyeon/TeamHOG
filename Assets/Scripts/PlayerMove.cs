using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float upPower = 5.8f;
    public float fallPower = 2.5f;
    public float lowJumpMultipler = 2f;
    [SerializeField] private LayerMask platformerLayerMask;
    public Animator animator;
    public Transform boss;

    private Rigidbody2D rb;
    private CapsuleCollider2D capsuleCollider2D;
    private Transform fallCheck;


    public ParticleSystem particleJumpUp; //Trail particles
    public ParticleSystem particleJumpDown; //Explosion particles




    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        fallCheck = transform.Find("FallCheck");
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        Physics2D.IgnoreCollision(boss.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump(); // 점프
    }


    void Move()
    {
        float move = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(0, 0, 0);
        if (move != 0)
        {
            Flip(move);
            animator.SetBool("IsRunning", true);
            movement = new Vector3(move, 0f, 0f);
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }
        transform.position += movement * Time.deltaTime * moveSpeed;
    }


    void Jump()
    {
        if (IsGrounded() && Input.GetButtonDown("Jump"))
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, upPower), ForceMode2D.Impulse);
        }

        if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultipler - 1) * Time.deltaTime;
            particleJumpDown.Play();
            particleJumpUp.Play();
            animator.SetBool("IsJumping", true);
        }
        else if (rb.velocity.y < 0 && Physics2D.OverlapCircle(fallCheck.position, 0.5f, 1 << LayerMask.NameToLayer("Ground")))
        {
            //Debug.Log("Come to falling animation");
            animator.SetBool("IsFalling", false);
            animator.SetBool("PlayFalling", true);
        }
        else if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallPower - 1) * Time.deltaTime;
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsFalling", true);
        }
        else if(rb.velocity.y > 0)
        {
            particleJumpDown.Play();
            particleJumpUp.Play();
            animator.SetBool("IsJumping", true);
        }
        else
        {
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsFalling", false);
            animator.SetBool("PlayFalling", false);
        }
    }


    private bool IsGrounded()  
    {
        RaycastHit2D raycastHit2D = Physics2D.CapsuleCast(capsuleCollider2D.bounds.center, capsuleCollider2D.bounds.size, CapsuleDirection2D.Vertical, 0f, Vector2.down, .1f, platformerLayerMask);
        if (raycastHit2D.collider != null)
        {
            animator.SetBool("IsGrounded", true);
        }
        else
        {
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsGrounded", false);
        }
        return raycastHit2D.collider != null;
    }

    private void Flip(float move)
    {
        Vector3 charcterScale = transform.localScale;
        if (move < 0)
        {
            charcterScale.x = -1f;
        }
        else
        {
            charcterScale.x = 1f;
        }
        transform.localScale = charcterScale;

    }

    
}
