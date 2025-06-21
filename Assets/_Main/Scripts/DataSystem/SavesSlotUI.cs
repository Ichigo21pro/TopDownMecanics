using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SavesSlotUI : MonoBehaviour
{
    public TextMeshProUGUI slotText;

    public void LoadSlot(int slotIndex)
    {
        if (PlayerPrefs.HasKey($"SaveSlot_{slotIndex}"))
        {
            string text = PlayerPrefs.GetString($"SaveSlot_{slotIndex}");
            slotText.text = text;
        }
        else
        {
            slotText.text = "Nueva Partida";
        }
    }
}
