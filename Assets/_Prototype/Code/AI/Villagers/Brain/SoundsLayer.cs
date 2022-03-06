using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Prototype.Code.AI.Villagers.Brain
{
    // TODO: there should be one enum for that usage
    
    /// <summary>
    /// 
    /// </summary>
    public enum SoundEffectType
    {
        Walking,
    }
    
    /// <summary>
    /// 
    /// </summary>
    public class SoundsLayer : BrainLayer
    {
        [SerializeField] private AudioSource walkingChannel;

        public override void Initialize(Brain brain) {}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="effectType"></param>
        /// <exception cref="Exception"></exception>
        public void PlaySoundEffect(SoundEffectType effectType)
        {
            switch (effectType) {
                case SoundEffectType.Walking:
                    PlayWalkingSoundEffect();
                    break;
                
                default:
                    throw new Exception("VILLAGER SOUNDS LAYER --- CAN'T PLAY SOUND EFFECT FOR TYPE: " + effectType);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clip"></param>
        public void SetWalkingAudioClip(AudioClip clip)
        {
            if (walkingChannel.clip == clip) return;
            walkingChannel.clip = clip;
        }

        private void PlayWalkingSoundEffect()
        {
            if (walkingChannel.isPlaying) return;
            walkingChannel.pitch = Random.Range(1f, 1.5f);
            walkingChannel.volume = Random.Range(0.2f, 0.3f);
            walkingChannel.Play((ulong) 0.2);
        }
    }
}

