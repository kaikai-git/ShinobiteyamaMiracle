using UnityEngine;
using System.Collections.Generic;
using System.Linq;
[CreateAssetMenu]
public class ItemDataBase : ScriptableObject
{
   // public List<ItemInfomation> itemList = new List<ItemInfomation>();

    public List<ItemData> itemdataList = new List<ItemData>();


    public ItemData GetItemById(int id)
    {
        return itemdataList.FirstOrDefault(item => item.ItemID == id);
    }
}
