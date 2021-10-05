using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HopeMain.Code.AI.Player.Brain
{
    public enum SoundEffectType
    {
        Walking, 
    }
    
    public class SoundsLayer : BrainLayer
    {
        [SerializeField] private AudioSource walkingChannel;

        public override void Initialize(Brain brain) { }
        
        public void PlaySoundEffect(SoundEffectType effectType)
        {
            switch (effectType) {
                case SoundEffectType.Walking:
                    PlayWalkingSoundEffect();
                    break;
                
                default:
                    throw new Exception("PLAYER SOUND CONTROLLER --- CAN'T PLAY SOUND EFFECT FOR TYPE: " + effectType);
            }
        }
 
        public void SetWalkingAudioClip(AudioClip clip)
        {
            if (walkingChannel.clip == clip) return;
            walkingChannel.clip = clip;
        }

        private void PlayWalkingSoundEffect()
        {
            if (walkingChannel.isPlaying) return;
            walkingChannel.pitch = Random.Range(1f, 1.5f);
            walkingChannel.volume = Random.Range(0.3f, 0.4f);
            walkingChannel.Play((ulong) 0.2);
        }
    }
}
