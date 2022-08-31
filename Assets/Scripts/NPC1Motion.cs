using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC1Motion : MonoBehaviour
{
    private Vector3[] vector3 = new Vector3[11];
    private NavMeshAgent agent;
    public GameObject target;
    private LineRenderer line;
    private Animator animator;
    private static int index;
    // Start is called before the first frame update
    void Start()
    {
        index = 0;
        agent = GetComponent<NavMeshAgent>();
        line = GetComponent<LineRenderer>();
        animator = GetComponent<Animator>();
        vector3[0] = new Vector3(45f, 1.33f, 1.91f);
        vector3[1] = new Vector3(45f, 1.33f, -34.8f);
        vector3[2] = new Vector3(85.9f, 1.33f, -34.8f);
        vector3[3] = new Vector3(85.9f, 1.33f, -19.5f);//before open 1st door
        vector3[4] = new Vector3(54.41f, 2.263f, -13.62f);//before open 2nd door
        vector3[5] = new Vector3(51.67f, 13.14f, -19.29f);//before open 3rd door
        vector3[6] = new Vector3(66.94f, 13.34f, -17.15f);//animation status to 2 for 11 seconds
        vector3[7] = new Vector3(53.51f,13.34f,-18.6f);//before open 3rd door
        vector3[8] = new Vector3(51.42f,2.263f,-13.9f);//before open 2nd door
        vector3[9] = new Vector3(81f,2.2f,-19f);//before open 1st door
        vector3[10] = new Vector3(86.39f, 1.32f, 1.8f);
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetInteger("Status") == 1)
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
            //target.transform.position = new Vector3(x, y, z);
            if (index < 11)
            {
                if (index == 7) {
                    StartCoroutine(NpcAnimationChange());
                }

                target.transform.position = vector3[index++];
            }
            else
                index = 0;
        }
    }

    IEnumerator NpcAnimationChange()
    {
        animator.SetInteger("Status", 2);
        yield return new WaitForSeconds(11);
        animator.SetInteger("Status", 1);
    }
}
