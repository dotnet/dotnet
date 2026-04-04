<!-- markdownlint-disable MD022 MD024 MD031 MD033 -->

# Test results for ASP.NET Core 9

## Tracing

| http.route | App | Test Name |
| - | - | - |
| :broken_heart: | ConventionalRouting | [Root path](#activity__conventionalrouting-root-path) |
| :broken_heart: | ConventionalRouting | [Non-default action with route parameter and query string](#activity__conventionalrouting-non-default-action-with-route-parameter-and-query-string) |
| :broken_heart: | ConventionalRouting | [Non-default action with query string](#activity__conventionalrouting-non-default-action-with-query-string) |
| :green_heart: | ConventionalRouting | [Not Found (404)](#activity__conventionalrouting-not-found-404) |
| :green_heart: | ConventionalRouting | [Route template with parameter constraint](#activity__conventionalrouting-route-template-with-parameter-constraint) |
| :green_heart: | ConventionalRouting | [Path that does not match parameter constraint](#activity__conventionalrouting-path-that-does-not-match-parameter-constraint) |
| :broken_heart: | ConventionalRouting | [Area using `area:exists`, default controller/action](#activity__conventionalrouting-area-using-areaexists-default-controlleraction) |
| :broken_heart: | ConventionalRouting | [Area using `area:exists`, non-default action](#activity__conventionalrouting-area-using-areaexists-non-default-action) |
| :broken_heart: | ConventionalRouting | [Area w/o `area:exists`, default controller/action](#activity__conventionalrouting-area-wo-areaexists-default-controlleraction) |
| :green_heart: | AttributeRouting | [Default action](#activity__attributerouting-default-action) |
| :green_heart: | AttributeRouting | [Action without parameter](#activity__attributerouting-action-without-parameter) |
| :green_heart: | AttributeRouting | [Action with parameter](#activity__attributerouting-action-with-parameter) |
| :green_heart: | AttributeRouting | [Action with parameter before action name in template](#activity__attributerouting-action-with-parameter-before-action-name-in-template) |
| :green_heart: | AttributeRouting | [Action invoked resulting in 400 Bad Request](#activity__attributerouting-action-invoked-resulting-in-400-bad-request) |
| :broken_heart: | RazorPages | [Root path](#activity__razorpages-root-path) |
| :broken_heart: | RazorPages | [Index page](#activity__razorpages-index-page) |
| :broken_heart: | RazorPages | [Throws exception](#activity__razorpages-throws-exception) |
| :green_heart: | RazorPages | [Static content](#activity__razorpages-static-content) |
| :green_heart: | MinimalApi | [Action without parameter](#activity__minimalapi-action-without-parameter) |
| :green_heart: | MinimalApi | [Action with parameter](#activity__minimalapi-action-with-parameter) |
| :green_heart: | MinimalApi | [Action without parameter (MapGroup)](#activity__minimalapi-action-without-parameter-mapgroup) |
| :green_heart: | MinimalApi | [Action with parameter (MapGroup)](#activity__minimalapi-action-with-parameter-mapgroup) |
| :green_heart: | ExceptionMiddleware | [Exception Handled by Exception Handler Middleware](#activity__exceptionmiddleware-exception-handled-by-exception-handler-middleware) |

## Metrics

| http.route | App | Test Name |
| - | - | - |
| :broken_heart: | ConventionalRouting | [Root path](#metrics__conventionalrouting-root-path) |
| :broken_heart: | ConventionalRouting | [Non-default action with route parameter and query string](#metrics__conventionalrouting-non-default-action-with-route-parameter-and-query-string) |
| :broken_heart: | ConventionalRouting | [Non-default action with query string](#metrics__conventionalrouting-non-default-action-with-query-string) |
| :green_heart: | ConventionalRouting | [Not Found (404)](#metrics__conventionalrouting-not-found-404) |
| :green_heart: | ConventionalRouting | [Route template with parameter constraint](#metrics__conventionalrouting-route-template-with-parameter-constraint) |
| :green_heart: | ConventionalRouting | [Path that does not match parameter constraint](#metrics__conventionalrouting-path-that-does-not-match-parameter-constraint) |
| :broken_heart: | ConventionalRouting | [Area using `area:exists`, default controller/action](#metrics__conventionalrouting-area-using-areaexists-default-controlleraction) |
| :broken_heart: | ConventionalRouting | [Area using `area:exists`, non-default action](#metrics__conventionalrouting-area-using-areaexists-non-default-action) |
| :broken_heart: | ConventionalRouting | [Area w/o `area:exists`, default controller/action](#metrics__conventionalrouting-area-wo-areaexists-default-controlleraction) |
| :green_heart: | AttributeRouting | [Default action](#metrics__attributerouting-default-action) |
| :green_heart: | AttributeRouting | [Action without parameter](#metrics__attributerouting-action-without-parameter) |
| :green_heart: | AttributeRouting | [Action with parameter](#metrics__attributerouting-action-with-parameter) |
| :green_heart: | AttributeRouting | [Action with parameter before action name in template](#metrics__attributerouting-action-with-parameter-before-action-name-in-template) |
| :green_heart: | AttributeRouting | [Action invoked resulting in 400 Bad Request](#metrics__attributerouting-action-invoked-resulting-in-400-bad-request) |
| :broken_heart: | RazorPages | [Root path](#metrics__razorpages-root-path) |
| :broken_heart: | RazorPages | [Index page](#metrics__razorpages-index-page) |
| :broken_heart: | RazorPages | [Throws exception](#metrics__razorpages-throws-exception) |
| :green_heart: | RazorPages | [Static content](#metrics__razorpages-static-content) |
| :green_heart: | MinimalApi | [Action without parameter](#metrics__minimalapi-action-without-parameter) |
| :green_heart: | MinimalApi | [Action with parameter](#metrics__minimalapi-action-with-parameter) |
| :green_heart: | MinimalApi | [Action without parameter (MapGroup)](#metrics__minimalapi-action-without-parameter-mapgroup) |
| :green_heart: | MinimalApi | [Action with parameter (MapGroup)](#metrics__minimalapi-action-with-parameter-mapgroup) |
| :green_heart: | ExceptionMiddleware | [Exception Handled by Exception Handler Middleware](#metrics__exceptionmiddleware-exception-handled-by-exception-handler-middleware) |

## Tracing tests details

<a name="activity__conventionalrouting-root-path"></a>
## ConventionalRouting: Root path

```json
{
  "ActivityDisplayName": "GET {controller=ConventionalRoute}/{action=Default}/{id?}",
  "ActivityHttpRoute": "{controller=ConventionalRoute}/{action=Default}/{id?}",
  "IdealHttpRoute": "ConventionalRoute/Default/{id?}",
  "RouteInfo": {
    "HttpMethod": "GET",
    "Path": "/",
    "RoutePattern.RawText": "{controller=ConventionalRoute}/{action=Default}/{id?}",
    "IRouteDiagnosticsMetadata.Route": "{controller=ConventionalRoute}/{action=Default}/{id?}",
    "HttpContext.GetRouteData()": {
      "controller": "ConventionalRoute",
      "action": "Default"
    },
    "ActionDescriptor": {
      "AttributeRouteInfo.Template": null,
      "Parameters": [],
      "ControllerActionDescriptor": {
        "ControllerName": "ConventionalRoute",
        "ActionName": "Default"
      },
      "PageActionDescriptor": null
    }
  }
}
```
<a name="activity__conventionalrouting-non-default-action-with-route-parameter-and-query-string"></a>
## ConventionalRouting: Non-default action with route parameter and query string

```json
{
  "ActivityDisplayName": "GET {controller=ConventionalRoute}/{action=Default}/{id?}",
  "ActivityHttpRoute": "{controller=ConventionalRoute}/{action=Default}/{id?}",
  "IdealHttpRoute": "ConventionalRoute/ActionWithStringParameter/{id?}",
  "RouteInfo": {
    "HttpMethod": "GET",
    "Path": "/ConventionalRoute/ActionWithStringParameter/2?num=3",
    "RoutePattern.RawText": "{controller=ConventionalRoute}/{action=Default}/{id?}",
    "IRouteDiagnosticsMetadata.Route": "{controller=ConventionalRoute}/{action=Default}/{id?}",
    "HttpContext.GetRouteData()": {
      "controller": "ConventionalRoute",
      "action": "ActionWithStringParameter",
      "id": "2"
    },
    "ActionDescriptor": {
      "AttributeRouteInfo.Template": null,
      "Parameters": [
        "id",
        "num"
      ],
      "ControllerActionDescriptor": {
        "ControllerName": "ConventionalRoute",
        "ActionName": "ActionWithStringParameter"
      },
      "PageActionDescriptor": null
    }
  }
}
```
<a name="activity__conventionalrouting-non-default-action-with-query-string"></a>
## ConventionalRouting: Non-default action with query string

```json
{
  "ActivityDisplayName": "GET {controller=ConventionalRoute}/{action=Default}/{id?}",
  "ActivityHttpRoute": "{controller=ConventionalRoute}/{action=Default}/{id?}",
  "IdealHttpRoute": "ConventionalRoute/ActionWithStringParameter/{id?}",
  "RouteInfo": {
    "HttpMethod": "GET",
    "Path": "/ConventionalRoute/ActionWithStringParameter?num=3",
    "RoutePattern.RawText": "{controller=ConventionalRoute}/{action=Default}/{id?}",
    "IRouteDiagnosticsMetadata.Route": "{controller=ConventionalRoute}/{action=Default}/{id?}",
    "HttpContext.GetRouteData()": {
      "controller": "ConventionalRoute",
      "action": "ActionWithStringParameter"
    },
    "ActionDescriptor": {
      "AttributeRouteInfo.Template": null,
      "Parameters": [
        "id",
        "num"
      ],
      "ControllerActionDescriptor": {
        "ControllerName": "ConventionalRoute",
        "ActionName": "ActionWithStringParameter"
      },
      "PageActionDescriptor": null
    }
  }
}
```
<a name="activity__conventionalrouting-not-found-404"></a>
## ConventionalRouting: Not Found (404)

```json
{
  "ActivityDisplayName": "GET",
  "ActivityHttpRoute": "",
  "IdealHttpRoute": "",
  "RouteInfo": {
    "HttpMethod": "GET",
    "Path": "/ConventionalRoute/NotFound",
    "RoutePattern.RawText": null,
    "IRouteDiagnosticsMetadata.Route": null,
    "HttpContext.GetRouteData()": {},
    "ActionDescriptor": null
  }
}
```
<a name="activity__conventionalrouting-route-template-with-parameter-constraint"></a>
## ConventionalRouting: Route template with parameter constraint

```json
{
  "ActivityDisplayName": "GET SomePath/{id}/{num:int}",
  "ActivityHttpRoute": "SomePath/{id}/{num:int}",
  "IdealHttpRoute": "SomePath/{id}/{num:int}",
  "RouteInfo": {
    "HttpMethod": "GET",
    "Path": "/SomePath/SomeString/2",
    "RoutePattern.RawText": "SomePath/{id}/{num:int}",
    "IRouteDiagnosticsMetadata.Route": "SomePath/{id}/{num:int}",
    "HttpContext.GetRouteData()": {
      "controller": "ConventionalRoute",
      "action": "ActionWithStringParameter",
      "id": "SomeString",
      "num": "2"
    },
    "ActionDescriptor": {
      "AttributeRouteInfo.Template": null,
      "Parameters": [
        "id",
        "num"
      ],
      "ControllerActionDescriptor": {
        "ControllerName": "ConventionalRoute",
        "ActionName": "ActionWithStringParameter"
      },
      "PageActionDescriptor": null
    }
  }
}
```
<a name="activity__conventionalrouting-path-that-does-not-match-parameter-constraint"></a>
## ConventionalRouting: Path that does not match parameter constraint

```json
{
  "ActivityDisplayName": "GET",
  "ActivityHttpRoute": "",
  "IdealHttpRoute": "",
  "RouteInfo": {
    "HttpMethod": "GET",
    "Path": "/SomePath/SomeString/NotAnInt",
    "RoutePattern.RawText": null,
    "IRouteDiagnosticsMetadata.Route": null,
    "HttpContext.GetRouteData()": {},
    "ActionDescriptor": null
  }
}
```
<a name="activity__conventionalrouting-area-using-areaexists-default-controlleraction"></a>
## ConventionalRouting: Area using `area:exists`, default controller/action

```json
{
  "ActivityDisplayName": "GET {area:exists}/{controller=ControllerForMyArea}/{action=Default}/{id?}",
  "ActivityHttpRoute": "{area:exists}/{controller=ControllerForMyArea}/{action=Default}/{id?}",
  "IdealHttpRoute": "{area:exists}/ControllerForMyArea/Default/{id?}",
  "RouteInfo": {
    "HttpMethod": "GET",
    "Path": "/MyArea",
    "RoutePattern.RawText": "{area:exists}/{controller=ControllerForMyArea}/{action=Default}/{id?}",
    "IRouteDiagnosticsMetadata.Route": "{area:exists}/{controller=ControllerForMyArea}/{action=Default}/{id?}",
    "HttpContext.GetRouteData()": {
      "controller": "ControllerForMyArea",
      "action": "Default",
      "area": "MyArea"
    },
    "ActionDescriptor": {
      "AttributeRouteInfo.Template": null,
      "Parameters": [],
      "ControllerActionDescriptor": {
        "ControllerName": "ControllerForMyArea",
        "ActionName": "Default"
      },
      "PageActionDescriptor": null
    }
  }
}
```
<a name="activity__conventionalrouting-area-using-areaexists-non-default-action"></a>
## ConventionalRouting: Area using `area:exists`, non-default action

```json
{
  "ActivityDisplayName": "GET {area:exists}/{controller=ControllerForMyArea}/{action=Default}/{id?}",
  "ActivityHttpRoute": "{area:exists}/{controller=ControllerForMyArea}/{action=Default}/{id?}",
  "IdealHttpRoute": "{area:exists}/ControllerForMyArea/NonDefault/{id?}",
  "RouteInfo": {
    "HttpMethod": "GET",
    "Path": "/MyArea/ControllerForMyArea/NonDefault",
    "RoutePattern.RawText": "{area:exists}/{controller=ControllerForMyArea}/{action=Default}/{id?}",
    "IRouteDiagnosticsMetadata.Route": "{area:exists}/{controller=ControllerForMyArea}/{action=Default}/{id?}",
    "HttpContext.GetRouteData()": {
      "controller": "ControllerForMyArea",
      "area": "MyArea",
      "action": "NonDefault"
    },
    "ActionDescriptor": {
      "AttributeRouteInfo.Template": null,
      "Parameters": [],
      "ControllerActionDescriptor": {
        "ControllerName": "ControllerForMyArea",
        "ActionName": "NonDefault"
      },
      "PageActionDescriptor": null
    }
  }
}
```
<a name="activity__conventionalrouting-area-wo-areaexists-default-controlleraction"></a>
## ConventionalRouting: Area w/o `area:exists`, default controller/action

```json
{
  "ActivityDisplayName": "GET SomePrefix/{controller=AnotherArea}/{action=Index}/{id?}",
  "ActivityHttpRoute": "SomePrefix/{controller=AnotherArea}/{action=Index}/{id?}",
  "IdealHttpRoute": "SomePrefix/AnotherArea/Index/{id?}",
  "RouteInfo": {
    "HttpMethod": "GET",
    "Path": "/SomePrefix",
    "RoutePattern.RawText": "SomePrefix/{controller=AnotherArea}/{action=Index}/{id?}",
    "IRouteDiagnosticsMetadata.Route": "SomePrefix/{controller=AnotherArea}/{action=Index}/{id?}",
    "HttpContext.GetRouteData()": {
      "area": "AnotherArea",
      "controller": "AnotherArea",
      "action": "Index"
    },
    "ActionDescriptor": {
      "AttributeRouteInfo.Template": null,
      "Parameters": [],
      "ControllerActionDescriptor": {
        "ControllerName": "AnotherArea",
        "ActionName": "Index"
      },
      "PageActionDescriptor": null
    }
  }
}
```
<a name="activity__attributerouting-default-action"></a>
## AttributeRouting: Default action

```json
{
  "ActivityDisplayName": "GET AttributeRoute",
  "ActivityHttpRoute": "AttributeRoute",
  "IdealHttpRoute": "AttributeRoute",
  "RouteInfo": {
    "HttpMethod": "GET",
    "Path": "/AttributeRoute",
    "RoutePattern.RawText": "AttributeRoute",
    "IRouteDiagnosticsMetadata.Route": "AttributeRoute",
    "HttpContext.GetRouteData()": {
      "action": "Get",
      "controller": "AttributeRoute"
    },
    "ActionDescriptor": {
      "AttributeRouteInfo.Template": "AttributeRoute",
      "Parameters": [],
      "ControllerActionDescriptor": {
        "ControllerName": "AttributeRoute",
        "ActionName": "Get"
      },
      "PageActionDescriptor": null
    }
  }
}
```
<a name="activity__attributerouting-action-without-parameter"></a>
## AttributeRouting: Action without parameter

```json
{
  "ActivityDisplayName": "GET AttributeRoute/Get",
  "ActivityHttpRoute": "AttributeRoute/Get",
  "IdealHttpRoute": "AttributeRoute/Get",
  "RouteInfo": {
    "HttpMethod": "GET",
    "Path": "/AttributeRoute/Get",
    "RoutePattern.RawText": "AttributeRoute/Get",
    "IRouteDiagnosticsMetadata.Route": "AttributeRoute/Get",
    "HttpContext.GetRouteData()": {
      "action": "Get",
      "controller": "AttributeRoute"
    },
    "ActionDescriptor": {
      "AttributeRouteInfo.Template": "AttributeRoute/Get",
      "Parameters": [],
      "ControllerActionDescriptor": {
        "ControllerName": "AttributeRoute",
        "ActionName": "Get"
      },
      "PageActionDescriptor": null
    }
  }
}
```
<a name="activity__attributerouting-action-with-parameter"></a>
## AttributeRouting: Action with parameter

```json
{
  "ActivityDisplayName": "GET AttributeRoute/Get/{id}",
  "ActivityHttpRoute": "AttributeRoute/Get/{id}",
  "IdealHttpRoute": "AttributeRoute/Get/{id}",
  "RouteInfo": {
    "HttpMethod": "GET",
    "Path": "/AttributeRoute/Get/12",
    "RoutePattern.RawText": "AttributeRoute/Get/{id}",
    "IRouteDiagnosticsMetadata.Route": "AttributeRoute/Get/{id}",
    "HttpContext.GetRouteData()": {
      "action": "Get",
      "controller": "AttributeRoute",
      "id": "12"
    },
    "ActionDescriptor": {
      "AttributeRouteInfo.Template": "AttributeRoute/Get/{id}",
      "Parameters": [
        "id"
      ],
      "ControllerActionDescriptor": {
        "ControllerName": "AttributeRoute",
        "ActionName": "Get"
      },
      "PageActionDescriptor": null
    }
  }
}
```
<a name="activity__attributerouting-action-with-parameter-before-action-name-in-template"></a>
## AttributeRouting: Action with parameter before action name in template

```json
{
  "ActivityDisplayName": "GET AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate",
  "ActivityHttpRoute": "AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate",
  "IdealHttpRoute": "AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate",
  "RouteInfo": {
    "HttpMethod": "GET",
    "Path": "/AttributeRoute/12/GetWithActionNameInDifferentSpotInTemplate",
    "RoutePattern.RawText": "AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate",
    "IRouteDiagnosticsMetadata.Route": "AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate",
    "HttpContext.GetRouteData()": {
      "action": "GetWithActionNameInDifferentSpotInTemplate",
      "controller": "AttributeRoute",
      "id": "12"
    },
    "ActionDescriptor": {
      "AttributeRouteInfo.Template": "AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate",
      "Parameters": [
        "id"
      ],
      "ControllerActionDescriptor": {
        "ControllerName": "AttributeRoute",
        "ActionName": "GetWithActionNameInDifferentSpotInTemplate"
      },
      "PageActionDescriptor": null
    }
  }
}
```
<a name="activity__attributerouting-action-invoked-resulting-in-400-bad-request"></a>
## AttributeRouting: Action invoked resulting in 400 Bad Request

```json
{
  "ActivityDisplayName": "GET AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate",
  "ActivityHttpRoute": "AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate",
  "IdealHttpRoute": "AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate",
  "RouteInfo": {
    "HttpMethod": "GET",
    "Path": "/AttributeRoute/NotAnInt/GetWithActionNameInDifferentSpotInTemplate",
    "RoutePattern.RawText": "AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate",
    "IRouteDiagnosticsMetadata.Route": "AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate",
    "HttpContext.GetRouteData()": {
      "action": "GetWithActionNameInDifferentSpotInTemplate",
      "controller": "AttributeRoute",
      "id": "NotAnInt"
    },
    "ActionDescriptor": {
      "AttributeRouteInfo.Template": "AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate",
      "Parameters": [
        "id"
      ],
      "ControllerActionDescriptor": {
        "ControllerName": "AttributeRoute",
        "ActionName": "GetWithActionNameInDifferentSpotInTemplate"
      },
      "PageActionDescriptor": null
    }
  }
}
```
<a name="activity__razorpages-root-path"></a>
## RazorPages: Root path

```json
{
  "ActivityDisplayName": "GET",
  "ActivityHttpRoute": "",
  "IdealHttpRoute": "/Index",
  "RouteInfo": {
    "HttpMethod": "GET",
    "Path": "/",
    "RoutePattern.RawText": "",
    "IRouteDiagnosticsMetadata.Route": "",
    "HttpContext.GetRouteData()": {
      "page": "/Index"
    },
    "ActionDescriptor": {
      "AttributeRouteInfo.Template": "",
      "Parameters": [],
      "ControllerActionDescriptor": null,
      "PageActionDescriptor": {
        "RelativePath": "/Pages/Index.cshtml",
        "ViewEnginePath": "/Index"
      }
    }
  }
}
```
<a name="activity__razorpages-index-page"></a>
## RazorPages: Index page

```json
{
  "ActivityDisplayName": "GET Index",
  "ActivityHttpRoute": "Index",
  "IdealHttpRoute": "/Index",
  "RouteInfo": {
    "HttpMethod": "GET",
    "Path": "/Index",
    "RoutePattern.RawText": "Index",
    "IRouteDiagnosticsMetadata.Route": "Index",
    "HttpContext.GetRouteData()": {
      "page": "/Index"
    },
    "ActionDescriptor": {
      "AttributeRouteInfo.Template": "Index",
      "Parameters": [],
      "ControllerActionDescriptor": null,
      "PageActionDescriptor": {
        "RelativePath": "/Pages/Index.cshtml",
        "ViewEnginePath": "/Index"
      }
    }
  }
}
```
<a name="activity__razorpages-throws-exception"></a>
## RazorPages: Throws exception

```json
{
  "ActivityDisplayName": "GET PageThatThrowsException",
  "ActivityHttpRoute": "PageThatThrowsException",
  "IdealHttpRoute": "/PageThatThrowsException",
  "RouteInfo": {
    "HttpMethod": "GET",
    "Path": "/PageThatThrowsException",
    "RoutePattern.RawText": "PageThatThrowsException",
    "IRouteDiagnosticsMetadata.Route": "PageThatThrowsException",
    "HttpContext.GetRouteData()": {
      "page": "/PageThatThrowsException"
    },
    "ActionDescriptor": {
      "AttributeRouteInfo.Template": "PageThatThrowsException",
      "Parameters": [],
      "ControllerActionDescriptor": null,
      "PageActionDescriptor": {
        "RelativePath": "/Pages/PageThatThrowsException.cshtml",
        "ViewEnginePath": "/PageThatThrowsException"
      }
    }
  }
}
```
<a name="activity__razorpages-static-content"></a>
## RazorPages: Static content

```json
{
  "ActivityDisplayName": "GET",
  "ActivityHttpRoute": "",
  "IdealHttpRoute": "",
  "RouteInfo": {
    "HttpMethod": "GET",
    "Path": "/js/site.js",
    "RoutePattern.RawText": null,
    "IRouteDiagnosticsMetadata.Route": null,
    "HttpContext.GetRouteData()": {},
    "ActionDescriptor": null
  }
}
```
<a name="activity__minimalapi-action-without-parameter"></a>
## MinimalApi: Action without parameter

```json
{
  "ActivityDisplayName": "GET /MinimalApi",
  "ActivityHttpRoute": "/MinimalApi",
  "IdealHttpRoute": "/MinimalApi",
  "RouteInfo": {
    "HttpMethod": "GET",
    "Path": "/MinimalApi",
    "RoutePattern.RawText": "/MinimalApi",
    "IRouteDiagnosticsMetadata.Route": "/MinimalApi",
    "HttpContext.GetRouteData()": {},
    "ActionDescriptor": null
  }
}
```
<a name="activity__minimalapi-action-with-parameter"></a>
## MinimalApi: Action with parameter

```json
{
  "ActivityDisplayName": "GET /MinimalApi/{id}",
  "ActivityHttpRoute": "/MinimalApi/{id}",
  "IdealHttpRoute": "/MinimalApi/{id}",
  "RouteInfo": {
    "HttpMethod": "GET",
    "Path": "/MinimalApi/123",
    "RoutePattern.RawText": "/MinimalApi/{id}",
    "IRouteDiagnosticsMetadata.Route": "/MinimalApi/{id}",
    "HttpContext.GetRouteData()": {
      "id": "123"
    },
    "ActionDescriptor": null
  }
}
```
<a name="activity__minimalapi-action-without-parameter-mapgroup"></a>
## MinimalApi: Action without parameter (MapGroup)

```json
{
  "ActivityDisplayName": "GET /MinimalApiUsingMapGroup/",
  "ActivityHttpRoute": "/MinimalApiUsingMapGroup/",
  "IdealHttpRoute": "/MinimalApiUsingMapGroup/",
  "RouteInfo": {
    "HttpMethod": "GET",
    "Path": "/MinimalApiUsingMapGroup",
    "RoutePattern.RawText": "/MinimalApiUsingMapGroup/",
    "IRouteDiagnosticsMetadata.Route": "/MinimalApiUsingMapGroup/",
    "HttpContext.GetRouteData()": {},
    "ActionDescriptor": null
  }
}
```
<a name="activity__minimalapi-action-with-parameter-mapgroup"></a>
## MinimalApi: Action with parameter (MapGroup)

```json
{
  "ActivityDisplayName": "GET /MinimalApiUsingMapGroup/{id}",
  "ActivityHttpRoute": "/MinimalApiUsingMapGroup/{id}",
  "IdealHttpRoute": "/MinimalApiUsingMapGroup/{id}",
  "RouteInfo": {
    "HttpMethod": "GET",
    "Path": "/MinimalApiUsingMapGroup/123",
    "RoutePattern.RawText": "/MinimalApiUsingMapGroup/{id}",
    "IRouteDiagnosticsMetadata.Route": "/MinimalApiUsingMapGroup/{id}",
    "HttpContext.GetRouteData()": {
      "id": "123"
    },
    "ActionDescriptor": null
  }
}
```
<a name="activity__exceptionmiddleware-exception-handled-by-exception-handler-middleware"></a>
## ExceptionMiddleware: Exception Handled by Exception Handler Middleware

```json
{
  "ActivityDisplayName": "GET /Exception",
  "ActivityHttpRoute": "/Exception",
  "IdealHttpRoute": "/Exception",
  "RouteInfo": {
    "HttpMethod": "GET",
    "Path": "/Exception",
    "RoutePattern.RawText": "/Exception",
    "IRouteDiagnosticsMetadata.Route": "/Exception",
    "HttpContext.GetRouteData()": {},
    "ActionDescriptor": null
  }
}
```

## Metrics tests details

<a name="metrics__conventionalrouting-root-path"></a>
## ConventionalRouting: Root path

```json
{
  "MetricHttpRoute": "{controller=ConventionalRoute}/{action=Default}/{id?}",
  "IdealHttpRoute": "ConventionalRoute/Default/{id?}",
  "RouteInfo": {
    "HttpMethod": "GET",
    "Path": "/",
    "RoutePattern.RawText": "{controller=ConventionalRoute}/{action=Default}/{id?}",
    "IRouteDiagnosticsMetadata.Route": "{controller=ConventionalRoute}/{action=Default}/{id?}",
    "HttpContext.GetRouteData()": {
      "controller": "ConventionalRoute",
      "action": "Default"
    },
    "ActionDescriptor": {
      "AttributeRouteInfo.Template": null,
      "Parameters": [],
      "ControllerActionDescriptor": {
        "ControllerName": "ConventionalRoute",
        "ActionName": "Default"
      },
      "PageActionDescriptor": null
    }
  }
}
```
<a name="metrics__conventionalrouting-non-default-action-with-route-parameter-and-query-string"></a>
## ConventionalRouting: Non-default action with route parameter and query string

```json
{
  "MetricHttpRoute": "{controller=ConventionalRoute}/{action=Default}/{id?}",
  "IdealHttpRoute": "ConventionalRoute/ActionWithStringParameter/{id?}",
  "RouteInfo": {
    "HttpMethod": "GET",
    "Path": "/ConventionalRoute/ActionWithStringParameter/2?num=3",
    "RoutePattern.RawText": "{controller=ConventionalRoute}/{action=Default}/{id?}",
    "IRouteDiagnosticsMetadata.Route": "{controller=ConventionalRoute}/{action=Default}/{id?}",
    "HttpContext.GetRouteData()": {
      "controller": "ConventionalRoute",
      "action": "ActionWithStringParameter",
      "id": "2"
    },
    "ActionDescriptor": {
      "AttributeRouteInfo.Template": null,
      "Parameters": [
        "id",
        "num"
      ],
      "ControllerActionDescriptor": {
        "ControllerName": "ConventionalRoute",
        "ActionName": "ActionWithStringParameter"
      },
      "PageActionDescriptor": null
    }
  }
}
```
<a name="metrics__conventionalrouting-non-default-action-with-query-string"></a>
## ConventionalRouting: Non-default action with query string

```json
{
  "MetricHttpRoute": "{controller=ConventionalRoute}/{action=Default}/{id?}",
  "IdealHttpRoute": "ConventionalRoute/ActionWithStringParameter/{id?}",
  "RouteInfo": {
    "HttpMethod": "GET",
    "Path": "/ConventionalRoute/ActionWithStringParameter?num=3",
    "RoutePattern.RawText": "{controller=ConventionalRoute}/{action=Default}/{id?}",
    "IRouteDiagnosticsMetadata.Route": "{controller=ConventionalRoute}/{action=Default}/{id?}",
    "HttpContext.GetRouteData()": {
      "controller": "ConventionalRoute",
      "action": "ActionWithStringParameter"
    },
    "ActionDescriptor": {
      "AttributeRouteInfo.Template": null,
      "Parameters": [
        "id",
        "num"
      ],
      "ControllerActionDescriptor": {
        "ControllerName": "ConventionalRoute",
        "ActionName": "ActionWithStringParameter"
      },
      "PageActionDescriptor": null
    }
  }
}
```
<a name="metrics__conventionalrouting-not-found-404"></a>
## ConventionalRouting: Not Found (404)

```json
{
  "MetricHttpRoute": "",
  "IdealHttpRoute": "",
  "RouteInfo": {
    "HttpMethod": "GET",
    "Path": "/ConventionalRoute/NotFound",
    "RoutePattern.RawText": null,
    "IRouteDiagnosticsMetadata.Route": null,
    "HttpContext.GetRouteData()": {},
    "ActionDescriptor": null
  }
}
```
<a name="metrics__conventionalrouting-route-template-with-parameter-constraint"></a>
## ConventionalRouting: Route template with parameter constraint

```json
{
  "MetricHttpRoute": "SomePath/{id}/{num:int}",
  "IdealHttpRoute": "SomePath/{id}/{num:int}",
  "RouteInfo": {
    "HttpMethod": "GET",
    "Path": "/SomePath/SomeString/2",
    "RoutePattern.RawText": "SomePath/{id}/{num:int}",
    "IRouteDiagnosticsMetadata.Route": "SomePath/{id}/{num:int}",
    "HttpContext.GetRouteData()": {
      "controller": "ConventionalRoute",
      "action": "ActionWithStringParameter",
      "id": "SomeString",
      "num": "2"
    },
    "ActionDescriptor": {
      "AttributeRouteInfo.Template": null,
      "Parameters": [
        "id",
        "num"
      ],
      "ControllerActionDescriptor": {
        "ControllerName": "ConventionalRoute",
        "ActionName": "ActionWithStringParameter"
      },
      "PageActionDescriptor": null
    }
  }
}
```
<a name="metrics__conventionalrouting-path-that-does-not-match-parameter-constraint"></a>
## ConventionalRouting: Path that does not match parameter constraint

```json
{
  "MetricHttpRoute": "",
  "IdealHttpRoute": "",
  "RouteInfo": {
    "HttpMethod": "GET",
    "Path": "/SomePath/SomeString/NotAnInt",
    "RoutePattern.RawText": null,
    "IRouteDiagnosticsMetadata.Route": null,
    "HttpContext.GetRouteData()": {},
    "ActionDescriptor": null
  }
}
```
<a name="metrics__conventionalrouting-area-using-areaexists-default-controlleraction"></a>
## ConventionalRouting: Area using `area:exists`, default controller/action

```json
{
  "MetricHttpRoute": "{area:exists}/{controller=ControllerForMyArea}/{action=Default}/{id?}",
  "IdealHttpRoute": "{area:exists}/ControllerForMyArea/Default/{id?}",
  "RouteInfo": {
    "HttpMethod": "GET",
    "Path": "/MyArea",
    "RoutePattern.RawText": "{area:exists}/{controller=ControllerForMyArea}/{action=Default}/{id?}",
    "IRouteDiagnosticsMetadata.Route": "{area:exists}/{controller=ControllerForMyArea}/{action=Default}/{id?}",
    "HttpContext.GetRouteData()": {
      "controller": "ControllerForMyArea",
      "action": "Default",
      "area": "MyArea"
    },
    "ActionDescriptor": {
      "AttributeRouteInfo.Template": null,
      "Parameters": [],
      "ControllerActionDescriptor": {
        "ControllerName": "ControllerForMyArea",
        "ActionName": "Default"
      },
      "PageActionDescriptor": null
    }
  }
}
```
<a name="metrics__conventionalrouting-area-using-areaexists-non-default-action"></a>
## ConventionalRouting: Area using `area:exists`, non-default action

```json
{
  "MetricHttpRoute": "{area:exists}/{controller=ControllerForMyArea}/{action=Default}/{id?}",
  "IdealHttpRoute": "{area:exists}/ControllerForMyArea/NonDefault/{id?}",
  "RouteInfo": {
    "HttpMethod": "GET",
    "Path": "/MyArea/ControllerForMyArea/NonDefault",
    "RoutePattern.RawText": "{area:exists}/{controller=ControllerForMyArea}/{action=Default}/{id?}",
    "IRouteDiagnosticsMetadata.Route": "{area:exists}/{controller=ControllerForMyArea}/{action=Default}/{id?}",
    "HttpContext.GetRouteData()": {
      "controller": "ControllerForMyArea",
      "area": "MyArea",
      "action": "NonDefault"
    },
    "ActionDescriptor": {
      "AttributeRouteInfo.Template": null,
      "Parameters": [],
      "ControllerActionDescriptor": {
        "ControllerName": "ControllerForMyArea",
        "ActionName": "NonDefault"
      },
      "PageActionDescriptor": null
    }
  }
}
```
<a name="metrics__conventionalrouting-area-wo-areaexists-default-controlleraction"></a>
## ConventionalRouting: Area w/o `area:exists`, default controller/action

```json
{
  "MetricHttpRoute": "SomePrefix/{controller=AnotherArea}/{action=Index}/{id?}",
  "IdealHttpRoute": "SomePrefix/AnotherArea/Index/{id?}",
  "RouteInfo": {
    "HttpMethod": "GET",
    "Path": "/SomePrefix",
    "RoutePattern.RawText": "SomePrefix/{controller=AnotherArea}/{action=Index}/{id?}",
    "IRouteDiagnosticsMetadata.Route": "SomePrefix/{controller=AnotherArea}/{action=Index}/{id?}",
    "HttpContext.GetRouteData()": {
      "area": "AnotherArea",
      "controller": "AnotherArea",
      "action": "Index"
    },
    "ActionDescriptor": {
      "AttributeRouteInfo.Template": null,
      "Parameters": [],
      "ControllerActionDescriptor": {
        "ControllerName": "AnotherArea",
        "ActionName": "Index"
      },
      "PageActionDescriptor": null
    }
  }
}
```
<a name="metrics__attributerouting-default-action"></a>
## AttributeRouting: Default action

```json
{
  "MetricHttpRoute": "AttributeRoute",
  "IdealHttpRoute": "AttributeRoute",
  "RouteInfo": {
    "HttpMethod": "GET",
    "Path": "/AttributeRoute",
    "RoutePattern.RawText": "AttributeRoute",
    "IRouteDiagnosticsMetadata.Route": "AttributeRoute",
    "HttpContext.GetRouteData()": {
      "action": "Get",
      "controller": "AttributeRoute"
    },
    "ActionDescriptor": {
      "AttributeRouteInfo.Template": "AttributeRoute",
      "Parameters": [],
      "ControllerActionDescriptor": {
        "ControllerName": "AttributeRoute",
        "ActionName": "Get"
      },
      "PageActionDescriptor": null
    }
  }
}
```
<a name="metrics__attributerouting-action-without-parameter"></a>
## AttributeRouting: Action without parameter

```json
{
  "MetricHttpRoute": "AttributeRoute/Get",
  "IdealHttpRoute": "AttributeRoute/Get",
  "RouteInfo": {
    "HttpMethod": "GET",
    "Path": "/AttributeRoute/Get",
    "RoutePattern.RawText": "AttributeRoute/Get",
    "IRouteDiagnosticsMetadata.Route": "AttributeRoute/Get",
    "HttpContext.GetRouteData()": {
      "action": "Get",
      "controller": "AttributeRoute"
    },
    "ActionDescriptor": {
      "AttributeRouteInfo.Template": "AttributeRoute/Get",
      "Parameters": [],
      "ControllerActionDescriptor": {
        "ControllerName": "AttributeRoute",
        "ActionName": "Get"
      },
      "PageActionDescriptor": null
    }
  }
}
```
<a name="metrics__attributerouting-action-with-parameter"></a>
## AttributeRouting: Action with parameter

```json
{
  "MetricHttpRoute": "AttributeRoute/Get/{id}",
  "IdealHttpRoute": "AttributeRoute/Get/{id}",
  "RouteInfo": {
    "HttpMethod": "GET",
    "Path": "/AttributeRoute/Get/12",
    "RoutePattern.RawText": "AttributeRoute/Get/{id}",
    "IRouteDiagnosticsMetadata.Route": "AttributeRoute/Get/{id}",
    "HttpContext.GetRouteData()": {
      "action": "Get",
      "controller": "AttributeRoute",
      "id": "12"
    },
    "ActionDescriptor": {
      "AttributeRouteInfo.Template": "AttributeRoute/Get/{id}",
      "Parameters": [
        "id"
      ],
      "ControllerActionDescriptor": {
        "ControllerName": "AttributeRoute",
        "ActionName": "Get"
      },
      "PageActionDescriptor": null
    }
  }
}
```
<a name="metrics__attributerouting-action-with-parameter-before-action-name-in-template"></a>
## AttributeRouting: Action with parameter before action name in template

```json
{
  "MetricHttpRoute": "AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate",
  "IdealHttpRoute": "AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate",
  "RouteInfo": {
    "HttpMethod": "GET",
    "Path": "/AttributeRoute/12/GetWithActionNameInDifferentSpotInTemplate",
    "RoutePattern.RawText": "AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate",
    "IRouteDiagnosticsMetadata.Route": "AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate",
    "HttpContext.GetRouteData()": {
      "action": "GetWithActionNameInDifferentSpotInTemplate",
      "controller": "AttributeRoute",
      "id": "12"
    },
    "ActionDescriptor": {
      "AttributeRouteInfo.Template": "AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate",
      "Parameters": [
        "id"
      ],
      "ControllerActionDescriptor": {
        "ControllerName": "AttributeRoute",
        "ActionName": "GetWithActionNameInDifferentSpotInTemplate"
      },
      "PageActionDescriptor": null
    }
  }
}
```
<a name="metrics__attributerouting-action-invoked-resulting-in-400-bad-request"></a>
## AttributeRouting: Action invoked resulting in 400 Bad Request

```json
{
  "MetricHttpRoute": "AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate",
  "IdealHttpRoute": "AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate",
  "RouteInfo": {
    "HttpMethod": "GET",
    "Path": "/AttributeRoute/NotAnInt/GetWithActionNameInDifferentSpotInTemplate",
    "RoutePattern.RawText": "AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate",
    "IRouteDiagnosticsMetadata.Route": "AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate",
    "HttpContext.GetRouteData()": {
      "action": "GetWithActionNameInDifferentSpotInTemplate",
      "controller": "AttributeRoute",
      "id": "NotAnInt"
    },
    "ActionDescriptor": {
      "AttributeRouteInfo.Template": "AttributeRoute/{id}/GetWithActionNameInDifferentSpotInTemplate",
      "Parameters": [
        "id"
      ],
      "ControllerActionDescriptor": {
        "ControllerName": "AttributeRoute",
        "ActionName": "GetWithActionNameInDifferentSpotInTemplate"
      },
      "PageActionDescriptor": null
    }
  }
}
```
<a name="metrics__razorpages-root-path"></a>
## RazorPages: Root path

```json
{
  "MetricHttpRoute": "",
  "IdealHttpRoute": "/Index",
  "RouteInfo": {
    "HttpMethod": "GET",
    "Path": "/",
    "RoutePattern.RawText": "",
    "IRouteDiagnosticsMetadata.Route": "",
    "HttpContext.GetRouteData()": {
      "page": "/Index"
    },
    "ActionDescriptor": {
      "AttributeRouteInfo.Template": "",
      "Parameters": [],
      "ControllerActionDescriptor": null,
      "PageActionDescriptor": {
        "RelativePath": "/Pages/Index.cshtml",
        "ViewEnginePath": "/Index"
      }
    }
  }
}
```
<a name="metrics__razorpages-index-page"></a>
## RazorPages: Index page

```json
{
  "MetricHttpRoute": "Index",
  "IdealHttpRoute": "/Index",
  "RouteInfo": {
    "HttpMethod": "GET",
    "Path": "/Index",
    "RoutePattern.RawText": "Index",
    "IRouteDiagnosticsMetadata.Route": "Index",
    "HttpContext.GetRouteData()": {
      "page": "/Index"
    },
    "ActionDescriptor": {
      "AttributeRouteInfo.Template": "Index",
      "Parameters": [],
      "ControllerActionDescriptor": null,
      "PageActionDescriptor": {
        "RelativePath": "/Pages/Index.cshtml",
        "ViewEnginePath": "/Index"
      }
    }
  }
}
```
<a name="metrics__razorpages-throws-exception"></a>
## RazorPages: Throws exception

```json
{
  "MetricHttpRoute": "PageThatThrowsException",
  "IdealHttpRoute": "/PageThatThrowsException",
  "RouteInfo": {
    "HttpMethod": "GET",
    "Path": "/PageThatThrowsException",
    "RoutePattern.RawText": "PageThatThrowsException",
    "IRouteDiagnosticsMetadata.Route": "PageThatThrowsException",
    "HttpContext.GetRouteData()": {
      "page": "/PageThatThrowsException"
    },
    "ActionDescriptor": {
      "AttributeRouteInfo.Template": "PageThatThrowsException",
      "Parameters": [],
      "ControllerActionDescriptor": null,
      "PageActionDescriptor": {
        "RelativePath": "/Pages/PageThatThrowsException.cshtml",
        "ViewEnginePath": "/PageThatThrowsException"
      }
    }
  }
}
```
<a name="metrics__razorpages-static-content"></a>
## RazorPages: Static content

```json
{
  "MetricHttpRoute": "",
  "IdealHttpRoute": "",
  "RouteInfo": {
    "HttpMethod": "GET",
    "Path": "/js/site.js",
    "RoutePattern.RawText": null,
    "IRouteDiagnosticsMetadata.Route": null,
    "HttpContext.GetRouteData()": {},
    "ActionDescriptor": null
  }
}
```
<a name="metrics__minimalapi-action-without-parameter"></a>
## MinimalApi: Action without parameter

```json
{
  "MetricHttpRoute": "/MinimalApi",
  "IdealHttpRoute": "/MinimalApi",
  "RouteInfo": {
    "HttpMethod": "GET",
    "Path": "/MinimalApi",
    "RoutePattern.RawText": "/MinimalApi",
    "IRouteDiagnosticsMetadata.Route": "/MinimalApi",
    "HttpContext.GetRouteData()": {},
    "ActionDescriptor": null
  }
}
```
<a name="metrics__minimalapi-action-with-parameter"></a>
## MinimalApi: Action with parameter

```json
{
  "MetricHttpRoute": "/MinimalApi/{id}",
  "IdealHttpRoute": "/MinimalApi/{id}",
  "RouteInfo": {
    "HttpMethod": "GET",
    "Path": "/MinimalApi/123",
    "RoutePattern.RawText": "/MinimalApi/{id}",
    "IRouteDiagnosticsMetadata.Route": "/MinimalApi/{id}",
    "HttpContext.GetRouteData()": {
      "id": "123"
    },
    "ActionDescriptor": null
  }
}
```
<a name="metrics__minimalapi-action-without-parameter-mapgroup"></a>
## MinimalApi: Action without parameter (MapGroup)

```json
{
  "MetricHttpRoute": "/MinimalApiUsingMapGroup/",
  "IdealHttpRoute": "/MinimalApiUsingMapGroup/",
  "RouteInfo": {
    "HttpMethod": "GET",
    "Path": "/MinimalApiUsingMapGroup",
    "RoutePattern.RawText": "/MinimalApiUsingMapGroup/",
    "IRouteDiagnosticsMetadata.Route": "/MinimalApiUsingMapGroup/",
    "HttpContext.GetRouteData()": {},
    "ActionDescriptor": null
  }
}
```
<a name="metrics__minimalapi-action-with-parameter-mapgroup"></a>
## MinimalApi: Action with parameter (MapGroup)

```json
{
  "MetricHttpRoute": "/MinimalApiUsingMapGroup/{id}",
  "IdealHttpRoute": "/MinimalApiUsingMapGroup/{id}",
  "RouteInfo": {
    "HttpMethod": "GET",
    "Path": "/MinimalApiUsingMapGroup/123",
    "RoutePattern.RawText": "/MinimalApiUsingMapGroup/{id}",
    "IRouteDiagnosticsMetadata.Route": "/MinimalApiUsingMapGroup/{id}",
    "HttpContext.GetRouteData()": {
      "id": "123"
    },
    "ActionDescriptor": null
  }
}
```
<a name="metrics__exceptionmiddleware-exception-handled-by-exception-handler-middleware"></a>
## ExceptionMiddleware: Exception Handled by Exception Handler Middleware

```json
{
  "MetricHttpRoute": "/Exception",
  "IdealHttpRoute": "/Exception",
  "RouteInfo": {
    "HttpMethod": "GET",
    "Path": "/Exception",
    "RoutePattern.RawText": "/Exception",
    "IRouteDiagnosticsMetadata.Route": "/Exception",
    "HttpContext.GetRouteData()": {},
    "ActionDescriptor": null
  }
}
```
