namespace Microsoft.ApplicationInsights.Extensibility.Implementation.External
{
    using System;

    /// <summary>
    /// Partial class to impelement ISerializableWithWriter.
    /// </summary>    
    internal partial class StackFrame : ISerializableWithWriter
    {
        public void Serialize(ISerializationWriter serializationWriter)
        {
            serializationWriter.WriteProperty("level", this.level);
            serializationWriter.WriteProperty("method", this.method);
            serializationWriter.WriteProperty("assembly", this.assembly);
            serializationWriter.WriteProperty("fileName", this.fileName);
            serializationWriter.WriteProperty("line", this.line);
        }
    }
}