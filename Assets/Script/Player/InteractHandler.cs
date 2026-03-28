using UI.Converseation;
using UnityEngine;
//using static PlayerStateHandler;

namespace Player
{
    //プレイヤーのゲーム内のオブジェクトへの干渉を扱うクラス。
    public class InteractHandler : MonoBehaviour
    {
        private Ray characterRay;
        [SerializeField] private float characterReach = 2.5f;
        [SerializeField] float heightOffset;

        PlayerStateHandler stateHandler;
        void Start()
        {
            stateHandler = GetComponent<PlayerStateHandler>();
        }


        public void IntaractObj()
        {
            //レイの発射と描画
            characterRay.origin = transform.position + heightOffset * Vector3.up;
            characterRay.direction = transform.forward;
            RaycastHit hit;
            Debug.DrawRay(characterRay.origin, characterRay.direction * characterReach, Color.red, 1.0f);

            if (Physics.Raycast(characterRay, out hit, characterReach))
            {
                IInteractedObj intaractedObj = hit.collider.GetComponent<IInteractedObj>();
                if (intaractedObj == null) return;

                intaractedObj.OnInteracted();
                SetInteractBehavie(intaractedObj);
            }
        }

        /// <summary>
        /// 各干渉可能オブジェクトに振れたとの処理
        /// </summary>
        /// <param name="interactedObj">干渉オブジェクトの種類</param>
        public void SetInteractBehavie(IInteractedObj interactedObj)
        {
            InteractedObjType interactedObjType = interactedObj.InteractedObjType;
            switch (interactedObjType)
            {
                case InteractedObjType.ITEM:

                    break;

                case InteractedObjType.CONVERSATION:
                    stateHandler.ChangeState(PlayerStateHandler.PlayerState.CONVERSE);
                    var conversationTarget = interactedObj as IConversationInteractable;  //asを使えば、キャスト出来なかったら安全にNullを返すらしい

                    //Player側でNPCの回転コルーチン呼び出すのは違和感ある
                    StartCoroutine(conversationTarget.LookTowardPlayer(this.transform));

                    Manager.CameraManager.Instance.SetLookTarget(conversationTarget.CurrentTransform);
                    ConverseUIManager.Instance.SetConversation(conversationTarget.ConversationID);
                    break;

                case InteractedObjType.DOCUMENT:
                    stateHandler.ChangeState(PlayerStateHandler.PlayerState.ReadiingDocument);
                    var interactDocument = interactedObj as IDocumentInteractable;  //asを使えば、キャスト出来なかったら安全にNullを返すらしい

                    UI.Document.DocumentUIManager.Instance.SetDocumentUI(interactDocument.DocumentID);
                    break;
            }
        }

    }

}

