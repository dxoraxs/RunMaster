using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinRotate : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;
    [SerializeField] private Vector3 rotateVector = new Vector3(0, 0, 1);

    private void Update() => transform.Rotate(rotateVector * rotateSpeed * Time.deltaTime);
}
