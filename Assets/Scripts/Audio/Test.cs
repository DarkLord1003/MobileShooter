using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public AudioClip AudioClip;
    // Start is called before the first frame update
    void Start()
    {
        if (AudioManager.Instance)
        {
            AudioManager.Instance.SetTrackVolume("Player", 10, 5);
        }

        InvokeRepeating("PlayTest", 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlayTest()
    {
        AudioManager.Instance.PlayOneShotSound("Player", AudioClip, transform.position, 0.5f, 0.0f, 128);
    }
}
