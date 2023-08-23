using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Looker : MonoBehaviour
{
    [SerializeField] private Transform _target;

    void Update()
    {
        transform.LookAt(_target);
    }
}
