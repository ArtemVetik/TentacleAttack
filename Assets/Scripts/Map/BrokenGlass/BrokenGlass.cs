using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BrokenGlass : MonoBehaviour
{
    [SerializeField] private GameObject _normalWindow;
    [SerializeField] private GameObject _brokenWindow;
    [SerializeField] private GameObject[] _glasses;
    [SerializeField] private float _brokenGlassLifeTime;

    private AudioSource _audio;
    private Animator _selfAnimator;
    private readonly string Destroy = nameof(Destroy);
    private bool _isBroken = false;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
        _selfAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isBroken == false && other.TryGetComponent(out TargetMovement tentacle))
        {
            _normalWindow.gameObject.SetActive(false);
            _brokenWindow.gameObject.SetActive(true);

            if (SaveDataBase.GetSoundSetting() == true)
            {
                _audio.Play();
            }

            StartCoroutine(DestroyGlass());

            _isBroken = true;
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
        _selfAnimator.SetTrigger(Destroy);
    }
}
