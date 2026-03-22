using UnityEngine;



//Sound関連の管理クラス（シングルトン）
public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;


    [SerializeField] AudioSource seAudioSource2D;     //2DSE用
    [SerializeField] AudioSource bgmAudioSource2D;    //2DBGM用
        

    private void Awake()
    {
        if (instance != null && instance.gameObject != null)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static SoundManager Instance
    {
        get
        {
            if (instance == null)   //インスタンスが無ければシーン内のオブジェクトを探す
            {
                instance = FindObjectOfType<SoundManager>();

                if (instance == null)   //それでも無ければ生成する
                {
                    var prefab = Resources.Load<SoundManager>("SoundManager");
                    instance = Instantiate(prefab);
                }
            }
            return instance;
        }
    }


    //SEを再生

    public void PlaySE(AudioClip audioClip)
    {
        seAudioSource2D.PlayOneShot(audioClip);
    }

    //BGmを再生
    public void PlayBGM(AudioClip audioClip)
    {
        bgmAudioSource2D.clip = audioClip;
        bgmAudioSource2D.Play();
    }

    //BGMを停止
    public void StopBgm()
    {
        bgmAudioSource2D.Stop();
    }

    //BGMを中断       
    public void PauseBgm()
    {
        bgmAudioSource2D.Pause();
    }


    //音量変更(0:最少 1:最大）
    public void SetBGMVolume(float _volume)
    {
        bgmAudioSource2D.volume = _volume;
    }

    public void SetSEVolume(float _volume)
    {
        seAudioSource2D.volume = _volume;
    }

    //各3D音源でボリューム調整で必要
    public float GetSEVolume()
    {
       return seAudioSource2D.volume;
    }
}
