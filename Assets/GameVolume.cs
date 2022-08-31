using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameVolume : MonoBehaviour
{
    //private AudioListener audio;
    public GameObject muteIcon;
    public GameObject unMuteIcon;

    // Start is called before the first frame update
    void Start()
    {
        //audio = GetComponent<AudioListener>();
        AudioListener.volume = 1;
        //audio.volume = 0.05f;
        unMuteIcon.SetActive(true);
        muteIcon.SetActive(false);
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.M))
        {
            if(AudioListener.volume == 1f)
            {
                //audio.mute = false;
                AudioListener.volume = 0f;
                muteIcon.SetActive(true);
                unMuteIcon.SetActive(false);
            }
            else
            {
                AudioListener.volume = 1f;
                //audio.mute = true;
                unMuteIcon.SetActive(true);
                muteIcon.SetActive(false);
            }
        }
    }
}
