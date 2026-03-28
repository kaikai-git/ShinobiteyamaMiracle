using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Converseation
{ 
    /// <summary>
    /// 会話を行う際のUI処理を行うシングルトンクラス
    /// </summary>
    public class ConverseUIManager : SingletonBase<ConverseUIManager>
    {
        //[SerializeField] ConversationData testdata;     // データを格納
        [SerializeField] Text converseationText;
        bool isTypewritingText = false;                 //現在タイプライタ表示中かどうか
        const float typewriteDelay = 0.05f;             //タイプライタ表示で次の文字が表示されるまでの間隔
        List<string> currentConversattionData;
        int currentIndex = 0;

        Coroutine _someCoroutine;

        [SerializeField] GameObject conversationPanel;

   
        void Start()
        {
            UnSetConversation();
            // ID=1 の会話データを取得
            //var convList = GetConversationTextFromID(1);

            //// データが存在しない場合
            //if (convList == null || convList.Count == 0)
            //{
            //    Debug.LogWarning("会話データが見つかりませんでした（id=1）");
            //    return;
            //}

            //// 1件ずつ出力
            //Debug.Log($"=== 会話ID: 1 の内容 ===");
            //for (int i = 0; i < convList.Count; i++)
            //{
            //    Debug.Log($"[{i}] {convList[i]}");
            //}
        }


        //public static List<string> CollectConversations(ConversationData row)
        //{
        //    List<string> list = new List<string>();

        //    foreach (var field in typeof(ConversationData).GetFields(BindingFlags.Public | BindingFlags.Instance))
        //    {
        //        // id は数値なのでスキップ
        //        if (field.FieldType != typeof(string)) continue;

        //        string value = field.GetValue(row) as string;
        //        if (!string.IsNullOrEmpty(value))
        //            list.Add(value);
        //    }

        //    return list;
        //}


        //IDから文字列を返す。
        //List<string> GetConversationTextFromID(int id)
        //{
        //    if (testdata == null || testdata.JapaneseConversationData == null)
        //        return new List<string>();

        //    // id は配列の index とは別（id フィールドに依存）なので検索する
        //    foreach (var row in testdata.JapaneseConversationData)
        //    {
        //        // row が null ならスキップ
        //        if (row == null) continue;

        //        // row.id の型が int である想定
        //        var idField = row.GetType().GetField("id", BindingFlags.Public | BindingFlags.Instance);
        //        if (idField != null)
        //        {
        //            object idVal = idField.GetValue(row);
        //            if (idVal is int rowId && rowId == id)
        //            {
        //                // 該当行が見つかったらその行から全ての文字列フィールドを取り出す
        //                return CollectStringFieldsFromRow(row);
        //            }
        //        }
        //        else
        //        {
        //            // もし row.id がプロパティになっている場合の安全処理
        //            var idProp = row.GetType().GetProperty("id", BindingFlags.Public | BindingFlags.Instance);
        //            if (idProp != null)
        //            {
        //                object idVal = idProp.GetValue(row, null);
        //                if (idVal is int rowId && rowId == id)
        //                    return CollectStringFieldsFromRow(row);
        //            }
        //        }
        //    }

        //    // 見つからなければ空リスト
        //    return new List<string>();
        //}

        // 行オブジェクトから string 型のフィールドを宣言順（ソースの並び順）で取り出すユーティリティ
        List<string> CollectStringFieldsFromRow(object row)
        {
            var list = new List<string>();
            if (row == null) return list;

            var type = row.GetType();

            // フィールドを取得して MetadataToken（宣言順に近い安定した順序）でソート
            var stringFields = type
                .GetFields(BindingFlags.Public | BindingFlags.Instance)
                .Where(f => f.FieldType == typeof(string))
                .OrderBy(f => f.MetadataToken); // 宣言順に近い順序

            foreach (var f in stringFields)
            {
                string value = f.GetValue(row) as string;
                if (!string.IsNullOrEmpty(value))
                    list.Add(value);
            }

            // もしプロパティで string を持っている列があるなら追加で処理（オプション）
            var stringProps = type
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.PropertyType == typeof(string))
                .OrderBy(p => p.MetadataToken);

            foreach (var p in stringProps)
            {
                string value = p.GetValue(row, null) as string;
                if (!string.IsNullOrEmpty(value))
                    list.Add(value);
            }

            return list;
        }
                        
        public void SetConversation(int _conversationID)
        {

            conversationPanel.SetActive(true);

            // ID=1 の会話データを取得
           // currentConversattionData = GetConversationTextFromID(_conversationID);
            currentIndex = 0;

            _someCoroutine = StartCoroutine(TypewriterText(currentConversattionData[currentIndex]));
        }

        public void UnSetConversation()
        {
            converseationText.text = "";    //空文字でテキストを初期化
            conversationPanel.SetActive(false);
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

        public bool IsLastConversation()   
        {
            bool isLastConversation = false;
            if(currentIndex == currentConversattionData.Count)
            {
                return true;
            }
            return isLastConversation;
        }

        public void OnAdvanceConversation()
        {
            if(isTypewritingText) //タイプライト表示中に入力があれば全文を表示させる
            {
               
                StopCoroutine(_someCoroutine);
                isTypewritingText = false;

                converseationText.text = currentConversattionData[currentIndex];
                
            }
            else                 
            {
                currentIndex++;
                if (currentIndex < currentConversattionData.Count) //タイプライト表示が終わっている時に入力があれば次の会話文に
                {
                    _someCoroutine = StartCoroutine(TypewriterText(currentConversattionData[currentIndex]));
                }
            }
        }

      
    }
}

