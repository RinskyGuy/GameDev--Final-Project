using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMotion : MonoBehaviour
{
    private Animator animator;
    private AudioSource doorSqueeck;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        doorSqueeck = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "NPC")
        {
            animator.SetBool("DoorIsOpening", true);
            if (!doorSqueeck.isPlaying)
                doorSqueeck.PlayDelayed(0.5f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "NPC")
        {
            animator.SetBool("DoorIsOpening", false);
            if (!doorSqueeck.isPlaying)
                doorSqueeck.PlayDelayed(0.5f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
