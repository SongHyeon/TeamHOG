﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    public float speed = 8f;
    public float stopDistance = 5f;
    public float awareDistance = 10f;
    public Animator animator;
    private Transform target;
    private Rigidbody2D rigidBody;
    public float attackDelayTime = 0.1f;
    private float attackStartTime = 0f;
    DelayAnimation delayAnimation;
    enum BossState
    {
        IDLE = 0,
        MOVE,
        ATTACK,
        BEATEN,
        DEAD
    }
    BossState state;

    // Start is called before the first frame update
    void Start()
    {
        delayAnimation = GetComponentInChildren<DelayAnimation>();
        state = BossState.IDLE;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == BossState.DEAD)
            return;
        state = TryMove(state);
        state = TryAttack(state);
    }

    private BossState TryAttack(BossState state)
    {
        if(state == BossState.IDLE
        && Time.time - attackStartTime > attackDelayTime
        && Vector2.Distance(transform.position, target.position ) <= stopDistance
        && animator.GetBool("Attacking")==false)
        {
            animator.SetBool("Attacking", true);
            animator.SetTrigger(string.Format("Attack{0}",Random.Range(1,4)));

            delayAnimation.standbyTime = Random.Range(0f, 2f);
            attackStartTime = Time.time + delayAnimation.standbyTime;
            
            return BossState.ATTACK;
        }
        return animator.GetBool("Attacking") ? BossState.ATTACK:BossState.IDLE;
    }

    private BossState TryMove(BossState state)
    {
        // State 가 ATTACK 아닐때 움직 일 수 있음.
        if(state != BossState.ATTACK 
        && Vector2.Distance(transform.position, target.position ) > stopDistance 
        && Vector2.Distance(transform.position, target.position) < awareDistance)
        {
            float move = transform.position.x - target.position.x;
            Flip(move);
            animator.SetBool("IsWalking", true);
            Vector3 velocity = rigidBody.velocity;
            velocity.x = speed*(move>0?-1f:1f);
            rigidBody.velocity = velocity;
            return BossState.MOVE;
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }
        return BossState.IDLE;
    }
    private void Flip(float move)
    {
        Vector3 charcterScale = transform.localScale;
        if (move < 0)
        {
            charcterScale.x = -1f * Mathf.Abs(transform.localScale.x);
        }
        else
        {
            charcterScale.x = 1f * Mathf.Abs(transform.localScale.x);
        }
        transform.localScale = charcterScale;

    }


}
