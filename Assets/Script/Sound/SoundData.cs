using UnityEngine;


public enum SEType
{
    [InspectorName("SE/UI:決定")] DECIDE_UI,
    [InspectorName("SE/UI:キャンセル")] CANCEL_UI
}

public enum BGMType
{
    [InspectorName("BGM/タイトルBGM")] TITLE_BGM,
    [InspectorName("BGM/ゲーム内BGM")] GAME_BGM
}
public class SoundData
{
   public AudioClip audioClip;

}

[System.Serializable]

public class SEData : SoundData
{
    public SEType seType;
}
[System.Serializable]

public class  BGMData : SoundData
{
    public BGMType bgmType;


}

