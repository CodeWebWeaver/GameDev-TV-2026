using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceButton : MonoBehaviour {
    [SerializeField] private Button button;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TextMeshProUGUI label;

    [Header("Selection Visual")]
    [SerializeField] private SelectionIndicatorView selectionIndicator;

    [Header("Colors")]
    [SerializeField] private Color selectedBackgroundColor = Color.white;
    [SerializeField] private Color deselectedBackgroundColor = Color.black;

    [SerializeField] private Color selectedTextColor = Color.black;
    [SerializeField] private Color deselectedTextColor = Color.white;

    private Choice _choice;
    private System.Action<Choice> _onSelected;

    private void Awake() {
        button ??= GetComponent<Button>();
        backgroundImage ??= GetComponent<Image>();
        label ??= GetComponentInChildren<TextMeshProUGUI>();

        button.onClick.AddListener(OnClick);
    }

    public void Setup(Choice choice, System.Action<Choice> onSelected) {
        _choice = choice;
        _onSelected = onSelected;

        label.text = choice.text;

        SetSelected(false);
        gameObject.SetActive(true);
    }

    public void Clear() {
        _choice = null;
        _onSelected = null;

        label.text = string.Empty;

        SetSelected(false);
        gameObject.SetActive(false);
    }

    public void SetSelected(bool selected) {
        if (selectionIndicator != null) {
            if (selected) selectionIndicator.Show();
            else selectionIndicator.Hide();
        }

        label.color = selected
            ? selectedTextColor
            : deselectedTextColor;

        backgroundImage.color = selected
            ? selectedBackgroundColor
            : deselectedBackgroundColor;

        if (selected) button.OnSelect(null);
        else button.OnDeselect(null);
    }

    public void Confirm() => _onSelected?.Invoke(_choice);

    private void OnClick() {
        _onSelected?.Invoke(_choice);
    }
}