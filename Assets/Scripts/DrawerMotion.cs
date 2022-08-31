using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawerMotion : MonoBehaviour
{
    private Animator animator;
    public GameObject RegReticle;
    public GameObject camera;
    public Text Txt;
    private AudioSource DrawerAudio;
    public GameObject Gold;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        DrawerAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit) && hit.transform.gameObject == this.gameObject && hit.distance<7)
        {
            SeeThroughChanges(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                animator.SetBool("DrawerOpen", !animator.GetBool("DrawerOpen"));
                if (!DrawerAudio.isPlaying)
                    DrawerAudio.PlayDelayed(0.5f);
            }
        }
        else
        {
            SeeThroughChanges(false);
        }

    }

    private void SeeThroughChanges(bool Touchable)
    {
        if (Touchable)
        {
            Txt.text = (animator.GetBool("DrawerOpen")) ? "Press (E) To Close" : "Press (E) To Open";
        }
        Txt.gameObject.SetActive(Touchable);
        //((Gold.gameObject.activeSelf)? "Press (E) To Close. Press (P) To Pick Gold" : "Press(E) To Close.")
    }
}
