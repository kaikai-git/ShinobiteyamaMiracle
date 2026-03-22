using System.Collections;
using UnityEngine;

public interface IConversationInteractable : IInteractedObj
{
    int ConversationID { get; }
    Transform CurrentTransform { get; }
    public IEnumerator LookTowardPlayer(Transform _player);
}
