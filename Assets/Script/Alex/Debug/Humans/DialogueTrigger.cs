using System;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{

    public event Action<bool, Player> OnDialoguePossible;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            //Debug.Log("Player entered dialogue trigger");
            if (other.TryGetComponent<Player>(out var player)) {
                OnDialoguePossible?.Invoke(true, player);
            } else {
                OnDialoguePossible?.Invoke(true, null);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            //Debug.Log("Player exited dialogue trigger");
            if (other.TryGetComponent<Player>(out var player)) {
                OnDialoguePossible?.Invoke(false, player);
            } else {
                OnDialoguePossible?.Invoke(false, null);
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        //Debug.Log("Player inside dialogue trigger");
    }
}
