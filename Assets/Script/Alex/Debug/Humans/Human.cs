using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class Human : MonoBehaviour {
    [SerializeField] protected PersonDataSO personDataSO;

    public string Name => personDataSO != null ? personDataSO.Name : string.Empty;
    public Sprite Portrait => personDataSO != null ? personDataSO.Portrait : null;

    public int Happiness { get; private set; } = 4;

    public HumanData HumanData => humanData;
    private HumanData humanData;

    public virtual FriendSystem FriendSystem => humanData.FriendSystem;

    protected SignalBus signalBus;

    protected virtual void Awake() {
        humanData = new HumanData(Name, Portrait, signalBus);
    }

    [Inject]
    private void Construct(SignalBus signalBus) {
        this.signalBus = signalBus;
        humanData = new HumanData(signalBus);
    }

    public void ChangeHappiness(int delta) {
        Happiness = Mathf.Clamp(Happiness + delta, 0, 10);
    }
}

public class HumanData {
    public FriendSystem FriendSystem => friendSystem;
    public Sprite Portrait { get; internal set; }
    public string Name { get; internal set; }

    private FriendSystem friendSystem;
    public HumanData(SignalBus signalBus) {
        friendSystem = new FriendSystem(this, signalBus);
    }

    public HumanData(string name, Sprite portrait, SignalBus signalBus) {
        Name = name;
        Portrait = portrait;
        friendSystem = new FriendSystem(this, signalBus);

    }

    public void ResetData() {
        Name = string.Empty;
        Portrait = null;
        friendSystem.Reset();
    }
}

public class PlayerData : HumanData {
    public PlayerData(SignalBus signalBus) : base(signalBus) {
        Debug.Log("player data created");
    }

    
}

[Serializable]
public class FriendSystem {
    private readonly List<HumanData> friends = new();
    private readonly HumanData owner;
    private readonly SignalBus signalBus;

    public IReadOnlyList<HumanData> Friends => friends;

    public event Action<HumanData> OnFriendAdded;

    public FriendSystem(HumanData owner, SignalBus signalBus) {
        this.owner = owner ?? throw new ArgumentNullException(nameof(owner));
        this.signalBus = signalBus ?? throw new ArgumentNullException(nameof(signalBus));
    }

    public bool AddFriend(HumanData friend) {
        if (friend == null || friends.Contains(friend))
            return false;

        friends.Add(friend);

        // Подія C# для локальних потреб
        OnFriendAdded?.Invoke(friend);

        // Глобальний сигнал Zenject надсилається ОДРАЗУ звідси!
        signalBus.Fire(new FriendAddedSignal(owner, friend));

        return true;
    }

    public bool RemoveFriend(HumanData human) => friends.Remove(human);
    public bool IsFriend(HumanData human) => friends.Contains(human);

    public void Reset() {
        friends.Clear();
    }

    public int Count => friends.Count;
}

public class FriendAddedSignal {
    public HumanData FriendAddedBy;
    public HumanData FriendAddedOn;

    public FriendAddedSignal(HumanData friendAddedBy, HumanData  friendAddedOn) {
        FriendAddedBy = friendAddedBy;
        FriendAddedOn = friendAddedOn;
    }
}
