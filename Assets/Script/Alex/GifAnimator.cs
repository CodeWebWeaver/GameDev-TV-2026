using UnityEngine;
using UnityEngine.UI;

public class GifAnimator : MonoBehaviour
{
    public Sprite[] frames; // Масив спрайтів для анімації
    public float framesPerSecond = 15f; // Кількість кадрів в секунду
    public Image image; // Компонент Image для відображення анімації

    private void Awake() {
        image = GetComponent<Image>();
        if (image == null)
        {
            Debug.LogError("GifAnimator requires an Image component.");
            enabled = false;
            return;
        }
    }

    private void Update() {
        float index = Time.time * framesPerSecond;
        index = index % frames.Length; // Зациклюємо індекс
        if (image != null) {
            image.sprite = frames[(int)index]; // Встановлюємо текстуру для Renderer
        }
    }
}

