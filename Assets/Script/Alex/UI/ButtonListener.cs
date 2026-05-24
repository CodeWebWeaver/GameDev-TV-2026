using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class ButtonListener : MonoBehaviour, ISelectHandler {
    [Inject] private SignalBus signalBus;

    [SerializeField] private Button button;

    private void Awake() {
        if (button == null) {
            button = GetComponent<Button>();
            if (button == null) {
                Debug.LogError($"{nameof(ButtonListener)}: Button reference is missing.");
            }
        }
    }
    private void OnEnable() {
        button.onClick.AddListener(OnButtonClicked);
    }

    private void OnDisable() {
        button.onClick.RemoveListener(OnButtonClicked);
    }

    private void OnButtonClicked() {
        signalBus.Fire(new ButtonClickedSignal(button));
    }

    public void OnSelect(BaseEventData eventData) {
        signalBus.Fire(new ButtonSelectedSignal(button));
    }
}

public class ButtonClickedSignal {
    private Button button;

    public ButtonClickedSignal(Button button) {
        this.button = button;
    }
}

public class ButtonSelectedSignal {
    private Button button;
    public ButtonSelectedSignal(Button button) {
        this.button = button;
    }
}