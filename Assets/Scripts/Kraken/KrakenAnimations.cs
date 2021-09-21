using System.Collections;
using UnityEngine;

public class KrakenAnimations : MonoBehaviour
{
    [SerializeField] private Transform _clothContainer;
    [SerializeField] private KrakenCloth _cloth;

    private Animator _selfAnimator;
    private EatingArea _eatingArea;
    private KrakenCloth _instCloth;
    private const string Eating = nameof(Eating);
    private const string Dancing = nameof(Dancing);

    private void Awake()
    {
        _selfAnimator = GetComponent<Animator>();
        _eatingArea = GetComponentInChildren<EatingArea>();
    }

    private void Start()
    {
        if (_cloth)
        {
            _instCloth = Instantiate(_cloth, _clothContainer);
            _instCloth.transform.localPosition = Vector3.zero;
            _instCloth.transform.localRotation = Quaternion.identity;
        }
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
        _instCloth?.Animator.SetTrigger(Eating);
    }

    private void PlayDancing(bool isWork, int progress)
    {
        if (isWork)
        {
            _selfAnimator.SetTrigger(Dancing);
            _instCloth?.Animator.SetTrigger(Dancing);
        }
    }

}