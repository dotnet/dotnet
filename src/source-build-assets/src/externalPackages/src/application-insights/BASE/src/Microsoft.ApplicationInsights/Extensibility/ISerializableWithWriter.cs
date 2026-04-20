namespace Microsoft.ApplicationInsights.Extensibility
{
    /// <summary>
    /// Interface for defining objects which can be serialized with a given <see cref="ISerializationWriter"/>.
    /// </summary>
    public interface ISerializableWithWriter
    {
        /// <summary>
        /// Writes serialization info about the class using the given <see cref="ISerializationWriter"/>.
        /// </summary>
        void Serialize(ISerializationWriter serializationWriter);
    }
}
