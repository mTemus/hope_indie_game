using System;
using Code.AI.VillagerBrain.Layers;

namespace Code.AI.VillagerBrain.StimulusMessageSystem
{
    public class StimulusListener
    {
        private readonly BrainLayerTypes receiver;
        private readonly int stimulusType;
        private readonly Action<StimulusData> receiveMessage;

        public StimulusListener(BrainLayerTypes receiver, int stimulusType, Action<StimulusData> receiveMessage)
        {
            this.receiver = receiver;
            this.stimulusType = stimulusType;
            this.receiveMessage = receiveMessage;
        }

        public BrainLayerTypes Receiver => receiver;
        public int StimulusType => stimulusType;
        public Action<StimulusData> ReceiveMessage => receiveMessage;
    }
}
