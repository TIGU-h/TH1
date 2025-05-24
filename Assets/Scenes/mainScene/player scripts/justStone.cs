using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class justStone : MonoBehaviour
{
    [SerializeField] private float speed;
    private void Start()
    {
        Destroy(gameObject, 2);
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
