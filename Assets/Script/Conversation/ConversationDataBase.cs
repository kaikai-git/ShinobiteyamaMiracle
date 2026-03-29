using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 会話文のデータベースリスト
/// </summary>
[CreateAssetMenu]

public class ConversationDataBase : ScriptableObject
{
    public List<ConversationData> conversationDataList = new List<ConversationData>();

    public ConversationData GetConversationByID(int id)
    {
        return conversationDataList.FirstOrDefault(conversation => conversation.ConversationID == id);
    }

}
