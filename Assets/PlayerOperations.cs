using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOperations : MonoBehaviour
{
    public GameObject camera;
    public GameObject bullet;
    public Text winOrLoseText;
    public GameObject regularUI;
    public GameObject gameOverUI;
    public GameObject grenadePrefab;
    public GameObject gun;
    private ParticleSystem muzzleFlash;
    private AudioSource muzzleSound;
    public float throwForce;
    public static bool inGameOverUI;
    // Start is called before the first frame update
    void Start()
    {
        muzzleFlash = gun.GetComponent<ParticleSystem>();
        muzzleSound = gun.GetComponent<AudioSource>();
        inGameOverUI = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            /*StartCoroutine(*/shoot()/*)*/;
        }
        if (Input.GetKeyDown(KeyCode.Q) && System.Convert.ToInt32(GameObject.FindWithTag("GrenadeNumber").GetComponent<Text>().text) > 0)
        {
            ThrowGrenade.grenadePrefab = grenadePrefab;
            ThrowGrenade.throwForce = throwForce;
            ThrowGrenade.throwGrenade(camera);
            GameObject.FindWithTag("GrenadeNumber").GetComponent<Text>().text = "" + (System.Convert.ToInt32(GameObject.FindWithTag("GrenadeNumber").GetComponent<Text>().text) - 1);
        }
        checkGameStatus();
    }

    /*IEnumerator*/public void shoot()
    {
        RaycastHit hit;
        //StartCoroutine(ShowFlash());
        
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit))
        {
            bullet.SetActive(true);
            bullet.transform.position = hit.point;
            muzzleFlash.Play();
            muzzleSound.Play();
        }
    }

    private void checkGameStatus()
    {
        if(Enemy1.deadEN1 && Enemy2.deadEN2)
        {
            setup(true);
        }
        else if(PlayerMotion.dead)
        {
            setup(false);
        }
    }

    public void setup(bool win)
    {
        inGameOverUI = true;
        gameOverUI.SetActive(true);
        regularUI.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        if (win)
        {
            winOrLoseText.text = "You Win!";
        }
        else
        {
            winOrLoseText.text = "You Lose!";
        }

    }


}
