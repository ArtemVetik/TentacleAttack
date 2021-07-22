using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class HealthPresenter : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private TMP_Text _textValue;
    
    private Animator _selfAnimator;

    private void Awake()
    {
        _selfAnimator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _health.Damaged += OnDamaged;
    }

    private void OnDisable()
    {
        _health.Damaged -= OnDamaged;
    }

    private void Start()
    {
        UpdateTextValue();
    }

    private void OnDamaged()
    {
        UpdateTextValue();
        _selfAnimator.SetTrigger("Damaged");
    }

    private void UpdateTextValue()
    {
        _textValue.text = _health.Value.ToString();
    }
}
