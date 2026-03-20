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
        Complete    // tutorial completed
    }

    public static TutorialManager Instance;
    // start with target being at the bottom of the wave
    public TutorialStep currentStep = TutorialStep.BottomTarget;

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

        tutorialText.text = "Surf down to the green circle!";
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

            tutorialText.text = "Now surf up to the next circle!";
        }
        else if (currentStep == TutorialStep.TopTarget && hitTarget == topTarget)
        {
            // when the player hits the top target the tutorial is complete
            currentStep = TutorialStep.Complete;
            // both targets should not be visbile anymore
            bottomTarget.SetActive(false);
            topTarget.SetActive(false);

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

        Player.Instance.transform.position = respawnPoint.position;
        Player.Instance.SetTutorialInvincible(true);
        // player is reset to its original state
        Player.Instance.ResetPlayer();
        // camera is reset to original position
        surfCamera.ResetCamera();
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