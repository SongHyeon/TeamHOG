using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    private Vector2 startingPoint;
    public float speed = 8f;
    public float stopDistance = 5f;
    public float awareDistance = 10f;
    public Animator animator;
    
    private Transform target;

    // Start is called before the first frame update
    void Start()
    {
        startingPoint = transform.position;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        Debug.Log(target);
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position, target.position ) > stopDistance && Vector2.Distance(transform.position, target.position) < awareDistance)
        {
            float move = transform.position.x - target.position.x;
            Flip(move);
            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }
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
