using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManController : MonoBehaviour
{
    [SerializeField] private float speed;
    private bool isMove;
    private Vector3[] wayMovement;
    private int countPoint = 0;

    private Rigidbody rigidbody;
    private Animator animator;

    private List<Collider> collisions = new List<Collider>();
    private bool isGrounded;
    private Vector3 currentDirection = Vector3.zero;

    private Vector3 startPosition;
    private Quaternion startRotation;

    private void OnEnable()
    {
        EventManager.OnStartMovementMan += StartMovement;
        EventManager.OnRestartLevel += OnLevelRestart;
    }

    private void OnDisable()
    {
        EventManager.OnStartMovementMan -= StartMovement;
        EventManager.OnRestartLevel -= OnLevelRestart;
    }

    private void OnLevelRestart()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;
        isMove = false;
        wayMovement = null;
        countPoint = 0;
    }

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                if (!collisions.Contains(collision.collider))
                {
                    collisions.Add(collision.collider);
                }
                isGrounded = true;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        bool validSurfaceNormal = false;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                validSurfaceNormal = true; break;
            }
        }

        if (validSurfaceNormal)
        {
            isGrounded = true;
            if (!collisions.Contains(collision.collider))
            {
                collisions.Add(collision.collider);
            }
        }
        else
        {
            if (collisions.Contains(collision.collider))
            {
                collisions.Remove(collision.collider);
            }
            if (collisions.Count == 0) { isGrounded = false; }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collisions.Contains(collision.collider))
        {
            collisions.Remove(collision.collider);
        }
        if (collisions.Count == 0) { isGrounded = false; }
    }

    private void Update()
    {
        animator.SetBool("Grounded", isGrounded);
        animator.SetFloat("MoveSpeed", rigidbody.velocity.normalized.magnitude);
        if (isMove)
        {
            var direction = (wayMovement[countPoint] - transform.position);
            currentDirection = Vector3.Slerp(currentDirection, direction, Time.deltaTime * 10);
            rigidbody.velocity = direction.normalized * speed;

            transform.rotation = Quaternion.LookRotation(currentDirection);

            if (direction.magnitude < 0.05f)
            {
                countPoint++;
            }

            if (countPoint >= wayMovement.Length - 1)
            {
                isMove = false;
                EventManager.OnManEndPointMovement?.Invoke();
            }
        }
    }

    private void StartMovement(List<Vector3> points)
    {
        isMove = true;
        wayMovement = points.ToArray();
    }

    private void OnDrawGizmos()
    {
        if (isMove)
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < wayMovement.Length; i++)
            {
                Gizmos.DrawWireSphere(wayMovement[i], 0.1f);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GameConstants.TagFinish))
        {
            isMove = false;
            animator.SetTrigger("The End");
            EventManager.OnManFinish?.Invoke();
        }
        else if (other.CompareTag(GameConstants.TagCoin))
        {
            StartCoroutine(StopTakeCoin(other.transform));
        }
    }

    private IEnumerator StopTakeCoin(Transform coin)
    {
        isMove = false;
        var positionLook = coin.position;
        positionLook.y = transform.position.y;
        transform.LookAt(positionLook);
        animator.SetTrigger("Pickup");
        yield return new WaitForSeconds(1);
        coin.gameObject.SetActive(false);
        EventManager.OnCoinTake?.Invoke();
        yield return new WaitForSeconds(1);
        isMove = true;
    }
}
