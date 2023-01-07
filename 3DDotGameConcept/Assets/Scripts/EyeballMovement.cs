using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AudioSource))]
public class EyeballMovement : MonoBehaviour
{
    public GameObject destination1;
    public GameObject destination2;
    public GameObject coin;
    public Transform enemy;
    public GameObject player;
    public NavMeshAgent agent;
    public LayerMask obstacleMask;
    public LayerMask playerMask;
    public float rangeX;
    public float rangeZ;
    public int Life;
    public float visionRange;
    public float visionAngle;
    public bool hit;
    public bool isAttacking;
    public float attackRate;
    public AudioClip LevelUp;
    public AudioClip Groan;

    private bool PlayerSeen;
    private float lastTimeAttack;
    private float distX;
    private float distZ;
    private bool reachedDest1;
    private bool reachedDest2;
    private Vector3 initialVelocity;
    private bool dead;
    // Start is called before the first frame update
    void Start()
    {
        agent.destination = destination1.transform.position;
        isAttacking = false;
        hit = false;
        PlayerSeen = false;
        reachedDest1 = false;
        reachedDest2 = false;
        if (visionRange == 0) visionRange = 20;
        if (visionAngle == 0) visionAngle = 90;
        if (attackRate == 0) attackRate = 1;
        initialVelocity = agent.desiredVelocity;
        dead = false;
    }

    // Update is called once per frame
    void Update()
    {
        distX = enemy.transform.position.x - player.transform.position.x;
        distZ = enemy.transform.position.z - player.transform.position.z;
        float rate = Time.deltaTime - lastTimeAttack;
        if (!dead)
        {
            if (!PlayerSeen)
            {
                Patroling();
            }
            else if (PlayerSeen && (Mathf.Abs(distX) <= rangeX && Mathf.Abs(distZ) <= rangeZ) && !isAttacking)
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

    void FixedUpdate()
    {
        if (!dead)
        {
            Vector3 playerTarget = (player.transform.position - enemy.position).normalized;
            if (Vector3.Angle(transform.forward, playerTarget) < visionAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(enemy.position, player.transform.position);
                if (distanceToTarget <= visionRange)
                {
                    if (Physics.Raycast(enemy.position, playerTarget, distanceToTarget, obstacleMask) == false)
                    {

                        PlayerSeen = true;
                        if (!isAttacking)
                        {
                            agent.SetDestination(player.transform.position);
                        }
                    }
                    else if (Physics.Raycast(enemy.position, playerTarget, distanceToTarget, obstacleMask) == true)
                    {
                        PlayerSeen = false;
                        if (reachedDest1)
                        {
                            agent.SetDestination(destination2.transform.position);
                        }
                        else if (reachedDest2)
                        {
                            agent.SetDestination(destination1.transform.position);
                        }
                        Patroling();
                    }
                }

            }
            else
            {
                PlayerSeen = false;
                if (reachedDest1)
                {
                    agent.SetDestination(destination2.transform.position);
                }
                else if (reachedDest2)
                {
                    agent.SetDestination(destination1.transform.position);
                }
                else
                {
                    agent.SetDestination(destination1.transform.position);
                }

                Patroling();
            }

        }
    }

    private void Patroling()
    {
        if (agent.destination == null) agent.SetDestination(destination1.transform.position);
        if (enemy.position.x == destination1.transform.position.x && enemy.position.z == destination1.transform.position.z && !reachedDest1)
        {
            reachedDest1 = true;
            reachedDest2 = false;
            agent.SetDestination(destination2.transform.position);
        }
        if (enemy.position.x == destination2.transform.position.x && enemy.position.z == destination2.transform.position.z && !reachedDest2)
        {
            reachedDest1 = false;
            reachedDest2 = true;
            agent.SetDestination(destination1.transform.position);
        }
        if (reachedDest1 && reachedDest2)
        {
            reachedDest1 = false;
            reachedDest2 = false;
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
