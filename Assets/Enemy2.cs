using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy2 : MonoBehaviour
{
    public Slider HPBar;
    private Animator animator;
    private NavMeshAgent agent;
    public static float numOfGrenades;
    public GameObject ally;
    public Slider enemy1HP;
    public Slider enemy2HP;
    public GameObject enemy1;
    public GameObject enemy2;
    public Slider allyHP;
    public GameObject muzzlePoint;
    public GameObject bullet;
    public GameObject grenadePrefab;
    public static bool deadEN2;
    private static bool underAttack;


    // Start is called before the first frame update
    void Start()
    {
        numOfGrenades = 0;
        underAttack = false;
        deadEN2 = false;
        HPBar.maxValue = 100;
        HPBar.value = 100;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerOperations.inGameOverUI)
        {
            if (HPBar.value > HPBar.maxValue * 0.15 || underAttack)
            {
                agent.stoppingDistance = 20;
                switch (priorityOperation())
                {
                    case "Attack#MainPlayer":
                        attackMode(enemy1);
                        break;
                    case "Attack#SecondaryPlayer":
                        attackMode(enemy2);
                        break;
                    case "Follow":
                        underAttack = false;
                        Vector3 closestGrenade = FindClosestTag("grenade-spawn");
                        if (Vector3.Distance(this.gameObject.transform.position, closestGrenade) < 30)
                        {
                            agent.stoppingDistance = 0;
                            agent.SetDestination(closestGrenade);
                            animator.SetInteger("state", 1); // Walk
                        }
                        else if (Vector3.Distance(agent.transform.position, ally.transform.position) > agent.stoppingDistance)
                        {
                            agent.SetDestination(ally.transform.position); // runs A* to find path to target
                            animator.SetInteger("state", 1); // Walk
                        }
                        else
                        {
                            animator.SetInteger("state", 0); // Idle
                        }
                        break;
                    case "Follow#MainPlayer":
                        underAttack = false;
                        animator.SetInteger("state", 1); // Walk
                        agent.SetDestination(enemy1.transform.position);
                        break;
                    case "Follow#SecondaryPlayer":
                        underAttack = false;
                        animator.SetInteger("state", 1); // Walk
                        agent.SetDestination(enemy2.transform.position);
                        break;
                    case "Game Over":
                        animator.SetInteger("state", 0); // Idle
                        break;
                }
            }
            else
            {
                if (HPBar.value > 0)
                {
                    agent.stoppingDistance = 0;
                    animator.SetInteger("state", 1);
                    agent.SetDestination(FindClosestTag("hp-box"));
                }
                else
                {
                    Die();
                    StartCoroutine(disappear());
                }
            }
        }
        else
        {
            if (!deadEN2)
            {
                animator.SetInteger("state", 4);
            }
        }
    }

    private void Die()
    {
        animator.SetInteger("state", 2);//Die
        agent.enabled = false;
        deadEN2 = true;
    }


    IEnumerator disappear()
    {
        float time = animator.GetCurrentAnimatorStateInfo(0).length + animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        yield return new WaitForSeconds(time);
        this.gameObject.SetActive(false);
    }

    private string priorityOperation()
    {
        bool alone = allyHP.value <= 0 ? true : false ;
        if (enemy1HP.value > 0 && Vector3.Distance(agent.transform.position, enemy1.transform.position) <= 40)
        {
            return "Attack#MainPlayer";
        }
        else if (enemy2HP.value > 0 && Vector3.Distance(agent.transform.position, enemy2.transform.position) <= 40)
        {
            return "Attack#SecondaryPlayer";
        }
        else
        {
            if (alone)
            {
                if(enemy1HP.value > 0)
                {
                    return "Follow#MainPlayer";
                }
                else if(enemy2HP.value > 0)
                {
                    return "Follow#SecondaryPlayer";
                }
                else
                {
                    return "Game Over";
                }
            }
            else
            {
                return "Follow";
            }
            
        }
    }

    private void attackMode(GameObject enemy)
    {
        if (numOfGrenades > 0)
        {
            agent.transform.LookAt(new Vector3(enemy.transform.position.x, agent.transform.position.y, enemy.transform.position.z));
            ThrowGrenade.grenadePrefab = grenadePrefab;
            ThrowGrenade.throwForce = 20;
            ThrowGrenade.throwGrenade(muzzlePoint);
            numOfGrenades--;
        }
        else
        {
            RaycastHit hit;
            animator.SetInteger("state", 1);
            agent.transform.LookAt(new Vector3(enemy.transform.position.x, agent.transform.position.y, enemy.transform.position.z));
            if (Physics.Raycast(muzzlePoint.transform.position, muzzlePoint.transform.forward, out hit))
            {
                bullet.SetActive(true);
                bullet.transform.position = hit.point;
            }
        }
    }

    private Vector3 FindClosestTag(string findTag)
    {
        GameObject[] tags;
        tags = GameObject.FindGameObjectsWithTag(findTag);
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject tag in tags)
        {
            float curDistance = Vector3.Distance(tag.transform.position, position);
            if (curDistance < distance)
            {
                closest = tag;
                distance = curDistance;
            }
        }
        if (closest == null)
        {
            return new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity);
        }
        else
        {
            return closest.transform.position;
        }
    }

    public static void beingAttacked()
    {
        underAttack = true;
    }
}
