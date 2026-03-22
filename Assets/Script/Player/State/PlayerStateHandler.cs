using System.Collections.Generic;
using Player;
using UnityEngine;

//プレイヤーの状態を管理するクラス
public class PlayerStateHandler : MonoBehaviour
{
    Dictionary<PlayerState, IPlayerState> playerStateDictionary;   // プレイヤーの状態と各ステートの処理を対応付ける辞書

    PlayerState currentState = PlayerState.EXPLORE; //現在のプレイヤーのステート、デフォルトはExplore

    [SerializeField] InventoryManager inventoryManager;
    PlayerInputHandler inputHandler;

    void Start()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
        // 辞書を初期化
        playerStateDictionary = new Dictionary<PlayerState, IPlayerState>
        {
            { PlayerState.EXPLORE, new ExploreState(inputHandler) },
            { PlayerState.OPEN_INVENTORY, new OpenInventoryState(inventoryManager,inputHandler) },
            { PlayerState.CONVERSE, new ConversationState(inputHandler) },
            { PlayerState.ReadiingDocument, new DocumentReadingState(inputHandler) },
        };

        playerStateDictionary[currentState].EnterState();   //デフォルトステートの初期化を行う
    }

    void Update()
    {
        playerStateDictionary[currentState].UpdateState();  //現在のステートの更新を行う
    }

    public void ChangeState(PlayerState newStateType)
    {
        playerStateDictionary[currentState].ExitState();    //現在のステートの後始末を行う
        currentState = newStateType;                        //ステートを更新する
        playerStateDictionary[currentState].EnterState();   //新しいステートの初期化を行う
    }

    public enum PlayerState //プレイヤーのステート
    {
        EXPLORE,        //通常のゲーム状態
        OPEN_INVENTORY, //インベントリを開いている状態
        CONVERSE,       //会話中の状態
        ReadiingDocument,//テキストを読む
    }

}
