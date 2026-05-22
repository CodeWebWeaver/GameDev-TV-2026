using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class FriendUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI textField;
    [SerializeField] private Image portraitImage;

    [Header("Animation")]
    [SerializeField] private RectTransform panel;
    [SerializeField] private float slideDuration = 0.5f;
    [SerializeField] private float visibleTime = 2f;
    [SerializeField] private float fadeDuration = 0.5f;

    private CanvasGroup canvasGroup;
    private Vector2 targetPosition; // Позиція в кутку, яку ви налаштували в Unity
    private Sequence currentSequence;

    private void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();

        // Запам'ятовуємо правильну позицію в кутку екрана
        if (panel != null) {
            targetPosition = panel.anchoredPosition;
        } else {
            targetPosition = Vector2.zero;
        }
    }

    public void SetPortrait(Sprite portrait) {
        if (portrait == null) {
            portraitImage.gameObject.SetActive(false);
            return;
        }

        portraitImage.gameObject.SetActive(true);
        portraitImage.sprite = portrait;
    }

    public void SetFriendName(string friendName) {
        textField.text = $"Friend added: {friendName}";
    }

    public void PopUp() {
        // Якщо анімація вже грає, вбиваємо її, щоб уникнути багів при швидкому повторному виклику
        if (currentSequence != null && currentSequence.IsActive()) {
            currentSequence.Kill();
        }

        gameObject.SetActive(true);

        // Розраховуємо зміщення вправо за межі екрану. 
        // Беремо ширину панелі (або 500 як запасний варіант), щоб сховати її повністю.
        float panelWidth = panel != null ? panel.rect.width : 500f;
        Vector2 startPosition = new Vector2(targetPosition.x + panelWidth + 50f, targetPosition.y);

        // Встановлюємо початковий стан перед анімацією
        panel.anchoredPosition = startPosition;
        canvasGroup.alpha = 1f;

        // Створюємо нову послідовність анімації
        currentSequence = DOTween.Sequence();

        // 1. Плавний виїзд справа у свій рідний куток
        currentSequence.Append(panel.DOAnchorPos(targetPosition, slideDuration)
            .SetEase(Ease.OutBounce));

        // 2. Очікування
        currentSequence.AppendInterval(visibleTime);

        // 3. Плавне зникнення (альфа в 0)
        currentSequence.Append(canvasGroup.DOFade(0f, fadeDuration));

        // 4. Вимикаємо об'єкт після завершення
        currentSequence.OnComplete(() => {
            gameObject.SetActive(false);
        });
    }

    private void OnDestroy() {
        // Завжди чистимо твіни при знищенні об'єкта, щоб уникнути витоку пам'яті
        if (currentSequence != null) currentSequence.Kill();
    }

#if UNITY_EDITOR
    [Header("Test Editor Settings")]
    [SerializeField] private Sprite portrait;
    [SerializeField] private string friendNameTest = "John Doe";

    [ContextMenu("Add Friend")]
    private void TestAddFriend() {
        // Якщо тестуємо в едіторі без запуску гри, Awake міг не спрацювати
        if (!Application.isPlaying && panel != null) {
            targetPosition = panel.anchoredPosition;
            canvasGroup = GetComponent<CanvasGroup>();
        }

        SetFriendName(friendNameTest);
        SetPortrait(portrait);
        PopUp();
    }
#endif
}