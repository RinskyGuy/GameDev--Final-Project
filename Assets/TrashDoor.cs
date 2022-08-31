using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrashDoor : MonoBehaviour
{
    private Animator animator;
    public GameObject crosshair;
    public GameObject camera;
    public Text txt;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit))
        {
            if(hit.transform.gameObject != null && hit.distance < 7 && this.transform.gameObject == hit.transform.gameObject){
                txt.text = (animator.GetBool("Open")) ? "Press [E] To Close" : "Press [E] To Open";
                if (Input.GetKeyDown(KeyCode.E))
                {
                    animator.SetBool("Open", !animator.GetBool("Open"));
                }
            }
            if(!(hit.transform.gameObject.tag == "Trash"))
            {
                txt.text = "";
            }
        }

    }
}
