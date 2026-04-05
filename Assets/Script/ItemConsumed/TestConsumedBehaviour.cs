using UnityEngine;

/// <summary>
/// 
/// </summary>
public class TestConsumedBehaviour : ItemConsumeBehaviour
{
    [SerializeField] GameObject testObj;
    override protected void OnInteractedInherit()
    {
        //ŠY“–
        Debug.Log("bb");

        testObj.SetActive(true);
    }
}
