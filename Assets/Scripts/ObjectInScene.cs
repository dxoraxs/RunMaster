using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInScene : MonoBehaviour
{
    private void Start()
    {
        Transform plane = transform;
        while (plane.parent != null)
        {
            plane = plane.parent;
        }
        if (plane.TryGetComponent(out BoxCollider boxCollider))
        {
            float rangeXPosition = boxCollider.size.x - transform.GetComponent<BoxCollider>().size.x;
            rangeXPosition /= 2;
            transform.position += Vector3.right * Random.Range(-rangeXPosition, rangeXPosition);
        }

        EventManager.OnRestartLevel += OnLevelRestart;
    }

    private void OnDestroy()
    {
        EventManager.OnRestartLevel -= OnLevelRestart;
    }

    private void OnLevelRestart()
    {
        gameObject.SetActive(true);
    }
}
