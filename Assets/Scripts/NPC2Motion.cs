using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC2Motion : MonoBehaviour
{
    private Vector3 vector3 = new Vector3(60f,2.33f,-20.17f);
    private NavMeshAgent agent;
    public GameObject target;
    private LineRenderer line;
    private Animator animator;
    private bool isLastStop;
    // Start is called before the first frame update
    void Start()
    {
        isLastStop = false;
        agent = GetComponent<NavMeshAgent>();
        line = GetComponent<LineRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(animator.GetInteger("Status"));
        if (animator.GetInteger("Status") == 0)
        {
            agent.SetDestination(target.transform.position);
            line.positionCount = agent.path.corners.Length;
            line.SetPositions(agent.path.corners);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == target.gameObject.name) // NPC enters the target collider
        { // move the target to a new position within the terrain
            if (isLastStop)
            {
                animator.SetInteger("Status", 1);
            }
            else
            {
                target.transform.position = vector3;
                isLastStop = true;
            }
        }
    }
}
