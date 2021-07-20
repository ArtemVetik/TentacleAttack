using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class HealthPresenter : MonoBehaviour
{
    [SerializeField] private Health _health;
    
    private Animator _selfAnimator;
    private List<Image> _images;

    private void Awake()
    {
        _selfAnimator = GetComponent<Animator>();
        _images = GetComponentsInChildren<Image>().ToList();
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
        UpdateHealthRender();
    }

    private void OnDamaged()
    {
        UpdateHealthRender();
        _selfAnimator.SetTrigger("Damaged");
    }

    private void UpdateHealthRender()
    {
        var health = _health.Value;

        foreach (var image in _images)
            image.gameObject.SetActive(false);

        for (int i = 0; i < health; i++)
            _images[i].gameObject.SetActive(true);
    }
}
