using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Respawn : MonoBehaviour
{
    private void Start()
    {
        EventManager.OnManEndPointMovement += OnManEndMovement;
        EventManager.OnRestartLevel += OnLevelRestart;
        gameObject.SetActive(false);
    }

    private void OnLevelRestart()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventManager.OnManEndPointMovement -= OnManEndMovement;
        EventManager.OnRestartLevel -= OnLevelRestart;
    }

    private void OnManEndMovement()
    {
        gameObject.SetActive(true);
    }

    private void OnMouseDown()
    {
        EventManager.OnRestartLevel?.Invoke();
    }
}
