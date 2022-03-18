
namespace _Prototype.Code.v002.System
{
    /// <summary>
    /// Interface that should be implemented by managers and classes that are handling multiple systems or entities
    /// in their update loop. It allows to control manually which script should be called first or how often
    /// </summary>
    public interface IManualUpdate
    {
        /// <summary>
        /// 'Update' method but manual, should be called in GameManager
        /// </summary>
        /// <param name="timeSpeed">Value of current game world time speed (default 1f)</param>
        public void ManualUpdate(float timeSpeed);
    }
}
