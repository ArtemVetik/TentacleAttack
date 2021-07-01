using UnityEngine;
using TMPro;


public class CharactersCountView : MonoBehaviour
{
    [SerializeField] private TMP_Text _countView;
    [SerializeField] private EnemyContainer _container;

    private void OnDisable()
    {
        _container.EnemyStucked -= ShowCount;
    }

    private void Start()
    {
        _container = FindObjectOfType<EnemyContainer>();
        _container.EnemyStucked += ShowCount;
        ShowCount(null);
    }

    private void ShowCount(Enemy enemy)
    {
        _countView.SetText("X" + _container.AliveEnemyCount);
    }


}