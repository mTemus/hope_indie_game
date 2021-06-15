using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Player
{
    public enum PlayerSoundEffectType
    {
        Walking, 
    }
    
    public class PlayerSoundEffects : MonoBehaviour
    {
        [SerializeField] private AudioSource walkingChannel;

        public void PlaySoundEffect(PlayerSoundEffectType effectType)
        {
            switch (effectType) {
                case PlayerSoundEffectType.Walking:
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
