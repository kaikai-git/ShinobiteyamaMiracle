using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//ゲーム内の目標を示すUIを管理するクラス
public class GoalUIManager : MonoBehaviour
{
    [SerializeField] Image goalImage;
    [SerializeField] TextMeshProUGUI goalText;

    const float fadeDuration = 1.0f;    //フェードにかかる時間

    const float activeGoalTime = 3.0f;  //目標を表示しておく時間

    public enum FadeType 
    {
        FADEIN,
        FADEOUT
    }
    void Start()
    {
        StartCoroutine(PerformFade());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // UIをフェードインまたはフェードアウトさせるリクエスト
    public void RequestFadeUI()
    {
        StopAllCoroutines(); // フェード中なら一度止める
        StartCoroutine(PerformFade());
    }


    IEnumerator PerformFade()
    {
        float timer = 0f;

        // 開始・終了のアルファ値を決定
        float startAlpha = 0.0f;
        float endAlpha = 1.0f;

        // 現在の色を取得
        Color imageColor = goalImage.color;
        Color textColor = goalText.color;

        //フェードインさせる
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / fadeDuration);

            float alpha = Mathf.Lerp(startAlpha, endAlpha, t);

            imageColor.a = alpha;
            textColor.a = alpha;

            goalImage.color = imageColor;
            goalText.color = textColor;

            yield return null;
        }

        // 最終値を補正して完全に表示
        imageColor.a = endAlpha;
        textColor.a = endAlpha;
        goalImage.color = imageColor;
        goalText.color = textColor;

        //ここで指定秒数完全表示する
        yield return new WaitForSeconds(activeGoalTime);

        //またアルファ値を下げる
        startAlpha = 1f;
        endAlpha = 0f;

        timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / fadeDuration);

            float alpha = Mathf.Lerp(startAlpha, endAlpha, t);

            imageColor.a = alpha;
            textColor.a = alpha;

            goalImage.color = imageColor;
            goalText.color = textColor;

            yield return null;
        }

        // 最終値を補正して完全に非表示
        imageColor.a = endAlpha;
        textColor.a = endAlpha;
        goalImage.color = imageColor;
        goalText.color = textColor;
    }
}
