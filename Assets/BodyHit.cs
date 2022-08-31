using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BodyHit : MonoBehaviour
{
    public Slider HPBar;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            switch (this.transform.parent.gameObject.name)
            {
                case "Player":
                case "Ally":
                    Ally.beingAttacked();
                    break;
                case "Enemy1":
                    Enemy1.beingAttacked();
                    break;
                case "Enemy2":
                    Enemy2.beingAttacked();
                    break;
            }
            HPBar.value -= 5;
            //other.gameObject.SetActive(false);
        }
    }
}
