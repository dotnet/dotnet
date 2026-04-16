namespace Microsoft.ApplicationInsights.Web.TestFramework
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;

    using TaskEx = System.Threading.Tasks.Task;

    internal class StubTransmission : Transmission
    {
        public Action<Stream> OnSave = stream => { };

        public Func<HttpWebResponseWrapper> OnSend = () => null;

        public StubTransmission()
            : base(new Uri("any://uri"), new byte[0], string.Empty, string.Empty)
        {
        }

        public StubTransmission(byte[] content)
            : base(new Uri("any://uri"), content, string.Empty, string.Empty)
        {
        }

        public Task SaveAsync(Stream stream)
        {
            return TaskEx.Run(() => this.OnSave(stream));
        }

        public override Task<HttpWebResponseWrapper> SendAsync()
        {
            return TaskEx.Run(this.OnSend);
        }
    }
}