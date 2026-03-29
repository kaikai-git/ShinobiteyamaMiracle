using System.Linq;
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
        [SerializeField] DocumentDataBase documentDatabase;             //書類データ
        [SerializeField] GameObject ducumentUIPanel;            //ドキュメントのUIパネル


        [SerializeField] TextMeshProUGUI documentTitleText;     //資料のタイトルテキスト
        [SerializeField] TextMeshProUGUI documentText;          //資料の内容テキスト
        
        [SerializeField] Button nextPageButton;                 //次のページに進むボタン
        [SerializeField] Button backPageButton;                 //前のページに戻るボタン

        const string EMPTY_TEXT = "";                           //空文字の定数
        int currentDocumentID = EMPTY_DOCUMENT_ID;              //現在呼んでる資料のID  

        int currentDocPageIndex = 0;                             //現在の書類のページ
        int currentDocMaxPage = 0;                               //現在の書類が何ページあるのか

        const int EMPTY_DOCUMENT_ID = -1;                       //空状態のID

        //ページの状態
        enum PageState
        {
            LEFT_END,       //左端
            RIGHT_END,      //右端
            NO_ENDS,        //右にも左にもいけない。（1ページしかない）
            MIDDLE_PAGE     //真ん中のページ（左にも右にもいける）
        }

        PageState pageState = PageState.MIDDLE_PAGE;

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
        /// 指定したIDから資料情報を取得、資料を取得したときに呼ばれる
        /// </summary>
        /// <param name="_documentID"></param>
        public void SetDocumentUI(int _documentID)
        {
            UnSetDocumentUI();                  //一応初期化

            currentDocumentID = _documentID;    //資料IDを更新

            ducumentUIPanel.SetActive(true);    //ドキュメントUIのパネルを表示


            //IDからドキュメント情報を取得
            DocumentData documentData = documentDatabase.GetDocumentById(_documentID);

            documentTitleText.text = documentData.DocumentName;  //タイトルを更新
            documentText.text = documentData.GetPageContent(0);  //内容を更新
            currentDocMaxPage = documentData.pages.Count;       //現在の資料の最大ページ数を取得

            SetPageState();                                     //ページ状況に応じて、送りボタンをセット
        }

        /// <summary>
        /// 初期化の処理　
        /// </summary>
        public void UnSetDocumentUI()
        {
            currentDocPageIndex = 0;                //現在の書類のページ数を0にして最初のページから読めるように
            ducumentUIPanel.SetActive(false);       //パネルを非表示
            currentDocumentID = EMPTY_DOCUMENT_ID;  //現在呼んでいる資料のIDを無しに
            documentTitleText.text = EMPTY_TEXT;    //タイトルを空にする。
            documentText.text = EMPTY_TEXT;         //内容のテキストを空にする
        }

        /// <summary>
        /// 次のページに進む ボタンにセットして使う
        /// </summary>
        public void OnNextPage()
        {
            //次のテキストに更新
            int nextPage = currentDocPageIndex += 1;
            documentText.text = documentDatabase.GetDocumentById(0).GetPageContent(nextPage);

            SetPageState();     //ページ送りボタンを更新
        }

        /// <summary>
        ///前のページに戻る　ボタンにセットして使う
        /// </summary>
        public void OnBackPage()
        {
            //前のテキストに更新
            int beforePage = currentDocPageIndex -= 1;
            documentText.text = documentDatabase.GetDocumentById(0).GetPageContent(currentDocPageIndex);

            SetPageState();      //ページ送りボタンを更新
        }

        /// <summary>
        /// ドキュメントのページからボタンの
        /// </summary>
        void SetPageState()
        {



            //ページの状態からボタンの表示非表示を設定
            SetPageButton();
        }

        /// <summary>
        /// 資料のページ状況からボタンを表示するかを設定
        /// </summary>
        void SetPageButton()
        {
            // 1ページ以上あり、かつ現在のページが最後ではないなら「次へ」を表示
            nextPageButton.gameObject.SetActive(currentDocMaxPage > 1 && currentDocPageIndex < currentDocMaxPage - 1);

            // 現在のページが 0 より大きいなら「前へ」を表示
            backPageButton.gameObject.SetActive(currentDocPageIndex > 0);


            //// 一旦両方を非表示にする
            //nextPageButton.gameObject.SetActive(false);
            //backPageButton.gameObject.SetActive(false);

            //switch (pageState)
            //{
            //    case PageState.NO_ENDS:
            //        // 両方 false なので何もしない
            //        break;

            //    case PageState.LEFT_END:
            //        nextPageButton.gameObject.SetActive(true);
            //        break;

            //    case PageState.RIGHT_END:
            //        backPageButton.gameObject.SetActive(true);
            //        break;

            //    case PageState.MIDDLE_PAGE:
            //        nextPageButton.gameObject.SetActive(true);
            //        backPageButton.gameObject.SetActive(true);
            //        break;
            //}
        }
    }

}


