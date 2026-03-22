using UnityEngine;

public class Document : MonoBehaviour, IDocumentInteractable
{
    InteractedObjType interactedObjType = InteractedObjType.DOCUMENT;
    public InteractedObjType InteractedObjType => interactedObjType;

    [SerializeField] int documentID;
    public int DocumentID => documentID;

    public void OnInteracted()
    {
       
    }
}
