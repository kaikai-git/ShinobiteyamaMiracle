using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI.Converseation
{ 
    /// <summary>
    /// 会話を行う際のUI処理を行うシングルトンクラス
    /// </summary>
    public class ConverseUIManager : SingletonBase<ConverseUIManager>
    {
        [SerializeField] ConversationDataBase conversationDataBase;     //会話文のデータベース
        [SerializeField] TextMeshProUGUI converseationText;
        bool isTypewritingText = false;                 //現在タイプライタ表示中かどうか
        const float typewriteDelay = 0.05f;             //タイプライタ表示で次の文字が表示されるまでの間隔

        List<string> currentConversationDataList;           //現在の会話データのリスト
        int currentSentenceIndex = 0;                   //現在の文章の番号。

        Coroutine _someCoroutine;

        [SerializeField] GameObject conversationPanel;



        /// <summary>
        /// 初期化処理
        /// </summary>
        public override void InitSingleton()
        {
            UnSetConversation();    //初期化
        }


        public void SetConversation(int _conversationID)
        {

            conversationPanel.SetActive(true);

            currentSentenceIndex = 0;

            //会話の文章を取得して追加
            ConversationData conversation = conversationDataBase.GetConversationByID(_conversationID);

            currentConversationDataList = conversation.Conversations;

            _someCoroutine = StartCoroutine(TypewriterText(currentConversationDataList[currentSentenceIndex]));
        }

        public void UnSetConversation()
        {
            converseationText.text = "";            //空文字でテキストを初期化
            currentSentenceIndex = 0;               //センテンス番号を初期化
            currentConversationDataList = null;     //会話データを空に
            conversationPanel.SetActive(false);     //会話文UIパネルを非表示
        }


        IEnumerator TypewriterText(string text) //1文字ずつ表示
        {
            isTypewritingText = true;
            converseationText.text = "";

            foreach (char letter in text.ToCharArray())
            {
                converseationText.text += letter;
                yield return new WaitForSeconds(typewriteDelay);
               
            }

            isTypewritingText = false;

        }

        /// <summary>
        /// 現在の文章が会話文の最後の文章かどうか
        /// </summary>
        /// <returns></returns>
        public bool IsLastConversation()   
        {
            bool isLastConversation = false;
            if(currentSentenceIndex == currentConversationDataList.Count)
            {
                return true;
            }
            return isLastConversation;
        }

        /// <summary>
        /// 入力に対して、文字を完全表示させる or　次の文章に進める
        /// </summary>
        public void OnAdvanceConversation()
        {
            if(isTypewritingText) //タイプライト表示中に入力があれば全文を表示させる
            {
               
                StopCoroutine(_someCoroutine);
                isTypewritingText = false;

                converseationText.text = currentConversationDataList[currentSentenceIndex];
                
            }
            //全文表示していれば次の文章に進む
            else                 
            {
                currentSentenceIndex++;
                if (currentSentenceIndex < currentConversationDataList.Count) //タイプライト表示が終わっている時に入力があれば次の会話文に
                {
                    _someCoroutine = StartCoroutine(TypewriterText(currentConversationDataList[currentSentenceIndex]));
                }
            }
        }

      
    }
}

