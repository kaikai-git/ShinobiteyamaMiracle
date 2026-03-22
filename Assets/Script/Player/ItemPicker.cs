using Item;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Player.InventoryManager))]
    public class ItemPicker : MonoBehaviour
    {
        private Ray characterRay;
        [SerializeField] private float characterReach = 2.5f;
        [SerializeField] float heightOffset;
        InventoryManager InventoryManager;
        ItemBase itemBase;
        void Start()
        {
            InventoryManager = GetComponent<InventoryManager>();
        }


        public void GetItem()
        {
            characterRay.origin = transform.position + heightOffset * Vector3.up;
            characterRay.direction = transform.forward;
            RaycastHit hit;
            Debug.DrawRay(characterRay.origin, characterRay.direction * characterReach, Color.red, 1.0f);
            if (Physics.Raycast(characterRay, out hit, characterReach))
            {
                if (hit.collider.gameObject.tag == "Item")
                {
                    itemBase = hit.collider.GetComponent<ItemBase>();   //例がヒットしたオブジェクトのitemBaseを取得
                    int getItemID = itemBase.GettedItem();              //ヒットしたアイテムのIDを取得
                    InventoryManager.AddItemEnventory(getItemID);//インベントリにアイテムを加える。

                }
            }

        }
    }
}

