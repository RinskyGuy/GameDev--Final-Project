using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grenade : MonoBehaviour
{
    private float delay, radius, force;
    public GameObject explosionEffectObject;
    private float countdown;
    private bool hasExploded, routinePlaying;
    private ParticleSystem explosionEffect;
    private List<string> npcs;
    private AudioSource explosionSound;

    // Start is called before the first frame update
    void Start()
    {
        explosionSound = this.gameObject.GetComponent<AudioSource>();
        npcs = new List<string>() {"Player", "Ally", "Enemy1", "Enemy2"};
        force = 700f;
        radius = 7f;
        hasExploded = false;
        routinePlaying = false;
        delay = 3f;
        countdown = delay;
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if(countdown <= 0f && !hasExploded)
        {
            Explode();
            hasExploded = true;
            StartCoroutine(stopExplosionEffect());
            routinePlaying = true;
        }
        if (!routinePlaying && hasExploded)
        {
            Destroy(this.gameObject);
        }
    }

    private void Explode()
    {
        bool eligible;
        float distance;
        //Show Effect
        explosionEffect = Instantiate(explosionEffectObject, transform.position, transform.rotation).GetComponent<ParticleSystem>();
        explosionSound.Play();
        //Get nearby objects - radius 5 - tier 1 hit
        Collider[] collidersTiers = Physics.OverlapSphere(transform.position, 2*radius);
        foreach(Collider nearbyObject in collidersTiers)
        {
            distance = Vector3.Distance(this.gameObject.transform.position, nearbyObject.transform.position);
            eligible = npcs.Contains(nearbyObject.tag);
            if (eligible) {
                if (distance <= radius)
                {
                    GameObject.FindWithTag(nearbyObject.tag + "-hp-bar").GetComponent<Slider>().value -= 40;
                }
                else if(distance > radius && distance<=radius*2)
                {
                    GameObject.FindWithTag(nearbyObject.tag + "-hp-bar").GetComponent<Slider>().value -= 20;
                }
            } 
        };
    }

    IEnumerator stopExplosionEffect()
    {
        float time = explosionEffect.main.duration;
        yield return new WaitForSeconds(time/2);
        explosionEffect.Stop();
        routinePlaying = false;
    }

//    void OnDrawGizmos()
//    {
//        Color color1 = Color.red;
//        color1[3] = 0.2f;
//        // Display the explosion radius when selected
//        Gizmos.color = color1;
//        Gizmos.DrawSphere(transform.position, 16);
//        color1 = Color.blue;
//        color1[3] = 0.2f;
//        Gizmos.color = color1;
//        Gizmos.DrawSphere(transform.position, 8);
//    }

}
