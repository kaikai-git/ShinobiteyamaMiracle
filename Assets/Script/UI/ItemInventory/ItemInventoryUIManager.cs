using UnityEngine;
using System.Collections.Generic;
using TMPro;

namespace UI
{
    //インベントリ―のアイテムを表示する関数。
    public class ItemInventoryUIManager : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI itemName;     //アイテム名UI
        [SerializeField] GameObject itemInventoryUIPanel;
        [SerializeField] GameObject itemInventoryCamera;
        [SerializeField] Transform characterInventoryTransform; //アイテムインベントリの位置情報
        const float ROTATE_SPEED = 300.0f;   //アイテムが回転する速度
        float targetAngle = 0.0f;   //回転アニメーションで回転する角度

        RenderTexture renderTexture;
        [Header("配置設定")]
        [SerializeField] private float radius = 1.0f;        // 円の半径
        [SerializeField] private float yOffset = 0.0f;       // 各アイテムの高さ（ローカル）
        [SerializeField] private float startAngleDeg = 0f;   // 配置開始角度（度）
        [SerializeField] private bool faceCenter = true;     // アイテムを中心に向けるか
        bool isChangeRotate = false;    //アイテム変更による回転演出中フラグ
        public bool IsRotate => isChangeRotate;
        int currentListIndex;
         List<GameObject> hasItemList = new List<GameObject>();  //所持アイテムのリスト


        [SerializeField]AudioClip openInventorySE;

        public enum RotateType  //回転の種類
        {
            CLOCK_WISE,
            COUNTER_CLOCK_WISE
        }
        RotateType currentRotateType;   //指定された回転の種類
       

        void Start()
        {
            var cm = FindObjectOfType<Player.InventoryManager>();
            if (cm != null) cm.OnCurrentIndexChanged += idx => currentListIndex = idx;

            // Camera コンポーネントを取得
            Camera cam = itemInventoryCamera.GetComponent<Camera>();
            renderTexture = cam.targetTexture;
        }

        void Update()
        {
            UpdateRotateInventory();
            RotateCurrentItem();
        }
        public void ActiveInventory()  //アイテムインベントリを有効可する
        {

            SoundManager.Instance.PlaySE(openInventorySE);
            itemInventoryUIPanel.SetActive(true);
            itemInventoryCamera.SetActive(true);
        }

        public void DisableInventory()  //アイテムインベントリを無効可する
        {
            itemInventoryUIPanel.SetActive(false);
            itemInventoryCamera.SetActive(false);
            ResetRendererTexture();  //オフに切り替わった時
        }

        void ResetRendererTexture() //RendererTextureをリセット
        {
            RenderTexture.active = renderTexture;
            GL.Clear(true, true, Color.clear);
            RenderTexture.active = null;
        }

        public void GenerateInventoryItem(GameObject _inventoryItem)    //アイテムを取得したときに、UI用のオブジェクトを生成
        {
            GameObject inventoryUIItemObj = Instantiate(_inventoryItem); //アイテムインベントリのUI用にアイテムオブジェクトを生成
            inventoryUIItemObj.transform.SetParent(characterInventoryTransform.transform); //インベントリの子オブジェクトとして生成

            // 必要なら初期のローカル位置/回転を調整
            //inventoryUIItemObj.transform.localPosition = Vector3.zero;
            inventoryUIItemObj.transform.localRotation = Quaternion.identity;
            hasItemList.Add(inventoryUIItemObj);     
        }

        public void RotateItemInventory(List<ItemInfomation> _hasItemList, RotateType rotateType)
        {
            if (_hasItemList == null || _hasItemList.Count == 0) return;
            if (isChangeRotate) return;   //回転中ならリクエストを受けない
            isChangeRotate = true;
            // 1アイテムあたりの角度（360°をアイテム数で割る）
            float anglePerItem = 360f / _hasItemList.Count;
            currentRotateType = rotateType;

            if (currentRotateType == RotateType.CLOCK_WISE)
            {
                targetAngle += anglePerItem;
            }
            else
            {
                targetAngle -= anglePerItem;
            }

        }

        void UpdateRotateInventory()
        {
            if (characterInventoryTransform == null) return;
            if (!isChangeRotate) return;   //回転リクエストがないときは処理しない

            // 現在の回転角度
            float currentAngle = characterInventoryTransform.localEulerAngles.y;

            float deltaRotate = ROTATE_SPEED * Time.deltaTime;

            if (currentRotateType == RotateType.CLOCK_WISE)
            {
                currentAngle += deltaRotate;
            }
            else
            {
                currentAngle -= deltaRotate;
            }

            // targetAngle に近づきすぎたら補正
            float angleDiff = Mathf.DeltaAngle(currentAngle, targetAngle);
            if (Mathf.Abs(angleDiff) < deltaRotate)
            {
                currentAngle = targetAngle;
                isChangeRotate = false;   //回転終了
            }

            characterInventoryTransform.localEulerAngles = new Vector3(0, currentAngle, 0);
        }

        public void ShowItemName(string _itemName)  //ホールドしているアイテム名をセット
        {
            itemName.text = _itemName;
        }

        public void RotateCurrentItem() //選択中のアイテムをインベントリで回転させる
        {
            if (isChangeRotate) return;   //アイテム変更による回転演出中は処理を返す
            if (hasItemList == null || hasItemList.Count == 0) return;    //空ならリターン

            
            //hasItemList[currentListIndex].transform.Rotate(new Vector3(0, 1, 0));
        }

        // 円形に等間隔でアイテムを配置する。手前のものから反時計回り順にリストの昇順
        public void ArrangeItemsCircle(int _currentHoldIndex)
        {
            int childCount = characterInventoryTransform.childCount;

            if (childCount == 0) return;


            float step = 360f / childCount;
            for (int i = 0; i < childCount; i++)
            {
                Transform child = characterInventoryTransform.GetChild(i);
                //ホールドしているアイテムが手前になるようにする
                int relativeIndex = (i - _currentHoldIndex + childCount) % childCount;
                float angleDeg = step * relativeIndex;  // 0度がホールドアイテム
                float rad = angleDeg * Mathf.Deg2Rad;   //度数からラジアンに変換

                // ローカル座標で円周上に配置(0度の時が手前(0,0,-1)になるように)
                float z = -Mathf.Cos(rad) * radius;
                float x = Mathf.Sin(rad) * radius;

                Vector3 localPos = new Vector3(x, yOffset, z);
                //Debug.Log(new Vector3(x, 0, z));
                child.localPosition = localPos;

                // // 中心を向かせたいとき（見た目を中心向きに）
                // if (faceCenter)
                // {
                //     // child.LookAt はワールド座標を使うので中心のワールド位置を指定
                //     child.LookAt(characterInventoryTransform.position);
                //     // 向きが上下に傾くのを防ぎたい場合は Y 軸のみで向ける補正を行う
                //     Vector3 euler = child.localEulerAngles;
                //     euler.x = 0f; // 上下回転をリセット（必要に応じて調整）
                //     child.localEulerAngles = euler;
                // }
            }
        }
    }
}

