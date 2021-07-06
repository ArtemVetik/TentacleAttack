using System.Collections;
using UnityEngine;

public class KrakenAnimations : MonoBehaviour
{
    private Animator _selfAnimator;
    private EatingArea _eatingArea;
    private const string Eating = nameof(Eating);
    private const string Dancing = nameof(Dancing);

    private void Awake()
    {
        _selfAnimator = GetComponent<Animator>();
        _eatingArea = GetComponentInChildren<EatingArea>();
    }

    private void OnEnable()
    {
        _eatingArea.Eating += OnEating;
    }

    private void OnDisable()
    {
        _eatingArea.Eating -= OnEating;
    }

    private void OnEating()
    {
        _selfAnimator.SetTrigger(Eating);
    }

    public void PlayDancing()
    {
        _selfAnimator.SetTrigger(Dancing);
    }
}