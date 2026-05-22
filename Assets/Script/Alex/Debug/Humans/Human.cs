using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using static UnityEngine.Rendering.DebugUI;

public class Human : MonoBehaviour {
    [SerializeField] private PersonDataSO personDataSO;
    [Inject] SignalBus signalBus;

    public string Name =>
        personDataSO != null
            ? personDataSO.Name
            : string.Empty;

    public Sprite Portrait => personDataSO != null
            ? personDataSO.Portrait
            : null;

    public int Happiness { get; private set; } = 4;

    protected virtual void Awake() {
        friendSystem = new(signalBus);
    }

    public FriendSystem FriendSystem => friendSystem;
    private FriendSystem friendSystem;

    public void ChangeHappiness(int delta) {
        Happiness += delta;
        Happiness = Mathf.Clamp(Happiness, 0, 10);
    }
}


[Serializable]
public class FriendSystem {
    private readonly List<Human> friends = new();

    public IReadOnlyList<Human> Friends => friends;

    private readonly SignalBus _signalBus;

    public FriendSystem(SignalBus signalBus) {
        _signalBus = signalBus;
    }

    public bool AddFriend(Human friend) {
        if (friend == null || friends.Contains(friend))
            return false;

        friends.Add(friend);

        _signalBus.Fire(new FriendAddedSignal(friend));

        return true;
    }

    public bool RemoveFriend(Human human) {
        return friends.Remove(human);
    }

    public bool IsFriend(Human human) {
        return friends.Contains(human);
    }

    public int Count => friends.Count;
}