using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossAnimationStateController : MonoBehaviour
{
    // Start is called before the first frame update
    Animator animator;
    FinalBossMovement stats;
    private int Life;
    private float velocity;
    private bool isAttacking;
    private bool GetsHit;
    private int AttackType;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        stats = GetComponent<FinalBossMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        Life = stats.Life;
        velocity = Mathf.Abs((stats.agent.velocity.x + stats.agent.velocity.z) / 2);
        isAttacking = stats.isAttacking;
        GetsHit = stats.hit;
        AttackType = stats.AttackType;
        animator.SetInteger("Life", Life);
        animator.SetFloat("Velocity", velocity);
        animator.SetBool("IsAttacking", isAttacking);
        animator.SetBool("GetsHit", GetsHit);
        animator.SetInteger("AttackType", AttackType);
        
    }
}
