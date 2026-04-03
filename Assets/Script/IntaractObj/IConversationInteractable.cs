using System.Collections;
using UnityEngine;

/// <summary>
/// 会話相手のインターフェイス
/// </summary>
public interface IConversationInteractable : IInteractedObj
{
    int ConversationID { get; }         //会話データのID
    Transform CurrentTransform { get; }
    public IEnumerator LookTowardPlayer(Transform _player);
}
