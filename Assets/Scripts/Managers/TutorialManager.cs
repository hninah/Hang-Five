using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public enum TutorialStep
    {
        WaveMovement,
        AvoidObstacles,
        Complete
    }

    public static TutorialManager Instance;

    public TutorialStep currentStep = TutorialStep.WaveMovement;

    [SerializeField] private GameObject obstacleSpawner;
    public TMPro.TextMeshProUGUI tutorialText;
    [SerializeField] private LaneSpawner laneSpawner;

    private int obstaclesDodged = 0;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private SurfCamera surfCamera;
    [SerializeField] private GameObject tutorialTarget;

    void Awake()   
    {
        Instance = this;
    }

    void Start()
    {
        tutorialText.enabled = true;
        tutorialText.text = "Surf to the circle by holding SPACE!";
        tutorialTarget.SetActive(true);
    }
    public void PlayerHitTarget()
    {
        if (currentStep != TutorialStep.WaveMovement) return;

        StartObstacleTutorial();
    }

    void StartObstacleTutorial()
    {
        currentStep = TutorialStep.AvoidObstacles;

        FreezeGame(2f);

        laneSpawner.EnableTutorialMode();
        obstacleSpawner.SetActive(true);

        obstaclesDodged = 0;
        tutorialText.text = "Avoid the wrecking balls!";
    }
    public void ObstacleDodged()
    {
        // if player is not currently surfing then dont count the obstacles as dodged
        if (Player.Instance.State != Player.PlayerState.SURFING) return;
        obstaclesDodged++;

        if (obstaclesDodged >= 3)
        {
            TutorialComplete();
        }
    }

    public void TutorialComplete()
    {
        currentStep = TutorialStep.Complete;
        // deactivate the spawner
        obstacleSpawner.SetActive(false);
        laneSpawner.DisableTutorialMode();
            
        tutorialText.text = "Great dodging! Now get surfing!";

        StartCoroutine(LoadGameplayAfterDelay());
    }
    IEnumerator LoadGameplayAfterDelay()
    {
        FreezeGame(3f); // freeze gameplay while text shows

        yield return new WaitForSecondsRealtime(3f);
        // transition to the gameplay scene
        GameManager.Instance.tutorialCompleted = true;
        SceneManager.LoadScene("Gameplay");
    }

    public void PlayerCrashed()
    {
        StartCoroutine(RespawnPlayer());
    }
    IEnumerator RespawnPlayer()
    {
        // hide player briefly
        Player.Instance.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.2f);
        // put player back where it started
        Player.Instance.transform.position = respawnPoint.position;
        // turn on invincibility frames
        Player.Instance.SetTutorialInvincible(true);
        // reset player position and animation
        Player.Instance.ResetPlayer();
        // reset camera position
        surfCamera.ResetCamera();
    }

    public bool IsTutorialObstaclePhase()
    {
        return currentStep == TutorialStep.AvoidObstacles;
    }
    void FreezeGame(float duration)
    {
        Time.timeScale = 0f;
        StartCoroutine(UnfreezeAfter(duration));
    }

    IEnumerator UnfreezeAfter(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
    }

}
