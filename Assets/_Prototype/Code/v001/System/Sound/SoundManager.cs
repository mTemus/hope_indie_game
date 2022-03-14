using System.Collections;
using System.Linq;
using UnityEngine;

namespace _Prototype.Code.v001.System.Sound
{
    public class SoundManager : MonoBehaviour
    {
        [Header("Channels")] 
        [SerializeField] private AudioSource backgroundChannel;
        [SerializeField] private AudioSource environmentChannel;
    
        [Header("Sounds")]
        [SerializeField] private AudioClip[] backgroundSounds;
        [SerializeField] private Assets.Sound[] environmentEffects;

        private float _backgroundTimer = 5f;
        
        private void Start()
        {
            environmentChannel.clip = environmentEffects.First(sound => sound.AssetName.Contains("water")).Clip;
            environmentChannel.Play();
            
            StartCoroutine(PlayBackgroundSound());
        }

        private IEnumerator PlayBackgroundSound()
        {
            yield return new WaitUntil(() => _backgroundTimer > 0);

            AudioClip clip = backgroundSounds[Random.Range(0, backgroundSounds.Length)];
            backgroundChannel.clip = clip;
            backgroundChannel.Play();

            _backgroundTimer = clip.length + Random.Range(0, 10);
        }

        private void Update()
        {
            _backgroundTimer -= Time.deltaTime;
        }
    }
}
