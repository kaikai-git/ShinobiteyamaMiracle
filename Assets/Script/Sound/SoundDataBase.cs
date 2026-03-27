using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SoundDataBase : ScriptableObject
{
    public List<SEData> seDataList = new List<SEData>();        //SEのデータリスト
    public List<BGMData> bgmDataList = new List<BGMData>();     //BGMのデータリスト

    Dictionary<SEType, SEData>seDataDic = new Dictionary<SEType, SEData>(); //SEの辞書　検索高速化
    Dictionary<BGMType, BGMData> bgmDataDic = new Dictionary<BGMType, BGMData>(); //SEの辞書　検索高速化


    //SEとBGMを登録
    public void RegistSeBgmData()
    {
        RegistDataDic(seDataList, seDataDic, d => d.seType);           //SEを登録
        RegistDataDic(bgmDataList, bgmDataDic, d => d.bgmType);        //BGMを登録
    }



    /// <summary>
    /// 辞書にデータを登録 現在は先に登録されたもの優先で重複無しで登録
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="sourceList">登録する元のソースリスト</param>
    /// <param name="targetDic">登録先の辞書型</param>
    /// <param name="keySeletcotr"></param>
    void RegistDataDic<TKey,TValue>
    (
        List<TValue>sourceList,                
        Dictionary<TKey, TValue>targetDic,      
        System.Func<TValue, TKey> keySeletcotr  
    )
    {
        targetDic.Clear();
        foreach (var data in sourceList)
        {
            if(data == null) continue;

            TKey key = keySeletcotr(data);            //どんな音か??( 決定UIかキャンセルUIか等)
            //Debug.Log($"[SoundDB] 登録完了: {key}");
            //まだ登録されていない音のenumなら登録
            if (!targetDic.ContainsKey(key))
            {
                targetDic.Add(key, data);
            }
        }

    }


    /// <summary>
    /// 指定した辞書と音の種類からaudioClipを取得
    /// </summary>
    /// <typeparam name="TKey">なんの音か</typeparam>
    /// <typeparam name="TValue">データクラスの型(SoundDataを継承している必要がある)</typeparam>
    /// <param name="dic">>検索対象となるサウンド辞書</param>
    /// <param name="key">音データが格納されている辞書。Enumをキーとしてデータを保持しているもの</param>
    /// <returns></returns>
    AudioClip GetAudioClip<TKey,TValue>(Dictionary<TKey,TValue>dic,TKey key) where TValue : SoundData
    {
        if (dic == null || !dic.ContainsKey(key))
        {
            //Debug.LogError($"SE '{soundTyoe}' が辞書に見つかりません！Registが呼ばれていないか、インスペクターの設定が漏れています。");
            return null;
        }

        return dic[key].audioClip;
    }

    //ゲッター
    public AudioClip GetSE(SEType seType) => GetAudioClip(seDataDic, seType);
    public AudioClip GetBGM(BGMType bgmType) => GetAudioClip(bgmDataDic, bgmType);

}
