using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;


[System.Serializable]
public class ConversationData 
{
    [SerializeField] int conversationID;        //会話のID
    public int ConversationID => conversationID;

    // 各会話ののテキストをリストで持つ　
    [SerializeField] List<LocalizedString> conversations;

    // リスト内の各要素を翻訳し、List<string>に変換する
    public List<string> Conversations => conversations
        .Select(c => c.GetLocalizedString())
        .ToList();

    //// 指定したインデックスのテキストを返す（コメントアウトされていた部分の修正版）
    //public string GetPageContent(int index)
    //{
    //    if (index >= 0 && index < conversations.Count)
    //    {
    //        return conversations[index].GetLocalizedString();
    //    }
    //    return "";
    //}

}
