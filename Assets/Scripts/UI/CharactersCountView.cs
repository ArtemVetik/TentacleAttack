using UnityEngine;
using TMPro;


public class CharactersCountView : MonoBehaviour
{
    [SerializeField] private TMP_Text _countView;
    [SerializeField] private EnemyContainer _container;

    private void OnEnable()
    {
        _container.EnemyStucked += ShowCount;
    }

    private void OnDisable()
    {
        _container.EnemyStucked -= ShowCount;
    }

    private void Start()
    {
        ShowCount(null);
    }

    private void ShowCount(Enemy enemy)
    {
        _countView.SetText("X" + _container.AliveEnemyCount);
    }


}