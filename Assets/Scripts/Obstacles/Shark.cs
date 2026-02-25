using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shark : Obstacle
{

    public Shark(float scrollSpeed) : base("Shark", scrollSpeed) { }
    public Shark() : base("Shark") { }

    public float aggroThreshold = 100.0f;
    //audio
    [Header("Audio")]
    [SerializeField] private AudioClip sharkSpawn;
    [SerializeField] private AudioClip sharkSplash;
    [SerializeField] private AudioClip sharkBite;

    private AudioSource audioSource;
    private bool bitePlayed = false;
    // Start is called before the first frame update
    void Start()
    {
        activeState = new SharkPassiveState();
        activeState.onEnterState();
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(sharkSpawn, 0.3f);
    }

    public override void obstacleSpecialties()
    {
        if (Mathf.Abs(transform.position.x - Player.Instance.transform.position.x) < aggroThreshold)
        {
            animator.SetBool("isAttacking", true);
            if (bitePlayed == false)
            {
                StartCoroutine(PlayBiteWithDelay(0.5f));
                bitePlayed = true;
            }
        }
    }
    IEnumerator PlayBiteWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        audioSource.PlayOneShot(sharkSplash, 0.3f);
    }
}
