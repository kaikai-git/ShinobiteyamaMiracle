

//プレイヤーからインタラクションされるオブジェクトのインターフェイス
public interface IInteractedObj
{
    InteractedObjType InteractedObjType { get; }

    //インタラクトされた際の振る舞い(アイテムを拾うときや、NPCに話しかけたとき)
    void OnInteracted();
}

//プレイヤーが干渉できる対象
public enum InteractedObjType
{
    ITEM_CONSUMED,  //アイテムの使用先
    CONVERSATION,   //会話相手
    DOCUMENT        //資料
}
