using UnityEngine;

namespace Item
{
    public class ItemBase : MonoBehaviour
    {
        [SerializeField] int itemID;
        public int GettedItem()//アイテム自体が取得された時の処理
        {
            Destroy(this.gameObject);   //自身を破棄
            return itemID;
        }
    }
}

