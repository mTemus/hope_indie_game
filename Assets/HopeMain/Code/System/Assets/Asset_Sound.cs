using System;
using UnityEngine;

namespace HopeMain.Code.System.Assets
{
    public enum AssetSoundType
    {
        Walking, Background, Construction,
    }
    
    [Serializable]
    public class Asset_Sound
    {
        [SerializeField] private string assetName;
        [SerializeField] private AudioClip clip;

        public string AssetName => assetName;
        public AudioClip Clip => clip;
    }
}
