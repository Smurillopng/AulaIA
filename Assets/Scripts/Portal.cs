using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private GameObject portal1;
    [SerializeField] private GameObject portal2;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameObject.CompareTag("Portal1"))
            {
                other.transform.position = portal2.transform.position;
            }
            else if (gameObject.CompareTag("Portal2"))
            {
                other.transform.position = portal1.transform.position;
            }
        }
    }
}
