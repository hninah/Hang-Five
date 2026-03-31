using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public enum TutorialStep
    {
        // three stages in the tutorial
        BottomTarget,   // hit the target on the bottom of the wave
        TopTarget,  // hit the target on the top of the wave
        FinalTarget,    // hit the target at the bottom of the wave again
        Complete    // tutorial completed
    }

    public static TutorialManager Instance;
    // start with target being at the bottom of the wave
    public TutorialStep currentStep = TutorialStep.BottomTarget;

    private Vector3 checkpointPosition; // where the player got the target

    [Header("Targets")]
    [SerializeField] private GameObject bottomTarget;
    [SerializeField] private GameObject topTarget;

    [Header("UI")]
    public TMPro.TextMeshProUGUI tutorialText;
    [SerializeField] private GameObject skipTutorialButton;

    [Header("Player Setup")]
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private SurfCamera surfCamera;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        tutorialText.enabled = true;

        // Start with bottom target
        currentStep = TutorialStep.BottomTarget;
        // bottom target should show up and top target should not be active
        bottomTarget.SetActive(true);
        topTarget.SetActive(false);

        tutorialText.text = "Using SPACE surf down to the green circle!";
    }

    public void PlayerHitTarget(GameObject hitTarget)
    {
        // when the player hits the bottom target then turn on the top target
        if (currentStep == TutorialStep.BottomTarget && hitTarget == bottomTarget)
        {
            currentStep = TutorialStep.TopTarget;

            // switch targets
            bottomTarget.SetActive(false);
            topTarget.SetActive(true);

            tutorialText.text = "Release SPACE to surf up to the next circle!";
        }
        // when the player hits top target turn the bottom target back on
        else if (currentStep == TutorialStep.TopTarget && hitTarget == topTarget)
        {
            currentStep = TutorialStep.FinalTarget;

            // save the positon where the player got the top target
            checkpointPosition = Player.Instance.transform.position;

            // disable the top target and activate the bottom target again
            bottomTarget.SetActive(true);
            topTarget.SetActive(false);

            tutorialText.text = "Now surf back down to the bottom!";
        }
        // when the player hits the bottom target load the gameplay scene
        else if (currentStep == TutorialStep.FinalTarget && hitTarget == bottomTarget)
        {
            currentStep = TutorialStep.Complete;

            bottomTarget.SetActive(false);

            tutorialText.text = "Nice! You're ready to surf!";

            StartCoroutine(LoadGameplayAfterDelay());
        }
    }

    IEnumerator LoadGameplayAfterDelay()
    {
        // freeze the game for like 2 seconds so the player can read the text
        FreezeGame(2f);
        yield return new WaitForSecondsRealtime(2f);
        // switch to gameplay scene
        GameManager.Instance.tutorialCompleted = true;
        SceneManager.LoadScene("Gameplay");
    }

    public void PlayerCrashed()
    {
        StartCoroutine(RespawnPlayer());
    }

    IEnumerator RespawnPlayer()
    {
        // the player disappears for like 0.2 seconds and then respawns where the respawn point is
        Player.Instance.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        // if the player is respawning after the top target, then respawn them back at the top target
        if (currentStep == TutorialStep.FinalTarget)
        {
            Player.Instance.transform.position = checkpointPosition;
        }
        // if the player is respawning before the top target, just respawn them in their original position
        else
        {
            Player.Instance.transform.position = respawnPoint.position;
        }

        Player.Instance.ResetPlayer();
        surfCamera.ResetCamera();
        Player.Instance.gameObject.SetActive(true);
    }

    void FreezeGame(float duration)
    {
        // freeze the game for however many seconds
        Time.timeScale = 0f;
        StartCoroutine(UnfreezeAfter(duration));
    }

    IEnumerator UnfreezeAfter(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
    }
    public void SkipTutorial()
    {
        // players are allowed to skip the tutorial (i put this in because i got too lazy to keep doing the tutorial)
        currentStep = TutorialStep.Complete;

        GameManager.Instance.tutorialCompleted = true;
        SceneManager.LoadScene("Gameplay");
    }
}