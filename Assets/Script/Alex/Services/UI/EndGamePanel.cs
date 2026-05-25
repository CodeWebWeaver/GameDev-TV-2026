using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
using Zenject;

public class EndGamePanel : UIPanel {

    [SerializeField] private TextMeshProUGUI friendsCountText;
    [SerializeField] private Button gameMenuButton;
    [SerializeField] private Button continueButton;

    [Inject] GameManager gameManager;
    [InjectOptional] PlayerData playerData;

    [SerializeField] private FriendElementUI friendListPrefab;
    [SerializeField] private Transform friendListContainer;

    private ObjectPool<FriendElementUI> _friendPool;
    private readonly List<FriendElementUI> _activeFriends = new();

    public event Action OnContinueButtonClicked;
    public event Action OnExitButtonClicked;

    protected override void Awake() {
        base.Awake();

        closeButton?.gameObject.SetActive(false);

        _friendPool = new ObjectPool<FriendElementUI>(
            CreateFriendElement,
            OnGetFriendElement,
            OnReleaseFriendElement,
            OnDestroyFriendElement,
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 100
        );
    }

    private void OnEnable() {
        gameMenuButton.onClick.AddListener(HandleMenuButton);
        continueButton.onClick.AddListener(HandleContinueButton);
    }

    public void HandleContinueButton() {
        gameManager.HandleCancel();
        OnContinueButtonClicked?.Invoke();
    }
    public void HandleMenuButton() {
        gameManager.EnterMenu();
        OnExitButtonClicked?.Invoke();
    }

    private void OnDisable() {
        gameMenuButton.onClick.RemoveListener(HandleMenuButton);
        continueButton.onClick.RemoveListener(HandleContinueButton);
    }

    public override void Show() { 
        base.Show(); 
        if (playerData == null || playerData.FriendSystem == null) return;
        FriendSystem friendSystem = playerData.FriendSystem; 
        SetFriendsCount(friendSystem.Friends.Count); 
        UpdateFriendList(friendSystem.Friends); 
    }

    private void SetFriendsCount(int count) { 
        if (friendsCountText != null) {
            friendsCountText.text = $"{count}";
        }
    }

    private FriendElementUI CreateFriendElement() {
        return Instantiate(friendListPrefab, friendListContainer);
    }

    private void OnGetFriendElement(FriendElementUI element) {
        element.gameObject.SetActive(true);
    }

    private void OnReleaseFriendElement(FriendElementUI element) {
        element.gameObject.SetActive(false);
    }

    private void OnDestroyFriendElement(FriendElementUI element) {
        Destroy(element.gameObject);
    }

    private void UpdateFriendList(IReadOnlyList<HumanData> friends) {
        // Повертаємо активні елементи в пул
        foreach (var element in _activeFriends) {
            _friendPool.Release(element);
        }

        _activeFriends.Clear();

        // Створюємо/отримуємо нові
        foreach (var friend in friends) {
            FriendElementUI element = _friendPool.Get();

            element.SetName(friend.Name);
            element.SetSprite(friend.Portrait);

            _activeFriends.Add(element);
        }
    }

    protected override void OnDestroy() {
        base.OnDestroy();
        _friendPool?.Dispose();
    }
}