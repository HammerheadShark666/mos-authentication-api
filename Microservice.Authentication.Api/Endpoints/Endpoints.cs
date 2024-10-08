﻿using Asp.Versioning;
using MediatR;
using Microservice.Authentication.Api.Extensions;
using Microservice.Authentication.Api.MediatR.AuthenticateUser;
using Microsoft.OpenApi.Models;
using System.Net;

namespace Microservice.Authentication.Api.Endpoints;

public static class Endpoints
{
    public static void ConfigureRoutes(this WebApplication webApplication)
    {
        var authenticateGroup = webApplication.MapGroup("v{version:apiVersion}").WithTags("authenticate");

        authenticateGroup.MapPost("/login", async (AuthenticateUserRequest authenticateUserRequest, IMediator mediator) =>
        {
            var authenticateUserResponse = await mediator.Send(authenticateUserRequest);
            return Results.Ok(authenticateUserResponse);
        })
        .Accepts<AuthenticateUserRequest>("application/json")
        .Produces<AuthenticateUserResponse>((int)HttpStatusCode.OK)
        .Produces<AuthenticateUserErrorResponse>((int)HttpStatusCode.BadRequest)
        .WithName("AuthenticateUser")
        .WithApiVersionSet(webApplication.GetApiVersionSet())
        .MapToApiVersion(new ApiVersion(1, 0))
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Authenticate a user",
            Description = "Authenticates a user and returns a token if valid",
            Tags = [new() { Name = "Microservice Order System - Authenticate" }]
        });
    }
}