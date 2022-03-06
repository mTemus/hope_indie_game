using System;
using UnityEngine;

namespace _Prototype.Code.System.Assets
{
    /// <summary>
    /// 
    /// </summary>
    public enum SoundType
    {
        Walking, Background, Construction,
    }
    
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class Sound
    {
        [SerializeField] private string assetName;
        [SerializeField] private AudioClip clip;

        public string AssetName => assetName;
        public AudioClip Clip => clip;
    }
}
