/// <summary>
/// アイテムの使用先のインターフェイス
/// </summary>
public interface IItemConsumed : IInteractedObj
{
    int CanConsumedItemID { get; }  //使用できるアイテムID
}

