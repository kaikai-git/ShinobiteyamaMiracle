using UnityEngine;


//シングルトンの基底クラス。継承先ですぐシングルトンを使える
public class SingletonBase<T> : MonoBehaviour where T : MonoBehaviour   //whererはT は MonoBehaviour、またはその子クラスでなければならない
{

    private static T instance;

    //唯一のインスタンスにアクセスするためのプロパティ
    public static T Instance
    {
        //取得に関する内容
        get
        {
            //インスタンスがまだない場合
            if (instance == null)
            {
                //シーンから既存のインスタンスを探す。
                instance = FindObjectOfType<T>();

                //それでも見つからない場合
                if(instance == null)
                {
                    //新しいGameObjectを作成し、T型のコンポーネントを追加する。
                    GameObject singletonObj = new GameObject(typeof(T).Name);
                    instance = singletonObj.AddComponent<T>();
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        //既にインスタンスが存在する場合(重複して作られそうになった場合）
        if (instance != null && instance != this)
        {
            //この重複したインスタンスを破棄する
            Destroy(instance);
            return;
        }

        //このインスタンスを唯一のものとして設定
        instance = this as T;

        //このシーンが切り替わっても破棄されないように設定
        DontDestroyOnLoad(gameObject);

        //必要に応じて、初期化処理を呼び出す
        Init();
    }

    protected virtual void Init()
    {
        //子クラスで初期化処理を記述
    }
}
