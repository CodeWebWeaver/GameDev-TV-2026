using UnityEngine;

public class DayNightTester : MonoBehaviour
{
    [ContextMenu("Transition to Morning")]
    void TestMorning() => DayNightController.Instance.TransitionToPhase(DayNightController.DayPhase.Morning);

    [ContextMenu("Transition to Afternoon")]
    void TestAfternoon() => DayNightController.Instance.TransitionToPhase(DayNightController.DayPhase.Afternoon);

    [ContextMenu("Transition to Night")]
    void TestNight() => DayNightController.Instance.TransitionToPhase(DayNightController.DayPhase.Night);
}
