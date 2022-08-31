using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Ally : MonoBehaviour
{
    public static float numOfGrenades;
    public Slider HPBar;
    private Animator animator;
    private NavMeshAgent agent;
    public GameObject ally;
    private float runningSpeed;
    private float walkingSpeed;
    private static bool underAttack;
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject muzzlePoint;
    public GameObject bullet;
    public Slider enemy1HP;
    public Slider enemy2HP;
    public GameObject grenadePrefab;

    // Start is called before the first frame update
    void Start()
    {
        numOfGrenades = 0;
        underAttack = false;
        HPBar.maxValue = 100;
        HPBar.value = 100;
        runningSpeed = 25;
        walkingSpeed = 10;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (HPBar.value > HPBar.maxValue*0.15 || underAttack) {
            agent.stoppingDistance = 20;
            switch (priorityOperation()) {
                case "Attack#1":
                    attackMode(enemy1);
                    break;
                case "Attack#2":
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
                        if (Vector3.Distance(agent.transform.position, ally.transform.position) > agent.stoppingDistance + 20)
                        {
                            agent.speed = runningSpeed;
                        }
                        else
                        {
                            agent.speed = walkingSpeed;
                        }
                    }
                    else
                    {
                        animator.SetInteger("state", 0); // Idle
                    }
                    break;
            }
        }
        else
        {
            if(HPBar.value > 0)
            {

                agent.stoppingDistance = 0;
                animator.SetInteger("state", 1);
                agent.SetDestination(FindClosestTag("hp-box"));                
            }
            else {
                Die();
                StartCoroutine(disappear());
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
        else {
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

    IEnumerator disappear()
    {
        float time = animator.GetCurrentAnimatorStateInfo(0).length + animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        yield return new WaitForSeconds(time/2);
        this.gameObject.SetActive(false);
    }

    private string priorityOperation()
    {
        if (enemy1HP.value > 0 && Vector3.Distance(agent.transform.position, enemy1.transform.position) <= 40)
        {
            return "Attack#1";
        }
        else if (enemy2HP.value > 0 && Vector3.Distance(agent.transform.position, enemy2.transform.position) <= 40)
        {
            return "Attack#2";
        }
        else
        {
            return "Follow";
        }
    }

    private void Die()
    {
        animator.SetInteger("state", 2);//Die
        agent.enabled = false;
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
            float curDistance = Vector3.Distance(tag.transform.position,position);
            if (curDistance < distance)
            {
                closest = tag;
                distance = curDistance;
            }
        }
        if(closest == null)
        {
            return new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity);
        }
        else
        {
            return closest.transform.position;
        }
    }

    public static void beingAttacked() {
        underAttack = true;
    }
}
