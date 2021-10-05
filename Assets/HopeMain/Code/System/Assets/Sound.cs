using System;
using UnityEngine;

namespace HopeMain.Code.System.Assets
{
    public enum SoundType
    {
        Walking, Background, Construction,
    }
    
    [Serializable]
    public class Sound
    {
        [SerializeField] private string assetName;
        [SerializeField] private AudioClip clip;

        public string AssetName => assetName;
        public AudioClip Clip => clip;
    }
}
