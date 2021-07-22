using System.Collections;
using UnityEngine;

public class ToggleDisappearingObjectsTrigger : MonoBehaviour
{
    [SerializeField] private Transform _parent;
    [SerializeField] private TriggerSpeaker _openTrigger;
    [SerializeField] private TriggerSpeaker _closeTrigger;

    private GameObject[] _disappearingObjects;
    private bool _isActive;

    private void OnEnable()
    {
        _openTrigger.TriggerEnter += OnOpenTriggerEnter;
        _closeTrigger.TriggerEnter += OnCloseTriggerEnter;
    }

    private void OnDisable()
    {
        _openTrigger.TriggerEnter -= OnOpenTriggerEnter;
        _closeTrigger.TriggerEnter -= OnCloseTriggerEnter;
    }

    private void Start()
    {
        _isActive = true;
        FillingObjects();
    }

    private void OnOpenTriggerEnter() => ToggleActive(false, _disappearingObjects);

    private void OnCloseTriggerEnter() => ToggleActive(true, _disappearingObjects);

    private void FillingObjects()
    {
        _disappearingObjects = new GameObject[_parent.childCount];

        for(int i = 0; i < _disappearingObjects.Length; i++)
        {
            _disappearingObjects[i] = _parent.GetChild(i).gameObject;
        }
        
    }

    private void ToggleActive(bool isActive, params GameObject[] gameObjects)
    {
        foreach(var gameObject in gameObjects)
        {
            gameObject.SetActive(isActive);
        }

        _isActive = isActive;
    }

}
