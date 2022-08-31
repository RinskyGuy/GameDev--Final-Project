using System.Collections; 
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickGrenade : MonoBehaviour
{
    private List<string> usertags = new List<string>() { "Player", "Ally", "Enemy1", "Enemy2"};
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
        //Debug.Log(other);
        eligible = usertags.Contains(other.tag);
        if (eligible)
        {
            StartCoroutine(disappear());
            this.gameObject.GetComponentInChildren<Collider>().isTrigger = false;
            Spawning.updateNumOfGrenades();
            switch (other.tag)
            {
                case "Player":
                    GameObject.FindWithTag("GrenadeNumber").GetComponent<Text>().text = "" + (System.Convert.ToInt32(GameObject.FindWithTag("GrenadeNumber").GetComponent<Text>().text) + 1);
                    break;
                case "Ally":
                    Ally.numOfGrenades++;
                    break;
                case "Enemy1":
                    Enemy1.numOfGrenades++;
                    break;
                case "Enemy2":
                    Enemy2.numOfGrenades++;
                    break;
            }
           
        }
    }

    IEnumerator disappear()
    {
        float time = this.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length +
            this.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime;
        yield return new WaitForSeconds(time / 100);
        this.gameObject.SetActive(false);
    }
}
