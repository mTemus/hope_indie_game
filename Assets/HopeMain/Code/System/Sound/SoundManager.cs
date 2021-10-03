using System.Collections;
using System.Linq;
using Code.System.Assets;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.System.Sound
{
    public class SoundManager : MonoBehaviour
    {
        [Header("Channels")] 
        [SerializeField] private AudioSource backgroundChannel;
        [SerializeField] private AudioSource environmentChannel;
    
        [Header("Sounds")]
        [SerializeField] private AudioClip[] backgroundSounds;
        [SerializeField] private Asset_Sound[] environmentEffects;

        private float backgroundTimer = 5f;
        
        private void Start()
        {
            environmentChannel.clip = environmentEffects.First(sound => sound.AssetName.Contains("water")).Clip;
            environmentChannel.Play();
            
            StartCoroutine(PlayBackgroundSound());
        }

        private IEnumerator PlayBackgroundSound()
        {
            yield return new WaitUntil(() => backgroundTimer > 0);

            AudioClip clip = backgroundSounds[Random.Range(0, backgroundSounds.Length)];
            backgroundChannel.clip = clip;
            backgroundChannel.Play();

            backgroundTimer = clip.length + Random.Range(0, 10);
        }

        private void Update()
        {
            backgroundTimer -= Time.deltaTime;
        }
    }
}
