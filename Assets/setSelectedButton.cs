using UnityEngine;
using UnityEngine.EventSystems;

public class SetSelectedButton : MonoBehaviour
{
    public GameObject defaultButton;

    void Start()
    {
        EventSystem.current.SetSelectedGameObject(defaultButton);
    }
}