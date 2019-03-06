namespace CygniAPI.Contexts
{
    /// <summary>
    /// A single header from the input context request.
    /// </summary>
    public class InHeader
    {
        /// <summary>
        /// Headers key.
        /// </summary>
        public string Key;
        /// <summary>
        /// All values that are bound to the key.
        /// </summary>
        public string[] Values;
    }
}
