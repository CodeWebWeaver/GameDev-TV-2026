using UnityEngine;

[CreateAssetMenu(fileName = "PersonalityParamSO", menuName = "ScriptableObjects/PersonalityParamSO", order = 1)]
public class PersonalityParamSO : ScriptableObject {
    [SerializeField] private string paramName;
    [SerializeField] private Sprite icon;
    [SerializeField] private int minValue = 0;
    [SerializeField] private int maxValue = 10;
    [SerializeField] private int defaultValue = 0;
    [SerializeField] private Color paramColor = Color.white;

    public string Name => paramName;
    public Sprite IconSprite => icon;
    public int MinValue => minValue;
    public int MaxValue => maxValue;
    public int DefaultValue => defaultValue;
    public Color ParamColor => paramColor;
}