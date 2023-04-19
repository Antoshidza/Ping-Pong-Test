using UnityEngine;

namespace Source.PingPong.Presentation
{
    public class LevelUI : MonoBehaviour, IVisible
    {
        public void SetVisible(bool value)
            => gameObject.SetActive(value);
    }
}