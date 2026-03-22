using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Document
{
    public class DocumentUIManager : MonoBehaviour
    {
        [SerializeField] DocumentData documentData; //書類データ
        [SerializeField] GameObject ducumentUIPanel;


        [SerializeField] TextMeshProUGUI documentTitleText;
        [SerializeField] TextMeshProUGUI documentText;
        
        [SerializeField] Button nextPageButton;
        [SerializeField] Button backPageButton;

        public static DocumentUIManager instance;
        const string EMPTY_TEXT = "";
        int currentDocumentID = EMPTY_DOCUMENT_ID;
        const int EMPTY_DOCUMENT_ID = -1;


        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                // インスタンスが複数存在しないように、既に存在していたら自身を消去する
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            // SetDocumentUI(1);
            UnSetDocumentUI();
        }
        
       

        public void SetDocumentUI(int _documentID)
        {
            currentDocumentID = _documentID;
            Debug.Log(currentDocumentID);
            ducumentUIPanel.SetActive(true);

            documentTitleText.text =  documentData.EnglishDocumentData[_documentID].title;
            documentText.text = documentData.EnglishDocumentData[_documentID].first;

        }

        public void UnSetDocumentUI()
        {
            ducumentUIPanel.SetActive(false);
            currentDocumentID = EMPTY_DOCUMENT_ID;
            documentTitleText.text = EMPTY_TEXT;
            documentText.text = EMPTY_TEXT;
        }

        //todoエクセルからの文字をとってくる
        public void OnNextPage()
        {
            documentText.text = documentData.EnglishDocumentData[currentDocumentID].second;
        }

        public void OnBackPage()
        {
            documentText.text = documentData.EnglishDocumentData[currentDocumentID].first;
        }


    }

}


