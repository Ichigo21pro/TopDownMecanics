using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem
{
    public static void SaveSlot(int slotIndex)
    {
        string date = System.DateTime.Now.ToString("d/M/yyyy - h:mm tt");
        PlayerPrefs.SetString($"SaveSlot_{slotIndex}", date);
        PlayerPrefs.Save();
    }
}
