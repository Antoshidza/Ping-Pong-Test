using Source.PingPong.Simulation;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Source.PingPong
{
    public class ReflectSurface : MonoBehaviour
    {
        private void LogNoIMovableWarning(Object source)
            => Debug.LogWarning($"{gameObject.name} triggered by {source.name} but later has no {nameof(IMovable)} component", source);

        protected virtual Vector3 GetReflectDirection(IMovable movable, in ContactPoint contact)
            => movable.GetReflectDirection(contact.normal);

        private void ApplyReflect(IMovable movable, in ContactPoint contact)
            => movable.Direction = GetReflectDirection(movable, contact);

        private void ApplyReflect(IMovable[] movables, in ContactPoint contact)
        {
            for (int movableIndex = 0; movableIndex < movables.Length; movableIndex++)
                ApplyReflect(movables[movableIndex], contact);
        }

        protected virtual void OnCollisionEnter(Collision other)
        {
            if(!other.gameObject.TryGetMovables(out var movables))
            {
                LogNoIMovableWarning(other.gameObject);
                return;
            }

            ApplyReflect(movables, other.GetContact(0));
        }
    }
}