using UnityEngine;

public class R : MonoBehaviour
{
    [SerializeField] ItemDataBase itemData;
    [SerializeField] DocumentDataBase documentData;

    void Start()
    {
        //SoundManager.Instance.PlaySE(SEType.CANCEL_UI);
    }

    private void Update()
    {
        //Debug.Log(itemData.GetItemById(0).ItemName);

        Debug.Log(documentData.GetItemById(0).DocumentName);

        //各ページを出力
        for (int i = 0; i < documentData.GetItemById(0).pages.Count; i++)
        {
            // インデックスを指定して内容を取得
            Debug.Log($"ページ {i + 1}: {documentData.GetItemById(0).GetPageContent(i)}");
        }

    }
}
