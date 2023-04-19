using UnityEngine;

namespace Source.PingPong.Simulation
{
    public static class MovableExtensions
    {
        public static void Reflect(this IMovable movable, in Vector3 normal)
            => movable.Direction = movable.GetReflectDirection(normal);

        public static Vector3 GetReflectDirection(this IMovable movable, in Vector3 normal)
        {
            var direction = movable.Direction;
            var reflectDirection = Vector3.Reflect(direction, normal);
            reflectDirection.z = 0f;
            return reflectDirection;
        }
        
        public static bool TryGetMovables(this GameObject source, out IMovable[] movables)
        {
            movables = source.GetComponents<IMovable>();
            return movables.Length != 0;
        }
    }
}