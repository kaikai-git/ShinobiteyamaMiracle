using Character;
using UI.Converseation;
using UnityEngine;
using UnityEngine.InputSystem; //入力で使用

namespace Player
{
    //プレイヤーからの入力値を具体的なアクションに変換するクラス
    public class PlayerInputHandler : MonoBehaviour
    {
        PlayerStateHandler playerStateHandler;
        CharacterMover characterMover;
        ItemPicker itemPicker;
        InventoryManager inventoryManager;
        InteractHandler interactHandler;

        PlayerInput playerInput;
        public enum EnableState
        {
            ENABLE,
            DISABLE
        }

        void OnEnable()
        {
            // イベントはここで一度だけ登録する（重複防止）
            // Explore アクションマップ
            playerInput.Explore.Move.performed += OnMove;
            playerInput.Explore.Move.canceled  += OnMoveCanceled;
            playerInput.Explore.RotateCam.performed += OnRotate;
            playerInput.Explore.SwitchHoldItem.performed += OnSwitchHoldItem;
            playerInput.Explore.OpenInventory.performed += OnOpenInventory;
            playerInput.Explore.Interact.performed += OnPickItem;

            // OpenInventory アクションマップ
            playerInput.OpenInventory.CloseInventory.performed += OnCloseInventory;
            playerInput.OpenInventory.RotateItemInventory.performed += OnRotateInventory;

            // Conversation アクションマップ
            playerInput.Conversation.Interaction.performed += OnAdvanceConversation;

            //ReadingDocumentアクションマップ
            playerInput.ReadingDocument.CloseReading.performed += OnCloseReading;


        }

        void OnDisable()
        {
            // 登録解除（OnEnable と対応）
            // Explore アクションマップ
            playerInput.Explore.Move.performed -= OnMove;
            playerInput.Explore.Move.canceled  -= OnMoveCanceled;
            playerInput.Explore.RotateCam.performed -= OnRotate;
            playerInput.Explore.SwitchHoldItem.performed -= OnSwitchHoldItem;
            playerInput.Explore.OpenInventory.performed -= OnOpenInventory;
            playerInput.Explore.Interact.performed -= OnPickItem;

            // OpenInventory アクションマップ
            playerInput.OpenInventory.CloseInventory.performed -= OnCloseInventory;
            playerInput.OpenInventory.RotateItemInventory.performed -= OnRotateInventory;

            // Conversation アクションマップ
            playerInput.Conversation.Interaction.performed -= OnAdvanceConversation;

            // Conversation アクションマップ
            playerInput.Conversation.Interaction.performed -= OnAdvanceConversation;

            //ReadingDocumentアクションマップ
            playerInput.ReadingDocument.CloseReading.performed -= OnCloseReading;
        }



        void Awake()
        {
            // Actionスクリプトのインスタンス生成
            playerInput = new PlayerInput();
        }
        void Start()
        {

            //playerInput.Enable();   //InputAction有効化
            playerStateHandler = GetComponent<PlayerStateHandler>();
            characterMover = GetComponent<CharacterMover>();

            itemPicker = GetComponent<ItemPicker>();
            inventoryManager = GetComponent<InventoryManager>();
            interactHandler = GetComponent<InteractHandler>();
            //cameraManager = GetComponent<CameraManager>();

            // カーソルを非表示＆ロック
            Cursor.lockState = CursorLockMode.Locked;   // カーソルを画面中央に固定
            Cursor.visible = false;                     // カーソルを非表示にする
        }

       

        public void SwitchEnableExploreAction(EnableState enableState)
        {
            if (enableState == EnableState.ENABLE)
            {
                // アクションマップ全体を有効化
                playerInput.Explore.Enable();
            }
            else if (enableState == EnableState.DISABLE)
            {
                // アクションマップを無効化
                playerInput.Explore.Disable();
            }
        }

        public void SwitchEnableOpenInventoryAction(EnableState enableState)
        {
            if (enableState == EnableState.ENABLE)
            {
                //インベントリアクションを有効化
                playerInput.OpenInventory.Enable();
            }
            else if (enableState == EnableState.DISABLE)
            {
                //インベントリアクションを無効化
                playerInput.OpenInventory.Disable();
            }
        }

        public void SwitchEnableConversationAction(EnableState enableState)
        {
          
            if (enableState == EnableState.ENABLE)
            {
                //会話アクションを有効化
                playerInput.Conversation.Enable();
            }
            else if (enableState == EnableState.DISABLE)
            {
                //会話アクションを無効化
                playerInput.Conversation.Disable();
            }
        }

        public void SwitchEnableReadingDocumentAction(EnableState enableState)
        {
            if (enableState == EnableState.ENABLE)
            {
                // アクションマップ全体を有効化
                playerInput.ReadingDocument.Enable();

                Cursor.lockState = CursorLockMode.Confined;   // カーソルを画面枠内では自由に操作
                Cursor.visible = true;                       // カーソルを表示にする
            }
            else if (enableState == EnableState.DISABLE)
            {
                // アクションマップを無効化
                playerInput.ReadingDocument.Disable();

                Cursor.lockState = CursorLockMode.Locked;   // カーソルを画面中央に固定
                Cursor.visible = false;                     // カーソルを非表示にする
            }
        }
        void OnMove(InputAction.CallbackContext context)
        {
            Vector2 inputMove = context.ReadValue<Vector2>();    //移動に使用する値を取得
            characterMover.SetMove(inputMove); //キャラクターの移動
        }
        void OnMoveCanceled(InputAction.CallbackContext context)
        {
            characterMover.SetMove(Vector2.zero);
        }

        void OnRotate(InputAction.CallbackContext context)
        {
            //todo ゲーム開始時に変な方向に向いてしまうので、他のマウスでも検証 ビルドしたらいけてた、、

            Vector2 inputRotate = context.ReadValue<Vector2>();//回転に使用する値を取得
            Manager.CameraManager.Instance.RotateExploreCam(inputRotate);//カメラの回転
        }

        void OnSwitchHoldItem(InputAction.CallbackContext context)
        {
            Vector2 inputScroll = context.ReadValue<Vector2>();
            inventoryManager.ChangeHoldItem(inputScroll);//所持アイテムの変更
        }

        private void OnPickItem(InputAction.CallbackContext context)    //アイテムを拾う時
        {
            if (itemPicker != null)
            {
                itemPicker.GetItem();
                interactHandler.IntaractObj();
            }
        }

        private void OnAdvanceConversation(InputAction.CallbackContext context)    //会話時のアクション
        {
            ConverseUIManager.instance.OnAdvanceConversation();
            if(ConverseUIManager.instance.IsLastConversation())
            {
                playerStateHandler.ChangeState(PlayerStateHandler.PlayerState.EXPLORE);
            }
        }

        private void OnOpenInventory(InputAction.CallbackContext context)   //アイテムインベントリ表示
        {
            playerStateHandler.ChangeState(PlayerStateHandler.PlayerState.OPEN_INVENTORY);
        }

        private void OnCloseInventory(InputAction.CallbackContext context)   //アイテムインベントリ表示
        {
            playerStateHandler.ChangeState(PlayerStateHandler.PlayerState.EXPLORE);
        }

        private void OnRotateInventory(InputAction.CallbackContext context)   //アイテムインベントリを回転
        {

            float value = context.ReadValue<float>(); // -1 or +1
            inventoryManager.RotateInventory(value);

        }

        
        private void OnCloseReading(InputAction.CallbackContext context)
        {
            playerStateHandler.ChangeState(PlayerStateHandler.PlayerState.EXPLORE);

        }
    }
}

