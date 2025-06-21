using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    public List<SavesSlotUI> slots;

    void Start()
    {
       for (int i = 0; i < slots.Count; i++)
       {
          slots[i].LoadSlot(i);
       } 
    }
}
