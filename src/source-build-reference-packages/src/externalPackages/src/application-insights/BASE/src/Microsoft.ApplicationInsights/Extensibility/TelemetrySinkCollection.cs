namespace Microsoft.ApplicationInsights.Extensibility
{
    using System;
    using System.Diagnostics;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;

    internal class TelemetrySinkCollection : SnapshottingList<TelemetrySink>
    {
        private const int DefaultSinkIndex = 0;
        private const string DefaultSinkCannotBeChanged = "Default sink cannot be changed";

        public TelemetrySink DefaultSink => this[DefaultSinkIndex];

        public override TelemetrySink this[int index]
        {
            get => base[index];

            set
            {
                if (index == TelemetrySinkCollection.DefaultSinkIndex && this.Count != 0)
                {
                    throw new InvalidOperationException(DefaultSinkCannotBeChanged);
                }

                base[index] = value;
            }
        }

        public override bool Remove(TelemetrySink item)
        {
            if (this.IsDefaultSink(item))
            {
                throw new InvalidOperationException("Default sink cannot be removed");
            }

            return base.Remove(item);
        }

        public override void Clear()
        {
            throw new InvalidOperationException(nameof(TelemetrySinkCollection) + " cannot be cleared--default sink must always be available");
        }

        public override void Insert(int index, TelemetrySink item)
        {
            if (index == TelemetrySinkCollection.DefaultSinkIndex && this.Count != 0)
            {
                throw new InvalidOperationException(DefaultSinkCannotBeChanged);
            }

            base.Insert(index, item);
        }

        public override void RemoveAt(int index)
        {
            if (index == TelemetrySinkCollection.DefaultSinkIndex)
            {
                throw new InvalidOperationException(DefaultSinkCannotBeChanged);
            }

            base.RemoveAt(index);
        }

        private bool IsDefaultSink(TelemetrySink sink)
        {
            Debug.Assert(sink != null, "The 'sink' parameter value should not be null");
            return object.ReferenceEquals(sink, this[TelemetrySinkCollection.DefaultSinkIndex]);
        }
    }
}
