using System.Collections;
using UnityEngine;

public class EatingArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TriggerEnter");

        if(other.TryGetComponent(out Enemy enemy))
        {
            Destroy(enemy.gameObject);
        }
    }

}
