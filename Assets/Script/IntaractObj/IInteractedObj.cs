

//プレイヤーからインタラクションされるオブジェクトのインターフェイス
public interface IInteractedObj
{
    InteractedObjType InteractedObjType { get; }

    //インタラクトされた際の振る舞い(アイテムを拾うときや、NPCに話しかけたとき)
    void OnInteracted();
}

public enum InteractedObjType
{
    ITEM,
    CONVERSATION,
    DOCUMENT
}
