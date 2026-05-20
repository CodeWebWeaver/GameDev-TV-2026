using UnityEngine;

public class UIManager : MonoBehaviour, IUiService {

    [SerializeField] UIPanel pauseUI;

    [SerializeField] UIPanel settingsPanel;

    public UIPanel SettingsMenu => settingsPanel;
    public UIPanel PauseUI => pauseUI;

    public void HidePanel(UIPanel panel) {
        panel.Hide();
    }

    public void ShowPanel(UIPanel panel) {
        panel.Show();
    }

    public void TogglePanel(UIPanel panel) {
        if (panel.IsOpen) {
            panel.Hide();
        } else {
            panel.Show();
        }
    }
}

public interface IUiService {
    UIPanel SettingsMenu { get; }
    UIPanel PauseUI { get; }

    void HidePanel(UIPanel pauseUI);
    void ShowPanel(UIPanel pauseUI);

    void TogglePanel(UIPanel panel);
}