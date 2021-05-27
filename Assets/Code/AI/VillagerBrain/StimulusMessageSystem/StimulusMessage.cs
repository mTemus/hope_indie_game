using Code.AI.VillagerBrain.Layers;

namespace Code.AI.VillagerBrain.StimulusMessageSystem
{
    public class StimulusMessage
    {
        private readonly string sender;
        private readonly BrainLayerTypes receiver;
        private readonly int stimulusType;
        private readonly StimulusData data;
        
        public float Delay { get; set; }
        
        public bool Received { get; set; }

        public StimulusMessage(string sender, BrainLayerTypes receiver, int stimulusType, StimulusData data, float delay = 0f)
        {
            this.sender = sender;
            this.receiver = receiver;
            this.stimulusType = stimulusType;
            this.data = data;
            Delay = delay;
            
            Received = false;
        }

        public string Sender => sender;
        public BrainLayerTypes Receiver => receiver;
        public int StimulusType => stimulusType;
        public StimulusData Data => data;
    }
}
