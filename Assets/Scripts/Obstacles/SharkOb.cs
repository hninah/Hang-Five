using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkOb : Obstacle
{   
    //shark switches to attack mode within this distance from player
    public float attackDistance = 10f;
    //speed that shark moves towards player while attacking
    public float attackSpeed = 1f;
    [HideInInspector] public Transform player;

    //constructors
    public SharkOb(float scrollSpeed):base("Shark", scrollSpeed) {}
    public SharkOb():base("Shark") {}

    //audio
    [Header("Audio")]
    [SerializeField] private AudioClip sharkSpawn;
    [SerializeField] private AudioClip sharkSplash;
    [SerializeField] private AudioClip sharkBite;

    private AudioSource audioSource;

    //Start is called before the first frame update
    void Start(){
        player = GameObject.Find("Player").GetComponent<Transform>();

        //shark starts in stationary state
        activeState = new StationaryState();
        activeState.onEnterState();

        audioSource = GetComponent<AudioSource>();
        PlaySpawnSound();
    }


    public override State getNextState(){
        
        //if the shark is close to the player, slowly move towards it
        float currDistance = Mathf.Abs( transform.position.x - player.position.x);
        
        /*
        if (activeState.Name != "AttackState"){
            Debug.Log("obs pos = " + transform.position.x + ", player pos = " + player.position.x + ", currDistance = " + currDistance);
        }
        */

        if (currDistance < attackDistance && activeState.Name != "AttackState"){
            return new AttackState(player);
        }

        //default: stay in current state
        return activeState;
    }

    void PlaySpawnSound()
    {
        if (audioSource == null) {
            return;
        };

        // 50/50 chance
        AudioClip clip;
        if (Random.value > 0.5f)
        {
            clip = sharkSpawn;
        }
        else
        {
            clip = sharkSplash;
        }
        audioSource.PlayOneShot(clip);
    }
}
