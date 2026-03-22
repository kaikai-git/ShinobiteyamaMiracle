
using System.Collections.Generic;
using UnityEngine;

//[ExcelAsset]
public class ConversationData : ScriptableObject
{
	public List<ConversationEntiity> JapaneseConversationData; // Replace 'EntityType' to an actual type that is serializable.
    public List<ConversationEntiity> EnglishConversationData;
}
