using UnityEngine;
using System.Collections;

public class WalkSoundController : MonoBehaviour {

    private AudioSource audioSource;
    private MainController mainController;
    public AudioClip walk;
    public AudioClip run;
 
    bool isWalking = false;
    bool isRunning = false;
 
 
    void Start()
    {
        mainController = GameObject.Find("RigidBodyFPSController").GetComponent<MainController>();

        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.clip = walk;
        audioSource.clip = run;
    }

     void Update()
     {
        if (mainController.getPlayerControlableFlg() != true)
        {
            isWalking = false;
            isRunning = false;
            PlayAudio();
        }
        else
        {
            GetState();
            PlayAudio();
        }
    }


    void GetState()
    {

        if (Input.GetAxis("Horizontal")!=0 || Input.GetAxis("Vertical")!=0)
        {
            if (Input.GetKey("left shift") || Input.GetKey("right shift"))
            {
                // Running
                isWalking = false;
                isRunning = true;
            }
            else
            {
                // Walking
                isWalking = true;
                isRunning = false;
            }
        }
        else
        {
            // Stopped
            isWalking = false;
            isRunning = false;
        }
    }


    void PlayAudio()
    {
        if (isWalking)
        {
            if (audioSource.clip != walk)
            {
                audioSource.Stop();
                audioSource.clip = walk;
            }

            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else if (isRunning)
        {
            if (audioSource.clip != run)
            {
                audioSource.Stop();
                audioSource.clip = run;
            }

            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }
}
