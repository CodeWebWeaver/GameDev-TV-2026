using System;
using UnityEngine;
using Zenject;

public class FriendService : MonoBehaviour
{
    [SerializeField] FriendUI friendUI;
    [Inject] SignalBus signalBus;

    private void HandleFriendAdded(FriendAddedSignal signal) {
        friendUI.SetFriendName(signal.Friend.Name);
        friendUI.SetPortrait(signal.Friend.Portrait);
        friendUI.PopUp();
    }

    private void OnEnable() {
        signalBus.Subscribe<FriendAddedSignal>(HandleFriendAdded);
    }

    private void OnDisable() {
        signalBus.Unsubscribe<FriendAddedSignal>(HandleFriendAdded);
    }

    public void ShowAllFriends() {
        // soon
    }

    public void HideAllFriends() {
        // soon
    }
}

