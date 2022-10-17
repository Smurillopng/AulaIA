using UnityEngine;

public class Spawns : MonoBehaviour
{
    [SerializeField] private GameObject slime1;
    [SerializeField] private GameObject slime2;
    [SerializeField] private Transform spawnPoint1;
    [SerializeField] private Transform spawnPoint2;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Instantiate(slime1, spawnPoint1.position, Quaternion.identity);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            Instantiate(slime2, spawnPoint2.position, Quaternion.identity);
        }
    }
}