using UnityEngine.UI;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth;
    public Slider healthSlider;
    public Vector3 posicaoDaVida;

    private void Update()
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = GetComponent<CharMove>().playerHealth;

        healthSlider.transform.position = transform.position + posicaoDaVida;
    }
}
