using System.Collections;
using UnityEngine;

public class KrakenAnimations : MonoBehaviour
{
    private Animator _selfAnimator;
    private EatingArea _eatingArea;
    private const string Eating = "Octopus_Rig|Octopus_Eating_1";

    private void OnDisable()
    {
        _eatingArea.Eating -= OnEating;
    }

    void Start()
    {
        _selfAnimator = GetComponent<Animator>();
        _eatingArea = GetComponentInChildren<EatingArea>();
        _eatingArea.Eating += OnEating;
    }

    private void OnEating()
    {
        _selfAnimator.Play(Eating);
    }

}