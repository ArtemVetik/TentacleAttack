using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class KrakenClothInitializer : MonoBehaviour
{
    [SerializeField] private Transform _clothContainer;
    [SerializeField] private ClothDataBase _dataBase;

    private Animator _animator;
    private ClothShop _clothShop;
    private ClothData _instData;

    public KrakenCloth InstCloth { get; private set; }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _clothShop = FindObjectOfType<ClothShop>(true);
    }

    private void OnEnable()
    {
        _clothShop.SelectDataChanged += OnSelectedClothChanged;
    }

    private void OnDisable()
    {
        _clothShop.SelectDataChanged -= OnSelectedClothChanged;
    }

    private void Start()
    {
        var inventory = new ClothInventory(_dataBase);
        inventory.Load();

        OnSelectedClothChanged(inventory.SelectedSkin);
    }

    private void OnSelectedClothChanged(ClothData data)
    {
        DeselectAllCloth();
        if (data != null)
            SelectCloth(data);
    }

    public void SelectCloth(ClothData clothData)
    {
        _instData = clothData;
        InstCloth = Instantiate(clothData.Prefab, _clothContainer);
        InstCloth.transform.localPosition = Vector3.zero;
        InstCloth.transform.localRotation = Quaternion.identity;

        InstCloth.Init(_animator.GetCurrentAnimatorStateInfo(0));
    }

    public void DeselectAllCloth()
    {
        _instData = null;
        InstCloth = null;

        for (int i = _clothContainer.childCount - 1; i >= 0; i--)
            Destroy(_clothContainer.GetChild(i).gameObject);
    }
}
