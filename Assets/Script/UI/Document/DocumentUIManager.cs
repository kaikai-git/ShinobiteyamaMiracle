using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Document
{
    /// <summary>
    /// 資料を読む際のUIの処理を行うシングルトンクラス
    /// </summary>
    public class DocumentUIManager : SingletonBase<DocumentUIManager>
    {
        [SerializeField] DocumentData documentData;             //書類データ
        [SerializeField] GameObject ducumentUIPanel;            //ドキュメントのUIパネル


        [SerializeField] TextMeshProUGUI documentTitleText;     //資料のタイトルテキスト
        [SerializeField] TextMeshProUGUI documentText;          //資料の内容テキスト
        
        [SerializeField] Button nextPageButton;                 //次のページに進むボタン
        [SerializeField] Button backPageButton;                 //前のページに戻るボタン

        const string EMPTY_TEXT = "";                           //空文字の定数
        int currentDocumentID = EMPTY_DOCUMENT_ID;              //現在呼んでる資料のID  
        const int EMPTY_DOCUMENT_ID = -1;                       //空状態のID


        protected override void AwakeInheritance()
        {
            //UnSetDocumentUI();    //初期化
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        public override void InitSingleton()
        {
            UnSetDocumentUI();    //初期化
        }


        /// <summary>
        /// 指定したIDから資料情報を取得
        /// </summary>
        /// <param name="_documentID"></param>
        public void SetDocumentUI(int _documentID)
        {
            UnSetDocumentUI();                  //一応初期化

            currentDocumentID = _documentID;    //資料IDを更新
            Debug.Log(currentDocumentID);
            ducumentUIPanel.SetActive(true);    //パネルを更新

            documentTitleText.text =  documentData.EnglishDocumentData[_documentID].title;  //タイトルを更新
            documentText.text = documentData.EnglishDocumentData[_documentID].first;        //内容を更新

        }

        /// <summary>
        /// 初期化の処理
        /// </summary>
        public void UnSetDocumentUI()
        {
            ducumentUIPanel.SetActive(false);       //パネルを非表示
            currentDocumentID = EMPTY_DOCUMENT_ID;  //現在呼んでいる資料のIDを無しに
            documentTitleText.text = EMPTY_TEXT;    //タイトルを空にする。
            documentText.text = EMPTY_TEXT;         //内容のテキストを空にする
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


