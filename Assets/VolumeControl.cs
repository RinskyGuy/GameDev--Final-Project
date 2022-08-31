using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public static Slider volume;
    private static AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        audio = GameObject.FindWithTag("music").GetComponent<AudioSource>();
        volume = GetComponentInChildren<Slider>();
        volume.maxValue = 0.3f;
        volume.minValue = 0f;
        volume.value = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        audio.volume = volume.value;
    }

    public static void StopMusic()
    {
        audio.Stop();
    }
}
