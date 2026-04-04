// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using TestApp.AspNetCore;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen();

        builder.Services.AddMvc();

        builder.Services.AddSignalR();

        builder.Services.AddSingleton<HttpClient>();

        builder.Services.AddSingleton(
            new TestCallbackMiddleware());

        builder.Services.AddSingleton(
            new TestActivityMiddleware());

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.MapHub<TestHub>("/testHub");

        app.UseMiddleware<CallbackMiddleware>();

        app.UseMiddleware<ActivityMiddleware>();

        app.AddTestMiddleware();

        app.Run();
    }
}
