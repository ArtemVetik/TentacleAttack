using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class LaudatoryPanel : MonoBehaviour
{
    [SerializeField] private MessagesForLaudatoryPanel _messgaes;
    [SerializeField] private TMP_Text _messageView;
    [SerializeField] private EnemyHolder _enemyHolder;
    [SerializeField] private float _showingTime;

    private Animator _selfAnimator;
    private int _enemyHoldingCount;
    private Coroutine _showingPanel;

    private const string _showPanel = "ShowPanel";
    private const string _hiddenPanel = "HiddenPanel";


    private void OnEnable()
    {
        _enemyHolder.EnemyHold += OnEnemyHold;
        _enemyHolder.EnemyLeave += OnEnemyLeave;
    }

    private void OnDisable()
    {
        _enemyHolder.EnemyHold -= OnEnemyHold;
        _enemyHolder.EnemyLeave -= OnEnemyLeave;
    }

    void Start()
    {
        _selfAnimator = GetComponent<Animator>();
        _enemyHoldingCount = 0;
    }

    private void OnEnemyHold()
    {
        _enemyHoldingCount++;

        if (_enemyHoldingCount > 1)
            ShowPanel();
    }

    private void OnEnemyLeave()
    {
        _messgaes.Reset();
        _enemyHoldingCount = 0;
    }

    private void ShowPanel()
    {
        _messageView.SetText(_messgaes.GetNextMessage());
        _selfAnimator.Play(_showPanel);

        if(_showingPanel != null)
        {
            StopCoroutine(_showingPanel);
        }
        _showingPanel = StartCoroutine(HiddenPanelTimer());
    }

    private IEnumerator HiddenPanelTimer()
    {
        yield return new WaitForSeconds(_showingTime);
        _selfAnimator.Play(_hiddenPanel);

    }

}