using ModestTree;
using Source.SaveLoad;
using UnityEngine;
using Zenject;

namespace Source.PingPong.Presentation
{
    public class BallViewSaveLoadManager : ISaveLoadManager
    {
        [Inject] private BallViewSettings _ballViewSettings;

        private const string BallColorPrefsName = "BallColor";

        public void Save()
        {
            PlayerPrefs.SetString(BallColorPrefsName, JsonUtility.ToJson(_ballViewSettings.Color));
        }

        public void Load()
        {
            var colorJson = PlayerPrefs.GetString(BallColorPrefsName);
            if(colorJson.IsEmpty())
                return;

            var color = JsonUtility.FromJson<Color>(colorJson);
            _ballViewSettings.Color = color;
        }
    }
}