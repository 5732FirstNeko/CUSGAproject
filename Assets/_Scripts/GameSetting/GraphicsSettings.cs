using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

//半山腰太挤，你总得去山顶看看//
public class GraphicsSettings : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private TMP_Dropdown antiAliasingDropdown;
    [SerializeField] private Toggle vsyncToggle;
    [SerializeField] private Slider brightnessSlider;
    [SerializeField] private Button applyButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Button defaultButton;
    [SerializeField] private GameObject confirmationPanel;

    [Header("Other Settings")]
    [SerializeField] private AudioMixer audioMixer;

    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions;
    private float currentRefreshRate;
    private int currentResolutionIndex = 0;

    // 保存当前设置用于取消操作
    private int tempQualityLevel;
    private int tempAntiAliasing;
    private bool tempVsync;
    private float tempBrightness;
    private int tempResolutionIndex;

    void Start()
    {
        // 初始化UI事件
        applyButton.onClick.AddListener(ApplySettings);
        cancelButton.onClick.AddListener(CancelSettings);
        defaultButton.onClick.AddListener(SetDefaultSettings);
        confirmationPanel.SetActive(false);

        // 初始化分辨率选项
        InitializeResolutions();

        // 初始化画质选项
        InitializeQualitySettings();

        // 初始化抗锯齿选项
        InitializeAntiAliasing();

        // 加载当前设置
        LoadCurrentSettings();

        // 保存当前设置作为临时设置
        SaveTemporarySettings();
    }

    // 初始化分辨率选项
    private void InitializeResolutions()
    {
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();
        resolutionDropdown.ClearOptions();
        currentRefreshRate = Screen.currentResolution.refreshRate;

        // 过滤相同刷新率的分辨率
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].refreshRate == currentRefreshRate)
            {
                filteredResolutions.Add(resolutions[i]);
            }
        }

        // 添加分辨率选项
        List<string> options = new List<string>();
        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            string resolutionOption = $"{filteredResolutions[i].width} x {filteredResolutions[i].height} " +
                                     $"{filteredResolutions[i].refreshRate}Hz";
            options.Add(resolutionOption);

            // 记录当前分辨率索引
            if (filteredResolutions[i].width == Screen.width &&
                filteredResolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    // 初始化画质选项
    private void InitializeQualitySettings()
    {
        qualityDropdown.ClearOptions();
        List<string> qualityOptions = new List<string>(QualitySettings.names);
        qualityDropdown.AddOptions(qualityOptions);
        qualityDropdown.value = QualitySettings.GetQualityLevel();
        qualityDropdown.RefreshShownValue();
    }

    // 初始化抗锯齿选项
    private void InitializeAntiAliasing()
    {
        antiAliasingDropdown.ClearOptions();
        List<string> aaOptions = new List<string>
        {
            "关闭 (0x)",
            "2x MSAA",
            "4x MSAA",
            "8x MSAA"
        };
        antiAliasingDropdown.AddOptions(aaOptions);

        // 根据当前设置选择对应选项
        switch (QualitySettings.antiAliasing)
        {
            case 0:
                antiAliasingDropdown.value = 0;
                break;
            case 2:
                antiAliasingDropdown.value = 1;
                break;
            case 4:
                antiAliasingDropdown.value = 2;
                break;
            case 8:
                antiAliasingDropdown.value = 3;
                break;
        }

        antiAliasingDropdown.RefreshShownValue();
    }

    // 加载当前设置到UI
    private void LoadCurrentSettings()
    {
        vsyncToggle.isOn = QualitySettings.vSyncCount > 0;

        // 加载亮度设置（假设使用AudioMixer来控制亮度）
        if (audioMixer != null)
        {
            float brightness;
            audioMixer.GetFloat("Brightness", out brightness);
            brightnessSlider.value = brightness;
        }
    }

    // 保存临时设置
    private void SaveTemporarySettings()
    {
        tempQualityLevel = qualityDropdown.value;
        tempAntiAliasing = antiAliasingDropdown.value;
        tempVsync = vsyncToggle.isOn;
        tempBrightness = brightnessSlider.value;
        tempResolutionIndex = resolutionDropdown.value;
    }

    // 应用设置
    public void ApplySettings()
    {
        // 应用分辨率设置
        Resolution resolution = filteredResolutions[resolutionDropdown.value];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        // 应用画质等级
        QualitySettings.SetQualityLevel(qualityDropdown.value);

        // 应用抗锯齿
        switch (antiAliasingDropdown.value)
        {
            case 0:
                QualitySettings.antiAliasing = 0;
                break;
            case 1:
                QualitySettings.antiAliasing = 2;
                break;
            case 2:
                QualitySettings.antiAliasing = 4;
                break;
            case 3:
                QualitySettings.antiAliasing = 8;
                break;
        }

        // 应用垂直同步
        QualitySettings.vSyncCount = vsyncToggle.isOn ? 1 : 0;

        // 应用亮度设置
        if (audioMixer != null)
        {
            audioMixer.SetFloat("Brightness", brightnessSlider.value);
        }

        // 显示确认面板
        confirmationPanel.SetActive(true);
        Invoke(nameof(HideConfirmationPanel), 2f);

        // 保存当前设置作为临时设置
        SaveTemporarySettings();
    }

    // 取消设置（恢复到之前的状态）
    public void CancelSettings()
    {
        qualityDropdown.value = tempQualityLevel;
        antiAliasingDropdown.value = tempAntiAliasing;
        vsyncToggle.isOn = tempVsync;
        brightnessSlider.value = tempBrightness;
        resolutionDropdown.value = tempResolutionIndex;

        qualityDropdown.RefreshShownValue();
        antiAliasingDropdown.RefreshShownValue();
        resolutionDropdown.RefreshShownValue();
    }

    // 设置为默认值
    public void SetDefaultSettings()
    {
        qualityDropdown.value = 2; // 默认中等画质
        antiAliasingDropdown.value = 1; // 默认2x MSAA
        vsyncToggle.isOn = true; // 默认开启垂直同步
        brightnessSlider.value = 0; // 默认亮度

        qualityDropdown.RefreshShownValue();
        antiAliasingDropdown.RefreshShownValue();
    }

    // 隐藏确认面板
    private void HideConfirmationPanel()
    {
        confirmationPanel.SetActive(false);
    }

    // 切换全屏/窗口模式
    public void ToggleFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
