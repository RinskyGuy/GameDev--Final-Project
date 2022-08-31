using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JukeboxMusic : MonoBehaviour
{
    public GameObject RegReticle;
    public GameObject camera;
    public Text Txt;
    private AudioSource JukeboxAudio;
    // Start is called before the first frame update
    void Start()
    {
        JukeboxAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit) && hit.transform.gameObject == this.gameObject && hit.distance < 7)
        {
            SeeThroughChanges(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!JukeboxAudio.isPlaying)
                    JukeboxAudio.Play();
                else
                    JukeboxAudio.Stop();
            }
        }
        else
        {
            SeeThroughChanges(false);
        }

    }

    private void SeeThroughChanges(bool Touchable)
    {
        if (Touchable)
        {
            Txt.text = (JukeboxAudio.isPlaying)? "Press (E) To Stop" : "Press (E) To Play";
        }
        Txt.gameObject.SetActive(Touchable);
    }
}
