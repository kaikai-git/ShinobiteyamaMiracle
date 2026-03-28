using UnityEngine;

/// <summary>
/// ゲームシーンの管理クラス
/// </summary>
public class GameManager : MonoBehaviour
{
    public void Start()
    {
        UI.Document.DocumentUIManager.Instance.InitSingleton();      //DocumentUIをリセット
        //UI.Document.DocumentUIManager.Instance.SetDocumentUI(1);      //DocumentUIをセット

    }
}
