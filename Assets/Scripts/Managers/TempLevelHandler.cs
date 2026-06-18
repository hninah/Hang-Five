using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempLevelHandler : MonoBehaviour
{
    private static TempLevelHandler _instance;
    public static TempLevelHandler Instance { get { return _instance; } }
    [SerializeField] private bool[] newLevelDesign;
    [SerializeField] private GameObject newLevelManager;
    [SerializeField] private GameObject oldLevelManager;

    // Start is called before the first frame update
    void Start()
    {
        _instance = this;
    }

    public bool newLevelType(int levelNum)
    {
        if (newLevelDesign.Length <= levelNum || levelNum < 0)
        {
            Debug.LogError($"Attempt to check status of invalid level {levelNum}");
            return false;
        }

        return newLevelDesign[levelNum];
    }

    public void selectLevelManager()
    {
        bool beatGame = GameManager.Instance == null ? false : GameManager.Instance.beatBoss;
        bool hasCutsceneManager = CutsceneManager.Instance != null;
        bool validStateTypeLevel = hasCutsceneManager ? newLevelType(CutsceneManager.Instance.getCurrentCutscene()) : PatternStateManager.Instance.Debugging;

        if (validStateTypeLevel || PatternStateManager.Instance.Debugging)
        {
            Debug.Log("PATTERN STATE MANAGER IN USE.");
            newLevelManager.GetComponent<PatternStateManager>().enabled = true;
            oldLevelManager.GetComponent<NewPatternManager>().enabled = false;
            oldLevelManager.SetActive(false);
            newLevelManager.SetActive(true);
            return;
        }

        Debug.Log("WE DECIDED NEW PATTERN SHOULD WORK!!!");
        newLevelManager.GetComponent<PatternStateManager>().enabled = false;
        oldLevelManager.GetComponent<NewPatternManager>().enabled = true;
        newLevelManager.SetActive(false);
        oldLevelManager.SetActive(true);
    }

    public PatternState getStartingPatternState()
    {
        if (CutsceneManager.Instance == null || newLevelType(CutsceneManager.Instance.getCurrentCutscene())) {
            return GameManager.Instance.getStartingState();
        }

        return null;
    }
}
