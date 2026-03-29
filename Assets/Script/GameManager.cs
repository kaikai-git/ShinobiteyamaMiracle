using UnityEngine;

/// <summary>
/// ゲームシーンの管理クラス
/// </summary>
public class GameManager : MonoBehaviour
{
    public void Start()
    {
        //この処理はUIManager作って移すべきかも
        UI.Document.DocumentUIManager.Instance.InitSingleton();           //DocumentUIをリセット
        UI.Converseation.ConverseUIManager.Instance.InitSingleton();      //会話文のUIをセット

    }
}
