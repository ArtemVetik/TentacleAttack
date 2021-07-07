using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class DragToMovePresenter : MonoBehaviour
{
    private SplineMovement _splineMovement;
    private TMP_Text _text;
    private Coroutine _presentCoroutine;

    private void Awake()
    {
        _splineMovement = FindObjectOfType<SplineMovement>();
        _text = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        _splineMovement.FullRewinded += OnFullRewinded;
        _splineMovement.SplineChanged += OnSplineChanged;
    }

    private void OnDisable()
    {
        _splineMovement.FullRewinded -= OnFullRewinded;
        _splineMovement.SplineChanged -= OnSplineChanged;
    }

    private void OnFullRewinded()
    {
        _presentCoroutine = StartCoroutine(PresentText(2f));
    }

    private void OnSplineChanged()
    {
        if (_presentCoroutine != null)
        {
            StopCoroutine(_presentCoroutine);
            _presentCoroutine = null;
        }

        _text.enabled = false;
    }

    private IEnumerator PresentText(float delay)
    {
        yield return new WaitForSeconds(delay);
        _text.enabled = true;
    }
}
