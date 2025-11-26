using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [Header("Inventory")]
    private List<Item> items = new List<Item>();


    public void AddItem(Item item)
    {
        if (item == null) return false;

        items.Add(item);
    }

    public bool RemoveItem(Item item)
    {
        if (!items.Contains(item)) return false;

        items.Remove(item);
        return true;
    }

    public bool HasItem(Item item)
    {
        return items.Contains(item);
    }

    public uint GetItemCount(Item item)
    {
    if (item == null) return 0;

    uint count = 0;

    foreach (var i in items)
    {
        if (i == item)
            count++;
    }

    return count;
    }


   
}
