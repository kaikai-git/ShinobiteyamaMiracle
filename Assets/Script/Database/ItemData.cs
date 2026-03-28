using UnityEngine;
using UnityEngine.Localization; // Localizationを使うために必要


[System.Serializable]
public class ItemData
{
    [SerializeField] int itemID;                 //アイテムのID
    public int ItemID => itemID;

    [SerializeField] LocalizedString itemName; // String Tableの「名前」への参照
    public string ItemName => itemName.GetLocalizedString();

    [SerializeField] LocalizedString itemDescription; // String Tableの「説明」への参照
    public string ItemDescription => itemDescription.GetLocalizedString();


    [SerializeField] GameObject holdItem;//手に持った時のアイテムオブジェクト
    public GameObject HoldItem => holdItem;
    [SerializeField] GameObject inventoryItem;//アイテムインベント内のアイテムオブジェクト
    public GameObject InventoryItem => inventoryItem;
    // public Sprite icon; // アイテムのアイコン画像
    // public GameObject prefab; // 3Dモデルなどのプレハブ
}