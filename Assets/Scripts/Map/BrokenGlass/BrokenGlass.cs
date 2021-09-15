using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenGlass : MonoBehaviour
{
    [SerializeField] private GameObject _normalWindow;
    [SerializeField] private GameObject _brokenWindow;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out TargetMovement tentacle))
        {
            _normalWindow.gameObject.SetActive(false);
            _brokenWindow.gameObject.SetActive(true);
        }
    }
}
