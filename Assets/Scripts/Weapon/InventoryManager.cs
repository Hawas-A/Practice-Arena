using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InventoryManager : MonoBehaviour
{
    [Header("Inventory")]
    private List<Item> items = new List<Item>();


    public void AddItem(Item item)
    {
        if (item == null) return;

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
    return (uint)items.Count(i => i != null && i == item);
    }


   
}
