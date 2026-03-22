using UI.Converseation;

public class ConversationState : Player.IPlayerState
{
    Player.PlayerInputHandler playerInputHandler;
    public ConversationState(Player.PlayerInputHandler _playerInputHandler)
    {
        playerInputHandler = _playerInputHandler;

        //カメラを対象者の方向に向ける。

    }
    public void EnterState()
    {

        playerInputHandler.SwitchEnableConversationAction(Player.PlayerInputHandler.EnableState.ENABLE);     //該当アクションを有効化
        //Manager.CameraManager.instance.ChangeCurrentCam(Manager.CameraManager.CameraType.CONVERSATION);
    }

    public void UpdateState()
    {
    }
    public void ExitState()
    {
       playerInputHandler.SwitchEnableConversationAction(Player.PlayerInputHandler.EnableState.DISABLE);      //該当アクションを無効化
       ConverseUIManager.instance.UnSetConversation();

        //Manager.CameraManager.instance.ChangeCurrentCam(Manager.CameraManager.CameraType.EXPLORE);
        Manager.CameraManager.instance.UnSetLookTarget();
    }
}
