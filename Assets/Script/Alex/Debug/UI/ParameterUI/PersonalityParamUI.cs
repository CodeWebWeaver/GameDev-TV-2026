using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PersonalityParamUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private Image icon;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Slider valueSlider;
    [SerializeField] private bool showAsSlider = false;

    private PersonalityParam _param;
    private string _paramName;

    public void Setup(PersonalityParam param) {
        _param = param;
        _paramName = param.Name;

        // Налаштовуємо UI
        if (param.data != null) {
            UpdateIcon(param.data.IconSprite);



            if (backgroundImage != null) {
                backgroundImage.color = param.data.ParamColor;
            }
                

            if (showAsSlider && valueSlider != null) {
                valueSlider.minValue = param.data.MinValue;
                valueSlider.maxValue = param.data.MaxValue;
                valueSlider.value = param.CurrentValue;
            }
        }

        // Підписуємось на зміни
        param.OnValueChanged += OnParamValueChanged;

        // Початкове відображення
        UpdateUI(param.CurrentValue);
    }

    private void OnParamValueChanged(PersonalityParam param) {
        UpdateUI(param.CurrentValue);
    }

    public void UpdateIcon(Sprite iconSprite) {
        if (icon == null) return;

        if (iconSprite == null) {
            icon.gameObject.SetActive(false);
            return;
        }

        icon.gameObject.SetActive(true);
        icon.sprite = iconSprite;
    }

    private void UpdateUI(int value) {
        if (nameText != null)
            nameText.text = _paramName;

        if (valueText != null)
            valueText.text = value.ToString();

        if (showAsSlider && valueSlider != null)
            valueSlider.value = value;

        AnimateValueChange();
    }

    private void AnimateValueChange() {
        transform.localScale = Vector3.one * 1.1f;
        transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
    }

    private void OnDestroy() {
        if (_param != null) {
            _param.OnValueChanged -= OnParamValueChanged;
        }
    }
}
