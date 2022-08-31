using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Spawning : MonoBehaviour
{
    // Start is called before the first frame update
    private static int numOfCurrentHPBoxes, numOfCurrentGrenades;
    private int maxNumBoxes, maxNumGrenades;
    private int distance;
    private bool hpBoxRoutine, grenadeRoutine;
    public GameObject center, itemObject, grenadeObject;

    void Start()
    {
        grenadeRoutine = false;
        hpBoxRoutine = false;
        numOfCurrentHPBoxes = 0;
        numOfCurrentGrenades = 0;
        maxNumGrenades = 5;
        maxNumBoxes = 5;
        distance = 150;
    }

    // Update is called once per frame
    void Update()
    {
        if ((numOfCurrentGrenades < maxNumGrenades) && !grenadeRoutine)
        {
            grenadeRoutine = true;
            StartCoroutine(spawnItem(false, grenadeObject));
        }
        if ((numOfCurrentHPBoxes < maxNumBoxes) && !hpBoxRoutine)
        {
            hpBoxRoutine = true;
            StartCoroutine(spawnItem(true, itemObject));
        }
    }

    IEnumerator spawnItem(bool isBox, GameObject itemPrefab)
    {
        yield return new WaitForSeconds(0.3f);
        float directionFacing = Random.Range(0f, 360f);
        itemPrefab.gameObject.SetActive(true);
        GameObject aItem = Instantiate(itemPrefab, getRandomDestination(center), Quaternion.Euler(new Vector3(0f, directionFacing, 0f)));
        aItem.gameObject.SetActive(true);
        if (isBox)
        {
            numOfCurrentHPBoxes++;
            hpBoxRoutine = false;
        }
        else
        {
            aItem.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            numOfCurrentGrenades++;
            grenadeRoutine = false;
        }
    }

    Vector3 getRandomDestination(GameObject spawnLocation)
    {
        // Get Random Point inside Sphere which position is center, radius is maxDistance
        Vector3 randomPos = Random.insideUnitSphere * distance + spawnLocation.transform.position;
        NavMeshHit hit; // NavMesh Sampling Info Container
        // from randomPos find a nearest point on NavMesh surface in range of maxDistance
        NavMesh.SamplePosition(randomPos, out hit, distance, NavMesh.AllAreas);
        Vector3 vector = new Vector3(hit.position.x, hit.position.y, hit.position.z);
        return vector; // or return hit.position where y is random
    }

    public static void updateNumOfBoxes(){
        //theCollider.SetActive(false);
        numOfCurrentHPBoxes--;
    }

    public static void updateNumOfGrenades()
    {
        numOfCurrentGrenades--;
    }
}
