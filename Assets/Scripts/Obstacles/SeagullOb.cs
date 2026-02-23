using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeagullOb : Obstacle
{
    ///public float fallSpeed = 2f;
    public float spawnWait = 20f;
    public GameObject obsPrefab;

    //constructors
    public SeagullOb(float scrollSpeed):base("Seagull", scrollSpeed) {}
    public SeagullOb():base("Seagull") {}

    // audio
    [Header("Audio")]
    [SerializeField] private AudioClip[] audioClips;

    private AudioSource audioSource;


    //Start is called before the first frame update
    void Start(){
        //set starting state
        //seagull starts and stays in spawning state
        //activeState = new SpawningState(spawnWait, obsPrefab);
        activeState = new SineWaveState();
        ///activeState = new StationaryState(); ///stationary if we don't want it spawning

        activeState.onEnterState();

        //audio setup
        audioSource = GetComponent<AudioSource>();
        PlayRandomSound();
    }
    void PlayRandomSound()
    {
        //play a random seagull sound
        if (audioClips == null || audioClips.Length == 0)
        {
            return;
        }

        int index = Random.Range(0, audioClips.Length);
        audioSource.PlayOneShot(audioClips[index]);
    }
}
