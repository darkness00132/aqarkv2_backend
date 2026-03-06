using Application;
using Application.Exceptions;
using Infrastructure;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddCors(corsBuilder =>
{
    corsBuilder.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("https://localhost:3000")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler(errorApp =>
    {
        errorApp.Run(async context =>
        {
            var ex = context.Features.Get<IExceptionHandlerFeature>()?.Error;

            if (ex is ApiException apiEx)
            {
                context.Response.StatusCode = apiEx.StatusCode;

                await Results.Problem(
                    statusCode: apiEx.StatusCode,
                    title: "Request Failed",
                    detail: apiEx.Message
                ).ExecuteAsync(context);

                return;
            }

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            await Results.Problem(
                statusCode: 500,
                title: "Server Error"
            ).ExecuteAsync(context);

        });
    });

    app.UseStatusCodePages();
}


app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();