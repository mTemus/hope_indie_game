namespace Code.Utilities
{
    public static class Utilities
    {
        public static int IncrementIdx(int idx, int value, int maxValue)
        {
            if (idx + value >= maxValue) 
                return 0;
            if(idx + value < 0) 
                return maxValue;
            
            return idx + value;
        }
    }
}