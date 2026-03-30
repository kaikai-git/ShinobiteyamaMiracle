using UnityEngine;

//演出の種類
enum EventType
{
    KAKASHI,    //カカシが出現
    OLD_WOMAN   //おばあちゃんが出現
}

[System.Serializable]   
public class EventDat
{
    [SerializeField] EventType eventType;
}
