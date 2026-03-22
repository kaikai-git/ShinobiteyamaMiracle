
public class DocumentReadingState : Player.IPlayerState
{
    Player.PlayerInputHandler playerInputHandler;


    public DocumentReadingState(Player.PlayerInputHandler _playerInputHandler)
    {
        playerInputHandler = _playerInputHandler;
    }
    public void EnterState()
    {
        playerInputHandler.SwitchEnableReadingDocumentAction(Player.PlayerInputHandler.EnableState.ENABLE);     //該当アクションを有効化
    }

    public void UpdateState()
    {
        // playerInputHandler.OnExploreAction();
    }
    public void ExitState()
    {
        UI.Document.DocumentUIManager.instance.UnSetDocumentUI();
        playerInputHandler.SwitchEnableReadingDocumentAction(Player.PlayerInputHandler.EnableState.DISABLE);      //該当アクションを無効化
    }
}
