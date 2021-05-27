using System.Collections.Generic;
using System.Linq;
using Code.AI.VillagerBrain.StimulusMessageSystem;
using UnityEngine;

namespace Code.AI.VillagerBrain.Layers
{
    public enum BrainLayerTypes
    {
        Behaviour, Perception, Motion, Work
    }
    
    public class Villager_Brain_PerceptionLayer : MonoBehaviour, IBrainLayer
    {
        private readonly List<StimulusMessage> messages = new List<StimulusMessage>();
        private readonly List<StimulusListener> listeners = new List<StimulusListener>();

        private readonly float delayDecrement = Time.deltaTime;
        
        public void Initialize(Villager_Brain brain)
        {
            
        }

        public void Update()
        {
            if (messages.Count <= 0) return;

            foreach (StimulusMessage stimulus in messages) {
                if (stimulus.Delay > 0) 
                    stimulus.Delay -= delayDecrement;
                else {
                    foreach (StimulusListener listener in listeners
                        .Where(listener => stimulus.Receiver == listener.Receiver)
                        .Where(listener => stimulus.StimulusType == listener.StimulusType)) {
                        listener.ReceiveMessage.Invoke(stimulus.Data);
                        stimulus.Received = true;
                    }
                }
            }

            List<StimulusMessage> tmpMessages = new List<StimulusMessage>(messages);
            
            foreach (StimulusMessage stimulus in tmpMessages
                .Where(stimulus => stimulus.Received)) { messages.Remove(stimulus); }
        }

        public void RegisterListener(StimulusListener newListener)
        {
            if (listeners.Contains(newListener)) return;
            listeners.Add(newListener);
        }

        public void ReceiveStimulusMessage(StimulusMessage message)
        {
            messages.Add(message);
        }
    }
}
