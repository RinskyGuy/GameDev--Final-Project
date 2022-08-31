using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadShot : MonoBehaviour
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
        if(other.tag == "Bullet")
        {
            switch (this.transform.parent.gameObject.name)
            {
                case "Player":
                case "Ally":
                    Debug.Log("Ally attacked");
                    Ally.beingAttacked();
                    break;
                case "Enemy1":
                    Debug.Log("enemy1 attacked");
                    Enemy1.beingAttacked();
                    break;
                case "Enemy2":
                    Debug.Log("enemy2 attacked");
                    Enemy2.beingAttacked();
                    break;
            }
            HPBar.value -= 15;
         //   other.gameObject.SetActive(false);
        }
    }
}
