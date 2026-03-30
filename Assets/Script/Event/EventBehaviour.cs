using Unity.VisualScripting;
using UnityEngine;

public class EventBehaviour : MonoBehaviour
{
    [SerializeField] Collider detectionCollider;    //プレイヤ―との接触を検知用コライダー

    bool isFired = false;   //イベントが発火したかのフラグ

    void Awake()
    {
        if (detectionCollider != null)
        {
            if (detectionCollider.gameObject.GetComponent<TriggerProxy>() == null)
            {
                // detectionColliderがあるオブジェクトにProxyを貼って、通知を受け取る
                var proxy = detectionCollider.gameObject.AddComponent<TriggerProxy>();
                proxy.OnEntered = HandleTriggerEnter;
            }
        }
    }

   /// <summary>
   /// 発火時の共通処理
   /// </summary>
   /// <param name="other"></param>
    private void HandleTriggerEnter(Collider other)
    {
        if (isFired) return;

        if (other.CompareTag("Player"))
        {
            isFired = true;
            ExecuteEvent();
        }
    }
    /// <summary>
    /// 発火時の具体的なイベント処理継承先で処理内容記述
    /// </summary>
    protected virtual void ExecuteEvent()
    {
        Debug.Log("Event Fired!");
    }

    private void OnDrawGizmos()
    {
        // detectionColliderがセットされていない場合は何もしない
        if (detectionCollider == null) return;

        //コライダーのトランスフォームをギズモに適用する
        Gizmos.matrix = detectionCollider.transform.localToWorldMatrix;

        // ギズモの色を設定（
        // 発火前は緑、発火後は赤にするなどの工夫も可能
        Gizmos.color = isFired ? new Color(1, 0, 0, 0.3f) : new Color(0, 1, 0, 0.3f);

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
