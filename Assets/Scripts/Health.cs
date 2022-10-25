using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth;
    public Slider healthSlider;
    public Vector3 healthPosition;

    private void Update()
    {
        healthSlider.value = GetComponent<CharMove>().playerHealth;

        healthSlider.transform.position = transform.position + healthPosition;
    }
}
