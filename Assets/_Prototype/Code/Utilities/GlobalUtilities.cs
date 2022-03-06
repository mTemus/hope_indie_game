namespace _Prototype.Code.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public static class GlobalUtilities
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="value"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static int IncrementIdx(int idx, int value, int maxValue)
        {
            if (idx + value > maxValue - 1) 
                return 0;
            if(idx + value < 0) 
                return maxValue - 1;
            
            return idx + value;
        }
    }
}