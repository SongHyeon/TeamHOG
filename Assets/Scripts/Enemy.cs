using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float life = 10;
    public float speed = 5f;
    public float enemyAndPlayer = 10f;
    public float attackRange = 5f;
    public float attackDelay = 1f;

    public LayerMask turnLayerMask;

    private bool isPlat;
    private bool isObstacle;
    private Transform fallCheck;
    private Transform wallCheck;
    private Transform player;
    private Animator animator;
    private float attackTime = 0.0f;
    private Rigidbody2D rb;


    public bool isInvincible = false;
    private bool facingRight = true;
    private bool isHitted = false;

    private bool isAttacking = false;


    void Awake()
    {
        fallCheck = transform.Find("FallCheck");
        wallCheck = transform.Find("WallCheck");
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        animator.ResetTrigger("Attack");
        if (life <= 0)
        {
            animator.SetBool("IsDead", true);
            StartCoroutine(DestroyEnemy());
        }

        isPlat = Physics2D.OverlapCircle(fallCheck.position, .2f, 1 << LayerMask.NameToLayer("Ground"));
        isObstacle = Physics2D.OverlapCircle(wallCheck.position, .2f, turnLayerMask);
        if (Vector2.Distance(player.position, rb.position) < enemyAndPlayer && isPlat)
        {
            LookAtPlayer();
            Vector2 target = new Vector2(player.position.x, player.position.y);
            Vector2 newPos = Vector2.MoveTowards(rb.position, new Vector2(target.x, rb.position.y), speed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
            Debug.Log(Time.time - attackTime);
            if (Vector2.Distance(player.position, rb.position) < attackRange && Time.time - attackTime > attackDelay)
            {
                
                isAttacking = true;
                attackTime = Time.time;
                animator.SetTrigger("Attack"); // 공격해라
                GetComponent<Enemy1_Attack>().Attack();
            }
        }
        else
        {
            if (!isHitted && life > 0 && Mathf.Abs(rb.velocity.y) < 0.5f)
            {
                if (isPlat && !isObstacle && !isHitted)
                {
                    if (facingRight)
                    {
                        rb.velocity = new Vector2(speed, rb.velocity.y);
                    }
                    else
                    {
                        rb.velocity = new Vector2(-speed, rb.velocity.y);
                    }
                }
                else
                {
                    // Debug.Log("Come to flip");
                    Flip();
                }
            }
           
        }
  }



    void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (transform.position.x > player.position.x && facingRight)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            facingRight = false;
        }
        else if (transform.position.x < player.position.x && !facingRight)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            facingRight = true;
        }
    }

    void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;
        // Debug.Log("Flip" + facingRight);

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void ApplyDamage(float damage) // Enemy Damaged
    {
        if (!isInvincible)
        {
            float direction = damage / Mathf.Abs(damage);
            damage = Mathf.Abs(damage);
            animator.SetBool("Dead",true);
            life -= damage;
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(direction * 500f, 100f));
            StartCoroutine(HitTime());
        }
    }


    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && life > 0)
        {
           // collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(10);
        }
    }

    IEnumerator HitTime()
    {
        isHitted = true;
        isInvincible = true;
        yield return new WaitForSeconds(0.1f);
        isHitted = false;
        isInvincible = false;
        animator.SetBool("Attack", false);
    }

    IEnumerator DestroyEnemy()
    {
        CapsuleCollider2D capsule = GetComponent<CapsuleCollider2D>();
        capsule.size = new Vector2(1f, 0.25f);
        capsule.offset = new Vector2(0f, -0.8f);
        capsule.direction = CapsuleDirection2D.Horizontal;
        yield return new WaitForSeconds(0.25f);
        rb.velocity = new Vector2(0, rb.velocity.y);
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
