using System;
using Tymski;
using UnityEngine;
using UnityEngine.SceneManagement;

public interface ISceneDataService {
    SceneReference GetSceneReference(SceneTypes sceneType);
    bool IsMainMenu();
}


public class SceneDataService : MonoBehaviour, ISceneDataService {
    [SerializeField] private SceneData sceneData;
    public SceneDataService(SceneData sceneData) {
        this.sceneData = sceneData;
    }

    public SceneReference GetSceneReference(SceneTypes sceneType) {
        return sceneType switch {
            SceneTypes.MainMenu => sceneData.mainMenuScene,
            SceneTypes.Game => sceneData.gameScene,
            _ => throw new ArgumentOutOfRangeException(nameof(sceneType), sceneType, null)
        };
    }

    public string GetGameSceneName() {
        return sceneData.gameScene.SceneName;
    }

    public string GetMenuSceneName() {
        return sceneData.mainMenuScene.SceneName;
    }

    public bool IsMainMenu() {
        Scene scene = SceneManager.GetActiveScene();
        string currentSceneName = GetSceneReference(SceneTypes.MainMenu).SceneName;
        return currentSceneName.Equals(scene.name);
    }
}

public enum SceneTypes {
    MainMenu,
    Game
}

[Serializable]
public class SceneData {
    public SceneReference mainMenuScene;
    public SceneReference gameScene;
}