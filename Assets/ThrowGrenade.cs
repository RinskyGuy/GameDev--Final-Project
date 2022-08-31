using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowGrenade : MonoBehaviour
{
    public static float throwForce;
    public static GameObject grenadePrefab;

    // Start is called before the first frame update

    public static void throwGrenade(GameObject thrower)
    {
        GameObject grenade = Instantiate(grenadePrefab, thrower.transform.position, thrower.transform.rotation);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        rb.AddForce(thrower.transform.forward * throwForce, ForceMode.VelocityChange);
    }
}
