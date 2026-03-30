using UnityEngine;
using UnityEngine.AI;

public class OldWomanEvent : EventBehaviour
{
    [SerializeField] GameObject oldWoman;

    [SerializeField] Transform activePos;
    [SerializeField] NavMeshAgent agent;
     IInteractedObj intaractedObj;
    [SerializeField] Player.InteractHandler interactHandler;

    //‚¨‚Î‚ ‚¿‚á‚ñ‚ğoŒ»‚³‚¹‚é
    protected override void ExecuteEvent()
    {
        oldWoman.SetActive(true);

        agent.SetDestination(activePos.position);

        //SE‚ğ–Â‚ç‚·
        SoundManager.Instance.PlaySE(SEType.DECIDE_UI);

        //ƒJƒƒ‰‚ğ‚¨‚Î‚ ‚¿‚á‚ñ‚Ì•ûŒü‚É“®‚©‚·
        intaractedObj = oldWoman.GetComponent<IInteractedObj>();

        if (intaractedObj != null)
        {
            // ƒJƒƒ‰‚ğ‚¨‚Î‚ ‚¿‚á‚ñ‚Ì•ûŒü‚É“®‚©‚·‚È‚Ç‚Ìˆ—
            interactHandler.SetInteractBehavie(intaractedObj);
        }
    }


}
