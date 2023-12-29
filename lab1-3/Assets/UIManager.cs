using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;
using UnityEngine.UI.Extensions.ColorPicker;

public class UIManager : MonoBehaviour
{
    [Header("UITerrain")]
    public Text textTerrainSizeY;
    public Slider sliderTerrainSizeY;
    public ColorPickerControl colorPickerTerrainFirst;
    public ColorPickerControl colorPickerTerrainSecond;
    public MeshGenerator meshGenerator;

    [Header("UIWater")]
    public Text textWaterSpeedWave;
    public Text textWaterHeightWave;
    public Text textWaterDepth;
    public Slider sliderWaterSpeedWave;
    public Slider sliderWaterHeightWave;
    public Slider sliderWaterDepth;
    public Material materialWater;

    [Header("UISky")]
    public Text textSkySpeed;
    public Text textSkyHeight;
    public Text textSkyFog;
    public Slider sliderSkySpeed;
    public Slider sliderSkyHeight;
    public Slider sliderSkyFog;
    public Volume volume;

    [Header("UIButton")]
    public Button buttonTerrainSettings;
    public Button buttonWaterSettings;
    public Button buttonSkySettings;
    public Button buttonSetColors;


    public static UIManager Instance { get { if (instance == null) instance = FindObjectOfType<UIManager>(); return instance; } private set { } }
    private static UIManager instance;
    
    
    public void SetSizeTerrain()
    {
        meshGenerator.SetSizeTerrain((int)sliderTerrainSizeY.value);
    }
    public void OnClickSetColors()
    {
        GradientColorKey[] colorKeys = new GradientColorKey[2];
        colorKeys[0].color = colorPickerTerrainFirst.CurrentColor;
        colorKeys[0].time = 0f;
        colorKeys[1].color = colorPickerTerrainSecond.CurrentColor;
        colorKeys[1].time = 1f;

        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
        alphaKeys[0].alpha = 1f;
        alphaKeys[1].alpha = 1f;

        meshGenerator.gradient.SetKeys(colorKeys, alphaKeys);
    }
    public void SetSpeedWaveWater()
    {
        materialWater.SetFloat("_SpeedWave", sliderWaterSpeedWave.value);
    }
    public void SetHeightWaveWater()
    {
        materialWater.SetFloat("_Displacment", sliderWaterHeightWave.value);
    }
    public void SetDepthWater()
    {
        materialWater.SetFloat("_Depth", sliderWaterDepth.value);
    }
    public void SetSpeedSky()
    {
        if (volume.profile.TryGet<VolumetricClouds>(out var cloud)) 
        {
            var currentSpeed = cloud.globalWindSpeed.value;
            currentSpeed.additiveValue = sliderSkySpeed.value;
            cloud.globalWindSpeed.value = currentSpeed;
        }
    }
    public void SetHeightSky()
    {
        if (volume.profile.TryGet<VolumetricClouds>(out var cloud))
        {
            var currentHeight = cloud.lowestCloudAltitude.value;
            currentHeight = sliderSkyHeight.value;
            cloud.lowestCloudAltitude.value = currentHeight;
        }
    }
    public void SetFogSky()
    {
        if (volume.profile.TryGet<Fog>(out var fog))
        {
            var fogDistance = fog.meanFreePath.value;
            fogDistance = sliderSkyFog.value;
            fog.meanFreePath.value = fogDistance;
        }
    }
    public void OnClickTerrain()
    {
        textTerrainSizeY.gameObject.active = textTerrainSizeY.gameObject.active ? false : true;
        sliderTerrainSizeY.gameObject.active = sliderTerrainSizeY.gameObject.active ? false : true;
        colorPickerTerrainFirst.gameObject.active = colorPickerTerrainFirst.gameObject.active ? false : true;
        colorPickerTerrainSecond.gameObject.active = colorPickerTerrainSecond.gameObject.active ? false : true;
        buttonWaterSettings.gameObject.active = buttonWaterSettings.gameObject.active ? false : true;
        buttonSkySettings.gameObject.active = buttonSkySettings.gameObject.active ? false : true;
        buttonSetColors.gameObject.active = buttonSetColors.gameObject.active ? false : true;
    }
    public void OnClickWater()
    {
        textWaterSpeedWave.gameObject.active = textWaterSpeedWave.gameObject.active ? false : true;
        textWaterHeightWave.gameObject.active = textWaterHeightWave.gameObject.active ? false : true;
        textWaterDepth.gameObject.active = textWaterDepth.gameObject.active ? false : true;
        sliderWaterSpeedWave.gameObject.active = sliderWaterSpeedWave.gameObject.active ? false : true;
        sliderWaterHeightWave.gameObject.active = sliderWaterHeightWave.gameObject.active ? false : true;
        sliderWaterDepth.gameObject.active = sliderWaterDepth.gameObject.active ? false : true;
        buttonTerrainSettings.gameObject.active = buttonTerrainSettings.gameObject.active ? false : true;
        buttonSkySettings.gameObject.active = buttonSkySettings.gameObject.active ? false : true;
    }
    public void OnClickSky()
    {
        textSkySpeed.gameObject.active = textSkySpeed.gameObject.active ? false : true;
        textSkyHeight.gameObject.active = textSkyHeight.gameObject.active ? false : true;
        textSkyFog.gameObject.active = textSkyFog.gameObject.active ? false : true;
        sliderSkySpeed.gameObject.active = sliderSkySpeed.gameObject.active ? false : true;
        sliderSkyHeight.gameObject.active = sliderSkyHeight.gameObject.active ? false : true;
        sliderSkyFog.gameObject.active = sliderSkyFog.gameObject.active ? false : true;
        buttonTerrainSettings.gameObject.active = buttonTerrainSettings.gameObject.active ? false : true;
        buttonWaterSettings.gameObject.active = buttonWaterSettings.gameObject.active ? false : true;
    }

    private void Awake()
    {
        instance = this;
    }
}
