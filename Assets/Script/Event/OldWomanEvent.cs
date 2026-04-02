using UnityEngine;
using UnityEngine.AI;

public class OldWomanEvent : EventBehaviour
{
    [SerializeField] GameObject oldWoman;

    [SerializeField] Transform activePos;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Player.InteractHandler interactHandler;


    private void Start()
    {
        //‚¨‚Î‚ ‚¿‚á‚ñÁ‚µ‚Æ‚­
        oldWoman.SetActive(false);
    }
    //‚¨‚Î‚ ‚¿‚á‚ñ‚ğoŒ»‚³‚¹‚é
    protected override void ExecuteEvent()
    {
        oldWoman.SetActive(true);

        agent.SetDestination(activePos.position);
        //SE‚ğ–Â‚ç‚·
        SoundManager.Instance.PlaySE(SEType.DECIDE_UI);

        //ƒJƒƒ‰‚ğ‚¨‚Î‚ ‚¿‚á‚ñ‚Ì•ûŒü‚É“®‚©‚·
        var conversationTarget = oldWoman.GetComponent<IConversationInteractable>();

        if (conversationTarget != null)
        {
            // ƒJƒƒ‰‚ğ‚¨‚Î‚ ‚¿‚á‚ñ‚Ì•ûŒü‚É“®‚©‚·‚È‚Ç‚Ìˆ—
            interactHandler.SetInteractBehavie(conversationTarget);
        }
    }


}
