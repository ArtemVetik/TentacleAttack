using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KrakenSkinInitializer : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer _bodyMesh;
    [SerializeField] private SkinDataBase _dataBase;

    private Material _bodyMaterial;

    private void Awake()
    {
        _bodyMaterial = _bodyMesh.material;
    }

    private void OnEnable()
    {
        SkinInventory.SelectedSkinChanged += UpdateSkin;
    }

    private void OnDisable()
    {
        SkinInventory.SelectedSkinChanged -= UpdateSkin;
    }

    private void Start()
    {
        var inventory = new SkinInventory(_dataBase);
        inventory.Load();

        UpdateSkin(inventory.SelectedSkin);
    }

    private void UpdateSkin(SkinData selectedSkin)
    {
        if (selectedSkin == null)
        {
            _bodyMaterial.mainTexture = null;
        }
        else
        {
            var texture = selectedSkin.Texture;
            _bodyMaterial.mainTexture = texture;
        }
    }
}
