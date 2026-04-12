using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileOb : Obstacle
{
    [Header("Missile Parameters")]
    public float warningTimer;

    //constructors
    public MissileOb(float scrollSpeed):base("Missile", scrollSpeed) {}
    public MissileOb():base("Missile") {}

    //audio
    [Header("Audio")]
    [SerializeField] private AudioClip missileWhistle;
    [SerializeField] private AudioClip missileBoom;
    [SerializeField] private AudioClip missileWarning;

    private AudioSource audioSource;
    private bool hasStartedWhistle = false;
    private static float lastSpawnTime = -999f;
    [SerializeField] private float groupWindow = 0.15f;
    private bool isGroupLeader = false;
    void OnEnable()
    {
        Debug.Log("Missile spawn pos: " + transform.position);
        Player.Instance.tempPause.AddListener(StopAudio);

        audioSource = GetComponent<AudioSource>();
        hasStartedWhistle = false;
        // if its like a group of missiles only play the sound once
        if (Time.time - lastSpawnTime > groupWindow)
        {
            isGroupLeader = true;
            lastSpawnTime = Time.time;
        }
        else
        {
            isGroupLeader = false;
        }

        activeState = new WarningState(scrollSpeed);
        activeState.onEnterState(this);
        // idk why but i had to manually change where the obstacle spawned
        Vector3 pos = transform.position;
        pos.x = Mathf.Min(pos.x, 8.2f);
        transform.position = pos;
        // playing the missile sound
        audioSource.PlayOneShot(missileWarning, 0.8f);
    }


    void OnDisable()
    {
        Player.Instance.tempPause.RemoveListener(StopAudio);
    }
    public override State getNextState(){
        warningTimer -= Time.deltaTime;

        //when warning's over, go to basic movement
        if (warningTimer <= 0f && !hasStartedWhistle)
        {
            hasStartedWhistle = true;

            if (isGroupLeader)
            {
                audioSource.clip = missileWhistle;
                audioSource.volume = 0.1f;
                audioSource.Play();
            }
            return new StationaryState(scrollSpeed);
        }

        //destroy object if it's out of bounds
        if (transform.position.x <= deathBoundX){
            return new DeathState();
        }

        return activeState;
    }
    public void StopAudio()
    {
        audioSource.Stop();

    }
}
