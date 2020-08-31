using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayAnimation : MonoBehaviour
{
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    IEnumerator Delay(float sec)
    {
        animator.speed = 0.0f;
        yield return new WaitForSeconds(sec);
        animator.speed = 1f;
    }

}
