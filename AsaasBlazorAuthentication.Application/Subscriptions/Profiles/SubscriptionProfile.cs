using AutoMapper;
using AsaasBlazorAuthentication.Domain.Subscriptions;
using AsaasBlazorAuthentication.Application.Subscriptions.UpdateSubscription;
using AsaasBlazorAuthentication.Application.Subscriptions.Models;

namespace AsaasBlazorAuthentication.Application.Subscriptions.Profiles;

internal sealed class SubscriptionProfile : Profile
{
    public SubscriptionProfile()
    {
        CreateMap<Subscription, SubscriptionDetailsViewModel>();
        CreateMap<Subscription, SubscriptionViewModel>();
        CreateMap<UpdateSubscriptionInputModel, UpdateSubscriptionCommand>();
    }
}