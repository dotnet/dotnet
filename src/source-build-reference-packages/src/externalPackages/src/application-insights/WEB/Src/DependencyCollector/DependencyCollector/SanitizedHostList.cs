namespace Microsoft.ApplicationInsights.DependencyCollector
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// Sanitized collection on host strings.
    /// </summary>
    internal class SanitizedHostList : ICollection<string>
    {
        private List<string> hostList = new List<string>();

        #region ICollection Implemenation
        public int Count
        {
            get
            {
                return this.hostList.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// We sanitize before adding to the list. We try our best to extract the host name from the passed in item and store that in the collection.
        /// </summary>
        /// <param name="item">Item to be added.</param>
        public void Add(string item)
        {
            if (string.IsNullOrEmpty(item))
            {
                // Since this is called in the context of the config. If there is an empty <Add /> element, we wil just ignore and move on.
                return;
            }

            Uri uriResult;
            if (Uri.TryCreate(item, UriKind.Absolute, out uriResult))
            {
                this.AddIfNotExist(uriResult.Host);
                return;
            }
            else if (Uri.TryCreate("http://" + item, UriKind.Absolute, out uriResult))
            {
                // If the user specified something that doesn't start with http - Let's append and try get host value that way
                this.AddIfNotExist(uriResult.Host);
                return;
            }

            // If the provided string is not a valid url - don't add it to the collection, i.e., do nothing here.
        }

        public void Clear()
        {
            this.hostList.Clear();
        }

        public bool Contains(string item)
        {            
            foreach (string hostName in this.hostList)
            {
                if (item.Contains(hostName))
                {
                    return true;
                }
            }

            return false;
        }

        public void CopyTo(string[] array, int arrayIndex)
        {
            this.hostList.CopyTo(array, arrayIndex);
        }

        public IEnumerator<string> GetEnumerator()
        {
            return this.hostList.GetEnumerator();
        }

        public bool Remove(string item)
        {
            return this.hostList.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.hostList.GetEnumerator();
        }
        #endregion

        private void AddIfNotExist(string hostName)
        {
            if (string.IsNullOrEmpty(hostName))
            {
                throw new ArgumentNullException(nameof(hostName));
            }

            if (!this.Contains(hostName))
            {
                this.hostList.Add(hostName);
            }
        }
    }
}