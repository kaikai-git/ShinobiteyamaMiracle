using System.Collections;
using UnityEngine;

public class OldWoman : MonoBehaviour, IConversationInteractable
{
    public InteractedObjType InteractedObjType => InteractedObjType.CONVERSATION;
    public int ConversationID => conversationID;
    [SerializeField] int conversationID = 3;

    public Transform CurrentTransform => transform;


    private float rotationSpeed = 100.0f;

  
    public void OnInteracted()
    {
        //currentTransform = this.transform;
    }

    

    //一定時間をかけてプレイヤーの方向を向かせる。
    public IEnumerator LookTowardPlayer(Transform _playerPos)
    {
        // プレイヤーの方向を計算
        Vector3 directionToPlayer = (_playerPos.position - transform.position).normalized;
        directionToPlayer.y = 0; 
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

        // 一定角度まで回転する
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null; // 1フレーム待つ
        }

        // 最終的にぴったり向かせる
        transform.rotation = targetRotation;
    }
}


