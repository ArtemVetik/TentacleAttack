using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxShift : MonoBehaviour
{
    [SerializeField] private Transform _target;

    private Material _skybox;

    private void Start()
    {
        _skybox = RenderSettings.skybox;
    }

    private void Update()
    {
        _skybox.SetFloat("_CubemapPosition", _target.position.y * -0.01f);
        _skybox.SetFloat("_FogPosition", _target.position.y * -0.01f);
    }
}
