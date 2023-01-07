using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AudioSource))]
public class SoldierMovement : MonoBehaviour
{
    public Transform enemy;
    public GameObject player;
    public GameObject coin;
    public NavMeshAgent agent;
    public LayerMask obstacleMask;
    public LayerMask playerMask;
    public float rangeX;
    public float rangeZ;
    public int Life;
    public float visionRange;
    public float visionAngle;
    public AudioClip LevelUp;
    public AudioClip Groan;
    public float attackRate;
    public bool hit;
    public bool isAttacking;
    
    private float lastTimeAttack;
    private bool PlayerSeen;
    private float distX;
    private float distZ;
    private Vector3 initialVelocity;
    private bool dead;
    // Start is called before the first frame update
    void Start()
    {
        agent.destination = enemy.position;
        isAttacking = false;
        hit = false;
        PlayerSeen = false;
        dead = false;
        if (visionRange == 0) visionRange = 20;
        if (visionAngle == 0) visionAngle = 90;
        if (attackRate == 0) attackRate = 1;
        lastTimeAttack = Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        distX = enemy.transform.position.x - player.transform.position.x;
        distZ = enemy.transform.position.z - player.transform.position.z;
        if (!dead)
        {
            Vector3 playerTarget = (player.transform.position - enemy.position).normalized;
            if (Vector3.Angle(transform.forward, playerTarget) < visionAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(enemy.position, player.transform.position);
                if (distanceToTarget <= visionRange)
                {
                    Debug.DrawRay(enemy.position, enemy.forward);
                    if (Physics.Raycast(enemy.position, playerTarget, distanceToTarget, obstacleMask) == false)
                    {
                        agent.SetDestination(player.transform.position);
                        PlayerSeen = true;
                    }
                    else
                    {
                        PlayerSeen = false;
                    }
                }
            }
            else
            {
                PlayerSeen = false;
            }

            float rate = Time.deltaTime - lastTimeAttack;

            if (PlayerSeen && (Mathf.Abs(distX) <= rangeX && Mathf.Abs(distZ) <= rangeZ) && !isAttacking)
            {
                AttackPlayer();
            }
        }

        if (Life <= 0 && !dead)
        {
            dead = true;
            StartCoroutine(Dead());
        }
    }


    private void AttackPlayer()
    {
        //CheckAnimation for attacking, make Player Receive damage
        isAttacking = true;
        lastTimeAttack = Time.deltaTime;
        StartCoroutine(Attack());
    }

    public void getHit()
    {
        StartCoroutine(Hit());
        --Life;
    }

    IEnumerator Hit()
    {
        hit = true;
        AudioSource.PlayClipAtPoint(Groan, transform.position);
        yield return new WaitForSeconds(0.5f);
        hit = false;
    }

    IEnumerator Dead()
    {
        yield return new WaitForSeconds(2.0f);
        AudioSource.PlayClipAtPoint(LevelUp, transform.position);
        player.GetComponent<Player>().ObtainHealth();
        Vector3 coinPosition = new Vector3(transform.position.x, 0.5f, transform.position.z);
        var coinInstance = Instantiate (coin, coinPosition, Quaternion.identity) as GameObject;
        Destroy(gameObject);
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        initialVelocity = agent.velocity;
        agent.velocity = new Vector3(0, 0, 0);
        agent.SetDestination(enemy.position);
        yield return new WaitForSeconds(1.0f);
        agent.velocity = initialVelocity;
        isAttacking = false;
    }

    public bool isDead() {
        return dead;
    }
}
