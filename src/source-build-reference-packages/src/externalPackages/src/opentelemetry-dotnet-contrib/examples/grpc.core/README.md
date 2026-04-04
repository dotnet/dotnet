# Grpc.Core Instrumentation for OpenTelemetry .NET - Examples

Project structure:

* Examples.GrpcCore.AspNetCore

  This is an all-in-one sample that shows how to use the client and server
  interceptors. The main project is a vanilla webapi project created
  using dotnet new ...

  When launched, the project will hit the <http://localhost:5000/weather>
  endpoint, make a call to a local in-process gRPC Core service running at
  localhost:5001.

  The end result is 3 spans dumped via the console exporter. A server span for
  the request to <http://localhost:5000/weather>, a client span for the outbound
  gRPC call and another server span for the gRPC Core server itself.
