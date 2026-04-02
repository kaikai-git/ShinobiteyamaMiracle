using System;
using UnityEngine;

namespace Event
{
    /// <summary>
    /// 接触を検知してイベントを飛ばすだけのクラス
    /// </summary>
    public class TriggerProxy : MonoBehaviour
    {
        public Action<Collider> OnEntered;

        private void OnTriggerEnter(Collider other)
        {
            OnEntered?.Invoke(other);
        }
    }

}

