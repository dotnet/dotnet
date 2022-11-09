// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace vstest.ProgrammerTests.Fakes;

internal class RequestResponsePair<TRequest, TResponse, TContext> where TRequest : class
{
    public RequestResponsePair(TRequest request, TResponse response, bool debug = false)
    {
        Request = request;
        Responses = new List<TResponse> { response };
        Debug = debug;
    }

    public RequestResponsePair(TRequest request, IEnumerable<TResponse> responses, bool debug = false)
    {
        Request = request;
        Responses = responses.ToList();
        Debug = debug;
    }

    public RequestResponsePair(TRequest request, IEnumerable<TResponse> responses, Action<TContext>? beforeAction = null, Action<TContext>? afterAction = null, bool debug = false)
    {
        Request = request;
        Responses = responses.ToList();
        BeforeAction = beforeAction;
        AfterAction = afterAction;
        Debug = debug;
    }

    public TRequest Request { get; }

    // TODO: make this Expression< so we can get some info about what this is doing when looking directly at this instance
    public Action<TContext>? BeforeAction { get; }
    public Action<TContext>? AfterAction { get; }
    public List<TResponse> Responses { get; }
    public bool Debug { get; }

    public override string ToString()
    {
        return $"\u2193{Request} \u2191{Responses.FirstOrDefault()}";
    }
}
