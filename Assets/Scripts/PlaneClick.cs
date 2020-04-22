using System.Collections.Generic;
using UnityEngine;

public class PlaneClick : MonoBehaviour
{
    [SerializeField] private float sizeBrush = 0.16f;
    [SerializeField] private Transform man;
    [Range(0, 1)] [SerializeField] private float rangeManStartDraw = 0.5f;
    private PlanePainter planePainter;
    private Vector3 lastPosition;
    private bool isDraw;
    private bool isDrawBesideMan;

    private List<Vector3> pointMovement = new List<Vector3>();

    private void Start()
    {
        planePainter = GetComponent<PlanePainter>();
    }

    private void OnEnable()
    {
        EventManager.OnRestartLevel += OnLevelRestart;
    }

    private void OnDestroy()
    {
        EventManager.OnRestartLevel -= OnLevelRestart;
    }

    private void OnLevelRestart()
    {
        pointMovement.Clear();
        isDrawBesideMan = false;
    }

    private void OnMouseDown()
    {
        Vector3 position;

        if (!isDraw || !GetPositionPointDown(out position))
        {
            return;
        }

        if ((position - man.position).magnitude < rangeManStartDraw)
        {
            isDrawBesideMan = true;
        }

        lastPosition = position;
    }
    private void OnMouseDrag()
    {
        Vector3 position = new Vector3();

        if (!isDraw || !GetPositionPointDown(out position) || !isDrawBesideMan)
        {
            return;
        }

        Vector3 direction = position - lastPosition;
        var distance = direction.magnitude;
        if (distance > sizeBrush)
        {
            int count = (int)(distance / sizeBrush);
            for (int i = 0; i < count; i++)
            {
                lastPosition += direction.normalized * sizeBrush;
                planePainter.OnBrushSpawn(lastPosition);
            }
        }
        planePainter.OnBrushSpawn(position);
        pointMovement.Add(lastPosition);
        lastPosition = position;
    }

    private void OnMouseEnter()
    {
        isDraw = true;
    }

    private void OnMouseExit()
    {
        StopDraw();
    }

    private void OnMouseUp()
    {
        StopDraw();
    }

    private void StopDraw()
    {
        if (!isDrawBesideMan)
        {
            return;
        }

        isDraw = false;
        EventManager.OnStartMovementMan?.Invoke(pointMovement);
    }

    private bool GetPositionPointDown(out Vector3 point)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            point = hit.point;
            return true;
        }
        else
        {
            point = new Vector3();
            return false;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(man.position, rangeManStartDraw);
    }
}
