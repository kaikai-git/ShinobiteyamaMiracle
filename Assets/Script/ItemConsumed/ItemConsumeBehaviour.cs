using UnityEngine;
using ItemConsumed;
using UnityEngine.UI;
using Player;
/// <summary>
/// アイテムを使用されるオブジェクトの共通処理を行う基底クラス
/// </summary>
public class ItemConsumeBehaviour : MonoBehaviour,IItemConsumed
{
    [SerializeField] Collider detectionCollider;    //プレイヤ―との接触を検知用コライダー
    [SerializeField] Image suggestImage;            //干渉できるサジェストUI
    bool isHit = false;                             //コライダーにプレイヤーが接触中か

    public InteractedObjType InteractedObjType => InteractedObjType.ITEM_CONSUMED;
        
   // [SerializeField] int CanConsumedItemID;
    bool isItemConsumed = false;        //アイテム消費したかのフラグ
    public int CanConsumedItemID => canConsumedItemID;  //このIDのアイテムをホールドしている状態で触れるとイベント発火
    [SerializeField] int canConsumedItemID;


    void Awake()
    {
        if (detectionCollider != null)
        {
            TriggerProxy proxy = detectionCollider.gameObject.GetComponent<TriggerProxy>();

            // detectionColliderがあるオブジェクトにTriggerProxyないなら追加して、通知を受け取る
            if (proxy == null)
            {
                proxy = detectionCollider.gameObject.AddComponent<TriggerProxy>();
            }

            //プレイヤーとの接触状況に応じての処理の関数を登録
            proxy.OnEntered = HandleTriggerEnter;
            proxy.OnStay = HandleTriggerStay;
            proxy.OnExit = HandleTriggerExit;

        }

        suggestImage.enabled = false;
    }

    /// <summary>
    /// 触れた際の処理
    /// </summary>
    public void OnInteracted()
    {
        //消費していたら処理しない
        if (isItemConsumed) return;

        int? currentID = InventoryManager.Instance.GetHoldItemID();

        //該当アイテムIDをホールドしていなければ処理しない
        if (currentID == null || currentID != canConsumedItemID) return;
        isItemConsumed = true;
        //継承先での各々の処理
        OnInteractedInherit();
    }

    /// <summary>
    /// 継承先での処理
    /// </summary>
    virtual protected void OnInteractedInherit()
    {

    }


    /// <summary>
    ///検知時の共通処理　UIを表示
    /// </summary>
    /// <param name="other"></param>
    private void HandleTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isHit = true;
            suggestImage.enabled = true;

        }
    }

    /// <summary>
    ///接触時の共通処理　ビルボード処理
    /// </summary>
    /// <param name="other"></param>
    private void HandleTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            suggestImage.transform.forward = Camera.main.transform.forward;

            Vector3 targetPos = Camera.main.transform.position;
            targetPos.y = suggestImage.transform.position.y;
            suggestImage.transform.LookAt(targetPos);
        }
    }


    /// <summary>
    ///検知外の共通処理 UIを非表示
    /// </summary>
    /// <param name="other"></param>
    private void HandleTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isHit = false;
            suggestImage.enabled = false;

        }
    }

    private void OnDrawGizmos()
    {
        // detectionColliderがセットされていない場合は何もしない
        if (detectionCollider == null) return;

        //コライダーのトランスフォームをギズモに適用する
        Gizmos.matrix = detectionCollider.transform.localToWorldMatrix;

        // ギズモの色を設定（
        // 発火前は緑、発火後は赤にするなどの工夫も可能
        Gizmos.color = isHit ? new Color(1, 0, 0, 0.6f) : new Color(0, 1, 0, 0.6f);

        //コライダーの種類に応じて形状を描画する
        if (detectionCollider is BoxCollider box)
        {
            // 塗りつぶしの立方体
            Gizmos.DrawCube(box.center, box.size);
            // 枠線（枠線も描くと形がはっきりします）
            Gizmos.DrawWireCube(box.center, box.size);
        }
        else if (detectionCollider is SphereCollider sphere)
        {
            // 球体
            Gizmos.DrawSphere(sphere.center, sphere.radius);
        }
    }
}
