using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayAnimation : MonoBehaviour
{
    Animator animator;
    public float standbyTime; // Delay 호출 이전에 스크립트에서 셋팅
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    IEnumerator Delay()
    {
        animator.speed = 0.0f;
        yield return new WaitForSeconds(standbyTime);
        animator.speed = 1f;
    }

}
