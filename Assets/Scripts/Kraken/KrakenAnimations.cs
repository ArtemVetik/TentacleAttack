using System.Collections;
using UnityEngine;

public class KrakenAnimations : MonoBehaviour
{
    [SerializeField] private KrakenAccessoryInitializator _accessoryInitializator;

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
        GlobalEventStorage.GameEnded += PlayDancing;
    }

    private void OnDisable()
    {
        _eatingArea.Eating -= OnEating;
        GlobalEventStorage.GameEnded -= PlayDancing;
    }

    private void OnEating()
    {
        _selfAnimator.SetTrigger(Eating);
        _accessoryInitializator.InstAccessory?.Animator.SetTrigger(Eating);
    }

    private void PlayDancing(bool isWork, int progress)
    {
        if (isWork)
        {
            _selfAnimator.SetTrigger(Dancing);
            _accessoryInitializator.InstAccessory?.Animator.SetTrigger(Dancing);
        }
    }

}