using System;
using UnityEngine;

namespace Source.PingPong.Simulation
{
    public class Racket : ReflectSurface, IRacket
    {
        [Range(0f, 1f)]
        [SerializeField] private float _distanceImpactLimit = 1f;
        [Tooltip("Bigger coefficient bigger X can be in result direction")]
        [SerializeField] private float _maxAngleCoefficient = 1f;
        [Tooltip("Racket has it's own direction impact based on where collision was happened relative to racket center")]
        [SerializeField] private float _racketImpact = .5f;
        [SerializeField] private Vector3 _racketNormal = Vector3.up;
        [SerializeField] private float _size = 1f;
        [SerializeField] private BoxCollider _boxCollider;

        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        public float Size
        {
            get => _size;
            set
            {
                const float tolerance = .001f;
                if (Math.Abs(value - _size) < tolerance)
                    return;
                _size = value;
                OnSizeChange?.Invoke(_size);
                UpdateBoxCollider(_size);
            }
        }

        public event Action OnReflect;
        public event Action<float> OnSizeChange;
        
        void ITransformParent.AddChild(Transform child)
            => child.parent = transform;

        protected override Vector3 GetReflectDirection(IMovable movable, in ContactPoint contact)
        {
            var baseDirection = base.GetReflectDirection(movable, in contact);
            
            var distanceFromCenter = transform.position.x - contact.point.x;
            var distanceCoff = Math.Min(_maxAngleCoefficient, Math.Abs(distanceFromCenter / _distanceImpactLimit));
            var simulateDirection = Vector3.Lerp(-_racketNormal, Vector3.left * Math.Sign(distanceFromCenter), distanceCoff);
            var simulateReflectDirection = Vector3.Reflect(simulateDirection, _racketNormal);

            return Vector3.Lerp(baseDirection, simulateReflectDirection, _racketImpact);
        }

        private void UpdateBoxCollider(float size)
        {
            var boxColliderSize = _boxCollider.size;
            boxColliderSize.x = size;
            _boxCollider.size = boxColliderSize;
        }

        protected override void OnCollisionEnter(Collision other)
        {
            base.OnCollisionEnter(other);
            
            if(other.gameObject.TryGetComponent<IMovable>(out _))
                OnReflect?.Invoke();
        }

        private void ValidateBoxCollider()
        {
            if(_boxCollider == null)
                return;
            
            UpdateBoxCollider(_size);
        }

        private void OnValidate()
        {
            ValidateBoxCollider();
        }

        #if UNITY_EDITOR
        private void DrawRacket()
        {
            var pos = transform.position;
            Gizmos.color = Color.cyan;
            var halfOffset = new Vector3(_size / 2f, 0f, 0f);
            Gizmos.DrawLine(pos - halfOffset, pos + halfOffset);
        }

        private void DrawContactDistanceImpact()
        {
            var pos = transform.position;
            var offset = new Vector3(_distanceImpactLimit * _size / 2f, 0f, 0f);
            const float gizmoSphereRadius = .15f;
            Gizmos.DrawSphere(pos + offset, gizmoSphereRadius);
            Gizmos.DrawSphere(pos - offset, gizmoSphereRadius);
        }

        private void DrawRacketNormal()
        {
            var pos = transform.position;
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(pos, pos + _racketNormal);
        }

        private void OnDrawGizmos()
        {
            DrawRacket();
            DrawRacketNormal();
        }

        private void OnDrawGizmosSelected()
        {
            DrawContactDistanceImpact();
        }
        #endif
    }
}