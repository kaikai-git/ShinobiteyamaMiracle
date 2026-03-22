using UnityEngine;
[System.Serializable]
public class ItemInfomation
{
    [SerializeField] int itemID;                 //アイテムのID
    public int ItemID => itemID;
    [SerializeField] string itemName;
    public string ItemName => itemName;
    [SerializeField] GameObject holdItem;//手に持った時のアイテムオブジェクト
    public GameObject HoldItem => holdItem;
    [SerializeField] GameObject inventoryItem;//アイテムインベント内のアイテムオブジェクト
    public GameObject InventoryItem => inventoryItem;
     
     
}
