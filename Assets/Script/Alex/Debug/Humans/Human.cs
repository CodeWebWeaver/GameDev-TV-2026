using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Human : MonoBehaviour {
    [SerializeField] private PersonDataSO personDataSO;

    public string Name => personDataSO != null ? personDataSO.Name : string.Empty;
    public Sprite Portrait => personDataSO != null ? personDataSO.Portrait : null;
    public int Happiness { get; private set; } = 4;

    public FriendSystem FriendSystem => friendSystem;
    private FriendSystem friendSystem;

    [Inject]
    private void Construct(SignalBus signalBus) {
        friendSystem = new FriendSystem(this, signalBus);
    }

    public void ChangeHappiness(int delta) {
        Happiness = Mathf.Clamp(Happiness + delta, 0, 10);
    }
}

[Serializable]
public class FriendSystem {
    private readonly List<Human> friends = new();
    private readonly Human owner;
    private readonly SignalBus signalBus;

    public IReadOnlyList<Human> Friends => friends;

    public event Action<Human> OnFriendAdded;

    public FriendSystem(Human owner, SignalBus signalBus) {
        this.owner = owner ?? throw new ArgumentNullException(nameof(owner));
        this.signalBus = signalBus ?? throw new ArgumentNullException(nameof(signalBus));
    }

    public bool AddFriend(Human friend) {
        if (friend == null || friends.Contains(friend))
            return false;

        friends.Add(friend);

        // Подія C# для локальних потреб
        OnFriendAdded?.Invoke(friend);

        // Глобальний сигнал Zenject надсилається ОДРАЗУ звідси!
        signalBus.Fire(new FriendAddedSignal(owner, friend));

        return true;
    }

    public bool RemoveFriend(Human human) => friends.Remove(human);
    public bool IsFriend(Human human) => friends.Contains(human);
    public int Count => friends.Count;
}