using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Villagers.Brain.Layers
{
    public enum VillagerSoundEffectType
    {
        Walking,
    }

    public class Villager_Brain_SoundsLayer : Villager_BrainLayer
    {
        [SerializeField] private AudioSource walkingChannel;

        public override void Initialize(Villager_Brain villagerBrain) {}

        public void PlaySoundEffect(VillagerSoundEffectType effectType)
        {
            switch (effectType) {
                case VillagerSoundEffectType.Walking:
                    PlayWalkingSoundEffect();
                    break;
                
                default:
                    throw new Exception("VILLAGER SOUNDS LAYER --- CAN'T PLAY SOUND EFFECT FOR TYPE: " + effectType);
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
            walkingChannel.volume = Random.Range(0.2f, 0.3f);
            walkingChannel.Play((ulong) 0.2);
        }
    }
}

