namespace Microsoft.ApplicationInsights.Extensibility.Implementation.External
{
    using System;
    using System.Linq;

    /// <summary>
    /// Partial class to implement ISerializableWithWriter.
    /// </summary>
    internal partial class ExceptionDetails : ISerializableWithWriter
    {
        public void Serialize(ISerializationWriter serializationWriter)
        {
            serializationWriter.WriteProperty("id", this.id);
            serializationWriter.WriteProperty("outerId", this.outerId);
            serializationWriter.WriteProperty("typeName", this.typeName);
            serializationWriter.WriteProperty("message", this.message);
            serializationWriter.WriteProperty("hasFullStack", this.hasFullStack);
            serializationWriter.WriteProperty("stack", this.stack);            
            serializationWriter.WriteProperty("parsedStack", this.parsedStack.ToList<ISerializableWithWriter>());
        }
    }
}