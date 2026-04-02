using System;
using UnityEngine;

namespace ItemConsumed
{
    /// <summary>
    /// アイテムの仕様対象オブジェクトの検知用コライダー
    /// </summary>
    public class TriggerProxy : MonoBehaviour
    {
        public Action<Collider> OnEntered;
        public Action<Collider> OnStay;
        public Action<Collider> OnExit;


        private void OnTriggerEnter(Collider other)
        {
            OnEntered?.Invoke(other);
        }

        private void OnTriggerStay(Collider other)
        {
            OnStay?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            OnExit?.Invoke(other);
        }
    }
}
