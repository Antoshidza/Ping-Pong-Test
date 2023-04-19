using Source.PingPong.Simulation;
using UnityEditor;
using UnityEngine;

namespace Source.PingPong
{
    public class Ball : MonoBehaviour, IBall
    {
        [SerializeField] private Rigidbody _rigidbody;
        private Vector3 _direction;
        private float _speed;
        private float _initialZ;

        public Vector3 Direction
        {
            get => _direction;
            set
            {
                _direction = value.normalized;
                UpdateMovement();
            }
        }

        public float Speed
        {
            get => _speed;
            set
            {
                _speed = value;
                UpdateMovement();
            }
        }

        public Vector3 Velocity
        {
            get => _direction * _speed;
            set
            {
                _direction = value.normalized;
                _speed = value.magnitude;
                UpdateMovement();
            }
        }

        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }
        
        void ITransformParent.AddChild(Transform child)
            => child.parent = transform;

        private void Update()
        {
            LockZPosition();
        }

        private void UpdateMovement()
        {
            _rigidbody.velocity = Velocity;
        }

        private void LockZPosition()
        {
            var tmp = transform.position;
            tmp.z = _initialZ;
            transform.position = tmp;
        }

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            var pos = transform.position;
            Gizmos.DrawLine(pos, pos + Vector3.ClampMagnitude(Velocity, 2.5f));
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(pos, pos + _direction);

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(pos, .25f);

            var labelOffset = new Vector3(0f, 1f, 0f);
            Handles.Label(pos + labelOffset, $"dir: {_direction.x:n1}, {_direction.y:n1}\nspeed: {_speed}");
        }
        #endif
    }
}