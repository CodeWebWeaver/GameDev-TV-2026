public interface IGameManager {
    ContextStateMachine<GameManager> StateMachine { get; }

    void ExitGame();
    void StartNewGame();
}