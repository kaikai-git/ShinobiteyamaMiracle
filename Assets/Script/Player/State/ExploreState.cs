
//通常状態のアクション管理クラス
public class ExploreState : Player.IPlayerState
{
    Player.PlayerInputHandler playerInputHandler;


    public ExploreState(Player.PlayerInputHandler _playerInputHandler)
    {
        playerInputHandler = _playerInputHandler;
    }
    public void EnterState()
    {
       playerInputHandler.SwitchEnableExploreAction(Player.PlayerInputHandler.EnableState.ENABLE);     //該当アクションを有効化
    }

    public void UpdateState()
    {
       // playerInputHandler.OnExploreAction();
    }
    public void ExitState()
    {
        playerInputHandler.SwitchEnableExploreAction(Player.PlayerInputHandler.EnableState.DISABLE);      //該当アクションを無効化
    }
}