using UnityEngine;

namespace Code.Player
{
    public class PlayerAnimatorAdapter : MonoBehaviour
    {
        private PlayerSoundEffects sounds;

        private void Awake()
        {
            sounds = transform.parent.gameObject.GetComponent<PlayerSoundEffects>();
        }

        private void PlaySoundEffect(PlayerSoundEffectType effectType) =>
            sounds.PlaySoundEffect(effectType);
    }
}
