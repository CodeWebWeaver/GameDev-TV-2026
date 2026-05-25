using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FriendElementUI : MonoBehaviour
{
    [SerializeField] Image portrait;
    [SerializeField] TextMeshProUGUI nameLabel;

    public void SetName(string name) {
        if (nameLabel != null) {
            nameLabel.text = name;
        }
    }

    public void SetSprite(Sprite sprite) {
        if (portrait != null) {
            portrait.sprite = sprite;
        }
    }
}
