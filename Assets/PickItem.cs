using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PickItem : MonoBehaviour
{
    private List<string> usertags = new List<string>() {"Player", "Ally", "Enemy1", "Enemy2"};
    private bool eligible;

    // Start is called before the first frame update
    void Start()
    {
        eligible = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        eligible = usertags.Contains(other.tag);
        if (eligible) {
            this.gameObject.GetComponent<Animator>().SetBool("neer", true);
            StartCoroutine(disappear());
            this.gameObject.GetComponent<Collider>().isTrigger = false;
            GameObject.FindWithTag(other.tag + "-hp-bar").GetComponent<Slider>().value += 15;
        }
    }

    IEnumerator disappear()
    {
        float time = this.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + 
            this.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime;
        yield return new WaitForSeconds(time/6);
        this.gameObject.SetActive(false);
    }


    private void OnTriggerExit(Collider other)
    {
        if (eligible)
        {
            Spawning.updateNumOfBoxes();
        }
    }
}
