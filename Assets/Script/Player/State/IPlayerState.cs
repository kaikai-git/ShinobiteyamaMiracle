namespace Player
{
    // プレイヤーの各ステートを表すインターフェイス
    interface IPlayerState
    {
        void EnterState();  //ステートに入った時に呼ばれる関数
        void UpdateState(); //ステート中に毎フレーム呼ばれる関数
        void ExitState();   //ステートから出るときに呼ばれる関数

    }
}


