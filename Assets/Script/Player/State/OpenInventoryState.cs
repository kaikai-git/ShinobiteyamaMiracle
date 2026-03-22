using Player;
using Character;
//インベントリを開いている時のアクション管理ステート
public class OpenInventoryState : IPlayerState
{
     PlayerInputHandler playerInputHandler;
     InventoryManager inventoryManager;
    public OpenInventoryState(InventoryManager _characterInventoryManager,
   PlayerInputHandler _playerInputHandler )
    {
        playerInputHandler = _playerInputHandler;
        inventoryManager = _characterInventoryManager;
    }
    public void EnterState()
    {
        //OnCurrentActon += OnInventoryAction();
        playerInputHandler.SwitchEnableOpenInventoryAction(PlayerInputHandler.EnableState.ENABLE);     //該当アクションを有効化
        inventoryManager.ActiveInventory();
    }

    public void UpdateState()
    {
        //playerInputHandler.OnOpenInventoryStateAction();
    }

    public void ExitState()
    {
        playerInputHandler.SwitchEnableOpenInventoryAction(PlayerInputHandler.EnableState.DISABLE);
         inventoryManager.DisableInventory();
    }
}
