using UnityEngine;

namespace Source.PingPong.Presentation
{
    public class SimpleMeshView : MonoBehaviour, ISizableView , IVisible
    {
        [SerializeField] private Transform _root;
        [SerializeField] private MeshRenderer _meshRenderer;

        public GameObject GameObject => gameObject;
        
        public Vector3 Size
        {
            get => _root.localScale;
            set => _root.localScale = value;
        }

        public Color Color
        {
            get => _meshRenderer.material.color;
            set => _meshRenderer.material.color = value;
        }

        public void SetVisible(bool value)
            => gameObject.SetActive(value);
    }
}