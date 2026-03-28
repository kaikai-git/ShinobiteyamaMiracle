using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;


[System.Serializable]

public class DocumentData 
{
    [SerializeField] int documentID;                 //ドキュメントのID
    public int DocumentID => documentID;

    [SerializeField] LocalizedString documentName; // String Tableの「名前」への参照
    public string DocumentName => documentName.GetLocalizedString();

    // 各ページのテキストをリストで持つ
    public List<LocalizedString> pages;

    // 指定したページのテキストを返す
    public string GetPageContent(int index)
    {
        if (index >= 0 && index < pages.Count)
        {
            return pages[index].GetLocalizedString();
        }
        return "";
    }
}
