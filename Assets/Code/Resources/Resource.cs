namespace Code.Resources
{
    public enum ResourceType
    {
        WOOD, STONE
    }

    public class Resource
    {
        private ResourceType type;

        public int Amount;
        
        public Resource(ResourceType type)
        { 
            this.type = type;
        }

        public Resource(ResourceType type, int amount)
        {
            this.type = type;
            Amount = amount;
        }

        public ResourceType Type => type;
    }
}
