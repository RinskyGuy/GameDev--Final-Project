using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickGold : MonoBehaviour
{
    public GameObject goldInDrawer;
    public GameObject goldInHand;
    public GameObject thrownGold;
    public GameObject aCamera;
    private AudioSource goldPickUp;
    public Text txt;
    private bool pickedUp;
    // Start is called before the first frame update
    void Start()
    {
        pickedUp = false;
        goldPickUp = aCamera.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(aCamera.transform.position,aCamera.transform.forward,out hit))
        {
            if (goldInHand.gameObject.activeSelf)
            {
                txt.text = "Press (P) To Throw";
                txt.gameObject.SetActive(true);
                if (Input.GetKeyDown(KeyCode.P) && hit.distance < 15){
                    goldPickUp.Play();
                    thrownGold.transform.position = hit.point;
                    goldInHand.SetActive(false);
                    thrownGold.SetActive(true);
                }
            }
            else
            {
                if (!pickedUp)
                {
                    if (hit.transform.gameObject == goldInDrawer.gameObject)
                    {
                        txt.text = "Press (P) To Pick Up";
                        txt.gameObject.SetActive(true);
                        if (Input.GetKeyDown(KeyCode.P))
                        {
                            goldPickUp.Play();
                            pickedUp = true;
                            goldInDrawer.SetActive(false);
                            goldInHand.SetActive(true);
                        }
                    }
                    else
                    {
                        txt.gameObject.SetActive(false);
                    }
                }
                else
                {
                    if (hit.transform.gameObject == thrownGold.gameObject)
                    {
                        txt.text = "Press (P) To Pick Up";
                        txt.gameObject.SetActive(true);
                        if (Input.GetKeyDown(KeyCode.P))
                        {
                            goldPickUp.Play();
                            thrownGold.SetActive(false);
                            goldInHand.SetActive(true);
                        }
                    }
                    else
                    {
                        txt.gameObject.SetActive(false);
                    }
                }
            }
        }
        else
        {
            txt.gameObject.SetActive(false);
        }
    }
}
