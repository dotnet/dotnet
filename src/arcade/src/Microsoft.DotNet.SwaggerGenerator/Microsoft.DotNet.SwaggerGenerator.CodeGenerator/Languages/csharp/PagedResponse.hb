using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azure;

namespace {{pascalCaseNs Namespace}}
{
    public static class AsyncEnumerable
    {
        public static async Task<List<T>> ToListAsync<T>(this IAsyncEnumerable<T> that, CancellationToken cancellationToken)
        {
            var results = new List<T>();
            await foreach (var value in that.WithCancellation(cancellationToken))
            {
                results.Add(value);
            }

            return results;
        }
    }

    public class AsyncPageable
    {
        public static AsyncPageable<T> Create<T>(Func<string, int?, IAsyncEnumerable<Page<T>>> pageFunc)
        {
            return new FuncAsyncPageable<T>(pageFunc);
        }

        public class FuncAsyncPageable<T> : AsyncPageable<T>
        {
            private readonly Func<string, int?, IAsyncEnumerable<Page<T>>> _pageFunc;

            public FuncAsyncPageable(Func<string, int?, IAsyncEnumerable<Page<T>>> pageFunc)
            {
                _pageFunc = pageFunc;
            }

            public override IAsyncEnumerable<Page<T>> AsPages(string continuationToken = null, int? pageSizeHint = null)
            {
                return _pageFunc(continuationToken, pageSizeHint);
            }
        }
    }

    public class LinkHeader
    {
        private LinkHeader(string firstPageLink, string prevPageLink, string nextPageLink, string lastPageLink)
        {
            FirstPageLink = firstPageLink;
            PrevPageLink = prevPageLink;
            NextPageLink = nextPageLink;
            LastPageLink = lastPageLink;
        }

        public string FirstPageLink { get; }
        public string PrevPageLink { get; }
        public string NextPageLink { get; }
        public string LastPageLink { get; }

        public static LinkHeader Parse(IEnumerable<string> header)
        {
            var links = ParseLinkHeader(header).ToList();
            var first = links.FirstOrDefault(t => t.rel == "first").href;
            var prev = links.FirstOrDefault(t => t.rel == "prev").href;
            var next = links.FirstOrDefault(t => t.rel == "next").href;
            var last = links.FirstOrDefault(t => t.rel == "last").href;
            return new LinkHeader(first, prev, next, last);
        }

        private static IEnumerable<(string href, string rel)> ParseLinkHeader(IEnumerable<string> linkHeader)
        {
            foreach (var header in linkHeader)
            {
                foreach (var link in ParseLinkHeader(header))
                {
                    yield return link;
                }
            }
        }

        private static IEnumerable<(string href, string rel)> ParseLinkHeader(string linkHeader)
        {
            foreach (var link in linkHeader.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries))
            {
                if (ParseLink(link, out var result))
                {
                    yield return result;
                }
            }
        }

        private static bool ParseLink(string link, out (string href, string rel) result)
        {
            result = default;
            var parts = link.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2)
            {
                return false;
            }

            var href = parts[0].Trim().TrimStart('<').TrimEnd('>');
            var props = new Dictionary<string, string>();
            foreach (var prop in parts.Skip(1))
            {
                if (TryParseProp(prop, out var p))
                {
                    props.Add(p.key, p.value);
                }
            }

            var rel = props["rel"];
            result = (href, rel);
            return true;
        }

        private static bool TryParseProp(string value, out (string key, string value) result)
        {
            result = default;
            var equalIdx = value.IndexOf('=');
            if (equalIdx < 0)
            {
                return false;
            }

            var key = value.Substring(0, equalIdx).Trim();
            var v = value.Substring(equalIdx + 1).Trim().Trim('"');
            result = (key, v);
            return true;
        }
    }
}
