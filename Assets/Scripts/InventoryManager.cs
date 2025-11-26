using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [Header("Inventory")]
    private List<Item> items = new List<Item>();


    public bool AddItem(Item item)
    {
        if (item == null) return false;

        items.Add(item);
        return true;
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

   
}
