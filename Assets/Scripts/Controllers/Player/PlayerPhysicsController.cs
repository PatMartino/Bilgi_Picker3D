using Managers;
using UnityEngine;
using Signals;

namespace Controllers.Player
{
    public class PlayerPhysicsController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private PlayerManager manager;
        [SerializeField] private new Collider collider;
        [SerializeField] private new Rigidbody rigidbody;

        #endregion

        #endregion

        public void OnReset()
        {
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("StageArea"))
            {
                CoreGameSignals.Instance.onStageAreaEntered?.Invoke();
                InputSignals.Instance.onEnableInput?.Invoke();
                InputSignals.Instance.onDisableInput?.Invoke();
            }
        }
    }
}