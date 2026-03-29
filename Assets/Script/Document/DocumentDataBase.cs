using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu]
public class DocumentDataBase : ScriptableObject
{
    public List<DocumentData> documentDataList = new List<DocumentData>();
    public DocumentData GetDocumentById(int id)
    {
        return documentDataList.FirstOrDefault(item => item.DocumentID == id);
    }
}
