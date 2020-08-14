using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float dmgValue = 4;
    public GameObject throwObject;
    public Transform attackCheck;
    public bool timeCheck = false;

    public float noOfClick = 0;
    float lastClickedTime = 0;
    float maxComboDelay = 0.9f;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Time.time - lastClickedTime > maxComboDelay)
        {
            noOfClick = 0;
        }

        if (Input.GetKeyDown(KeyCode.Z) && noOfClick != 3)
        {
            lastClickedTime = Time.time;
            noOfClick++;
            if(noOfClick == 1)
            {
                animator.SetBool("Attack1", true);
            }
            noOfClick = Mathf.Clamp(noOfClick, 0, 3);
        }
    }

    public void return1()
    {
        if (noOfClick >= 2)
        {
            animator.SetBool("Attack2", true);
        }
        else
        {
            animator.SetBool("Attack1", false);
        }
    }
    public void return2()
    {
        if (noOfClick >= 3)
        {
            animator.SetBool("Attack3", true);
        }
        else
        {
            animator.SetBool("Attack2", false);
            animator.SetBool("Attack1", false);
        }
    }
    public void return3()
    {
        noOfClick = 0;
        animator.SetBool("Attack1", false);
        animator.SetBool("Attack2", false);
        animator.SetBool("Attack3", false);
    }

    public void Damage()
    {
        dmgValue = Mathf.Abs(dmgValue);
        Collider2D[] collidersEnemies = Physics2D.OverlapCircleAll(attackCheck.position, 0.9f);
        for (int i = 0; i < collidersEnemies.Length; i++)
        {
            if (collidersEnemies[i].gameObject.tag == "Enemy")
            {
                if (collidersEnemies[i].transform.position.x - transform.position.x < 0)
                {
                    dmgValue = -dmgValue;
                }
                collidersEnemies[i].gameObject.SendMessage("ApplyDamage", dmgValue);
                CameraShake.Instance.ShakeCamera(5f, .1f);
            }
        }

    }
}
