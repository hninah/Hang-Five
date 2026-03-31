using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CreditsButton : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject creditsUI;

    public void ShowCredits()
    {
        mainMenuUI.SetActive(false);
        creditsUI.SetActive(true);
    }

    public void HideCredits()
    {
        mainMenuUI.SetActive(true);
        creditsUI.SetActive(false);
    }
}