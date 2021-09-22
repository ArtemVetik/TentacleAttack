using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenGlass : MonoBehaviour
{
    [SerializeField] private GameObject _normalWindow;
    [SerializeField] private GameObject _brokenWindow;
    [SerializeField] private GameObject[] _glasses;
    [SerializeField] private float _brokenGlassLifeTime;

    private Animator _selfAnimator;
    private readonly string Destroy = nameof(Destroy);

    private void Start()
    {
        _selfAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out TargetMovement tentacle))
        {
            _normalWindow.gameObject.SetActive(false);
            _brokenWindow.gameObject.SetActive(true);

            StartCoroutine(DestroyGlass());
        }
    }

    public void ClearCollisions()
    {
        foreach(var glass in _glasses)
        {
            glass.GetComponent<Collider>().enabled = false;
        }
    }

    public void ClearObjects()
    {
        foreach (var glass in _glasses)
        {
            glass.SetActive(false);
        }
    }


    private IEnumerator DestroyGlass()
    {
        yield return new WaitForSeconds(_brokenGlassLifeTime);
        Debug.Log("Destroy glass");
        _selfAnimator.SetTrigger(Destroy);
    }
}
