using System.Collections.Generic;
using UnityEngine;
using UI;

namespace Player
{
    //キャラクターの所持アイテムを管理するクラス
    public class InventoryManager : MonoBehaviour
    {
        const int MAX_ENVENTORY_NUM = 8;

        List<ItemInfomation> hasItemList = new List<ItemInfomation>();  //所持アイテムのリスト
        int currentListIndex = 0;   //現在保持しているアイテムが所持アイテムリストから何番目か
        [SerializeField] ItemDataBase itemDataBase; //アイテムのデータベース
        [SerializeField] Transform characterInventoryTransform; //インベントリの位置情報
        ItemInventoryUIManager itemInventoryUIManager;
        [SerializeField] Transform holdItemPos;    //現在ホールドしているアイテムを表示する位置情報

        GameObject holditemInstance;   //現在ホールドしているゲームオブジェクトのインスタンス
        ItemInfomation holdItemInfomation;  //現在ホールドしているアイテムの情報のインスタンス

        public event System.Action<int> OnCurrentIndexChanged;
        void NotifyIndexChanged()
        {
            OnCurrentIndexChanged?.Invoke(currentListIndex);
        }

        void Start()
        {

            itemInventoryUIManager = FindObjectOfType<ItemInventoryUIManager>();
        }

        public void ActiveInventory()  //アイテムインベントリを有効可する
        {
            itemInventoryUIManager.ActiveInventory();
        }

        public void DisableInventory()  //アイテムインベントリを無効可する
        {
            itemInventoryUIManager.DisableInventory();
        }

        public void RotateInventory(float value)  //アイテムインベントリを回転
        {
            if (hasItemList.Count <= 1) return;    // 所持アイテムが1以下の時は返す
            if (!itemInventoryUIManager.IsRotate)    //回転中なら返す

                if (value > 0)
                {
                    nextItem();
                    itemInventoryUIManager.RotateItemInventory(hasItemList, ItemInventoryUIManager.RotateType.CLOCK_WISE);  //アイテムインベントリを時計回りに回転
                }
                else
                {
                    itemInventoryUIManager.RotateItemInventory(hasItemList, ItemInventoryUIManager.RotateType.COUNTER_CLOCK_WISE);  //アイテムインベントリを反時計回りに回転
                    previousItem();
                }
                NotifyIndexChanged();
        }

        public void SetHoldItem(ItemInfomation _holdItemInfomation) //ホールドしているアイテムを生成する関数
        {
            //もし持っているオブジェクトがあれば消去
            if (holdItemInfomation != null)
            {
                Destroy(holditemInstance);
            }
            //IDからホールドするアイテム情報を取得
            holdItemInfomation = GetItemFromDataBase(_holdItemInfomation.ItemID);

            //手の位置にアイテム生成
            holditemInstance = Instantiate(holdItemInfomation.HoldItem, holdItemPos.position, holdItemPos.rotation);

            //アイテムを手の子オブジェクトに
            holditemInstance.transform.parent = holdItemPos;

            //現在ホールドしているアイテム名を更新
            itemInventoryUIManager.ShowItemName(holdItemInfomation.ItemName);
        }

        //マウスホイールで所持アイテム変更
        public void ChangeHoldItem(Vector2 _inputScroll)
        {
            if (hasItemList.Count == 0) return; // 所持アイテムがない場合は終了

            if (_inputScroll.y > 0)
            {
                nextItem();
            }
            else if (_inputScroll.y < 0)
            {
                previousItem();
            }
            itemInventoryUIManager.ArrangeItemsCircle(currentListIndex);
            NotifyIndexChanged();
        }

        public void nextItem()
        {
            if (hasItemList.Count == 0) return; // 所持アイテムがない場合は終了

            int nextIndex = (currentListIndex + 1) % hasItemList.Count; // 次のアイテムのインデックスを計算
            SetHoldItem(hasItemList[nextIndex]);
            currentListIndex = nextIndex;
        }

        public void previousItem()
        {
            if (hasItemList.Count == 0) return; // 所持アイテムがない場合は終了

            int previousIndex = (currentListIndex - 1 + hasItemList.Count) % hasItemList.Count; // 前のアイテムのインデックスを計算
            SetHoldItem(hasItemList[previousIndex]);
            currentListIndex = previousIndex;
        }

        public void AddItemEnventory(int _itemID)   //アイテムを取得したときに、インベントリにアイテムを追加する関数
        {
            ItemInfomation itemInfomation = GetItemFromDataBase(_itemID);   //IDからアイテムの情報を取得
            hasItemList.Add(itemInfomation);                                //所持アイテムリストんの末尾に追加

            itemInventoryUIManager.GenerateInventoryItem(itemInfomation.InventoryItem); //インベントリ用のアイテムを生成

            currentListIndex = hasItemList.Count;     //現在の所持アイテムを拾ったオブジェクトに更新
            SetHoldItem(itemInfomation);   //取得したアイテムをホールド
            itemInventoryUIManager.ArrangeItemsCircle(currentListIndex); // 生成後に等間隔で配置し直す
            NotifyIndexChanged();
        }



        public ItemInfomation GetItemFromDataBase(int _ItemID)  //アイテムIDからアイテムの情報を返す関数
        {
            foreach (ItemInfomation Item in itemDataBase.itemList)
            {
                if (Item.ItemID == _ItemID) //指定のItemIDが見つかればその情報を返す    
                {
                    return Item;
                }
            }
            return null;
        }
    }

}
