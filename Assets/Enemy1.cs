using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy1 : MonoBehaviour
{
    private Vector3 closestGrenade;
    public Slider HPBar;
    private Animator animator;
    private NavMeshAgent agent;
    public GameObject enemy1;
    public GameObject enemy2;
    public Slider enemy1HP;
    public Slider enemy2HP;
    public GameObject muzzlePoint;
    public GameObject bullet;
    public static bool deadEN1;
    private static bool underAttack;
    public static float numOfGrenades;
    public GameObject grenadePrefab;

    // Start is called before the first frame update
    void Start()
    {
        numOfGrenades = 0;
        underAttack = false;
        deadEN1 = false;
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
                    case "Follow#MainPlayer":
                        underAttack = false;
                        agent.SetDestination(enemy1.transform.position);
                        animator.SetInteger("state", 1); // Walk
                        break;
                    case "Follow#SecondaryPlayer":
                        underAttack = false;
                        agent.SetDestination(enemy2.transform.position);
                        animator.SetInteger("state", 1); // Walk
                        break;
                    case "ClosestGrenade":
                        agent.stoppingDistance = 0;
                        agent.SetDestination(closestGrenade);
                        animator.SetInteger("state", 1); // Walk
                        break;
                    case "Game Over":
                        animator.SetInteger("state", 0);
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
            if (!deadEN1)
            {
                animator.SetInteger("state", 4);
            }
        }
    }

    private void Die()
    {
        animator.SetInteger("state", 2);//Die
        agent.enabled = false;
        deadEN1 = true;
    }


    IEnumerator disappear()
    {
        float time = animator.GetCurrentAnimatorStateInfo(0).length + animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        yield return new WaitForSeconds(time);
        this.gameObject.SetActive(false);
    }

    private string priorityOperation()
    {
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
            closestGrenade = FindClosestTag("grenade-spawn");
            if (numOfGrenades < 2 && Vector3.Distance(this.gameObject.transform.position, closestGrenade) < 30)
            {
                return "ClosestGrenade";
            }
            else {
                if (enemy1HP.value > 0)
                {
                    return "Follow#MainPlayer";
                }
                else if (enemy2HP.value > 0)
                {
                    return "Follow#SecondaryPlayer";
                }
                else
                {
                    return "Game Over";
                }
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

    public static void beingAttacked()
    {
        underAttack = true;
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
}
