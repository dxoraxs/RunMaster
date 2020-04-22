using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanePainter : MonoBehaviour
{
    [SerializeField] private GameObject prefabBrush;
    private List<GameObject> drawing = new List<GameObject>();

    private void OnEnable()
    {
        EventManager.OnRestartLevel += OnLevelRestart;
    }

    private void OnDisable()
    {
        EventManager.OnRestartLevel -= OnLevelRestart;
    }

    private void OnLevelRestart()
    {
        foreach (GameObject smear in drawing)
        {
            Destroy(smear);
        }
        drawing.Clear();
    }

    public void OnBrushSpawn(Vector3 position)
    {
        var smear = Instantiate(prefabBrush, transform);
        smear.transform.position = position + Vector3.up* 0.001f;
        drawing.Add(smear);
    }
}
