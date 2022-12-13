using Data.ValueObjects;
using Managers;
using UnityEngine;
using Unity.Mathematics;
using Keys;



namespace Controllers.Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private PlayerManager manager;
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private new Collider collider;

        #endregion

        #region Private Variables

         private MovementData _data;
        private bool _isReadyToMove, _isReadyToPlay;
        private float2 _clampvalues;
        private float _xValue;

        #endregion

        #endregion
        private void FixedUpdate()
        {
            if (_isReadyToPlay)
            {
                if (_isReadyToMove)
                {
                    MovePlayer();
                }
                else StopPlayerHorizontally();
            }
            else StopPlayer();
        }
        public void OnReset()
        {
            StopPlayer();
            _isReadyToMove = false;
            _isReadyToPlay = false;
        }
        private void StopPlayer()
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }
        private void StopPlayerHorizontally()
        {
            rigidbody.velocity = new Vector3(0,rigidbody.velocity.y,_data.ForwardSpeed);
            rigidbody.angularVelocity = Vector3.zero;
        }
        private void MovePlayer()
        {
            var velocity = rigidbody.velocity;
            velocity = new Vector3(_xValue * _data.SidewaysSpeed, velocity.y, _data.ForwardSpeed);
            rigidbody.velocity = velocity;
            Vector3 position;
            position = new Vector3(Mathf.Clamp(rigidbody.position.x, _clampvalues.x, _clampvalues.y), (position = rigidbody.position).y, position.z);
            rigidbody.position = position;
        }

        public void GetMovementData(MovementData movementData)
        {
            _data = movementData;
        }
        internal void IsReadyToPlay(bool condition)
        {
            _isReadyToPlay = condition;
        }
        internal void IsReadyToMove(bool condition)
        {
            _isReadyToMove = condition;
        }
        internal void UptadeInputParams(HorizontalInputParams InputParams)
        {
            _xValue = InputParams.HorizontalInputValue;
            _clampvalues = new float2(InputParams.HorizontalInputClampNegativeSide, InputParams.HorizontalInputClampPositiveSide);
        }
    }
}