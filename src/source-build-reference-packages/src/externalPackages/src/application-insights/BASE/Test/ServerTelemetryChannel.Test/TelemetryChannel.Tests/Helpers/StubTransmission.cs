namespace Microsoft.ApplicationInsights.TestFramework
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;


    internal class StubTransmission : Transmission
    {
        public Action<Stream> OnSave = stream => { };

        public Func<HttpWebResponseWrapper> OnSend = () => null;

        public StubTransmission()
            : base(new Uri("any://uri"), new byte[0], JsonSerializer.ContentType, string.Empty)
        {
        }

        public StubTransmission(byte[] content)
            : base(new Uri("any://uri"), content, JsonSerializer.ContentType, string.Empty)
        {
        }

        public StubTransmission(ICollection<ITelemetry> telemetry)
            : base(new Uri("any://uri"), telemetry)
        {
        }

        public Task SaveAsync(Stream stream)
        {
            return Task.Run(() => this.OnSave(stream));
        }

        public override Task<HttpWebResponseWrapper> SendAsync()
        {
            return Task.Run(this.OnSend);
        }

        public override Tuple<Transmission, Transmission> Split(Func<int, int> calculateLength)
        {
            Tuple<Transmission,Transmission> ret = base.Split(calculateLength);

            if (ret.Item2 == null)
            {
                return Tuple.Create((Transmission)this, (Transmission) null);
            }

            return Tuple.Create((Transmission)this.Convert(ret.Item1), (Transmission)this.Convert(ret.Item2));
        }

        private StubTransmission Convert(Transmission transmission)
        {
            if (transmission != null)
            {
                if (transmission.TelemetryItems == null)
                {
                    transmission = new StubTransmission(transmission.Content)
                    {
                        OnSave = this.OnSave,
                        OnSend = this.OnSend,
                    };
                }
                else
                {
                    transmission = new StubTransmission(transmission.TelemetryItems)
                    {
                        OnSave = this.OnSave,
                        OnSend = this.OnSend,
                    };
                }
            }

            return (StubTransmission)transmission;
        }

        public int CountOfItems()
        {
            if (this.TelemetryItems != null)
            {
                return this.TelemetryItems.Count;
            }
            else
            {
                bool compress = this.ContentEncoding == JsonSerializer.CompressionType;
                string[] payloadItems = JsonSerializer
                    .Deserialize(this.Content, compress)
                    .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                return payloadItems.Length;
            }
        }
    }
}
