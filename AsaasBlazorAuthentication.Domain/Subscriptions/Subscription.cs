﻿using AsaasBlazorAuthentication.Common.Results;
using AsaasBlazorAuthentication.Common.Entities;
using AsaasBlazorAuthentication.Domain.Enrollments;

namespace AsaasBlazorAuthentication.Domain.Subscriptions;

public sealed class Subscription : BaseEntity
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public int Duration { get; private set; }

    public List<Enrollment> Enrollments { get; private set; } = [];

    protected Subscription() { }

    private Subscription(
        string name,
        string description,
        int duration)
    {
        Name = name;
        Description = description;
        Duration = duration;
    }

    public static Result<Subscription> Create(
        string name,
        string description,
        int duration)
    {
        var subscription =
            new Subscription(
                name,
                description,
                duration);

        return Result<Subscription>.Ok(subscription);
    }

    public void Update(string name, string description, int duration)
    {
        Name = name;
        Description = description;
        Duration = duration;
    }
}