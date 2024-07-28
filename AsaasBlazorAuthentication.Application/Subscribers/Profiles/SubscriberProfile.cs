using AutoMapper;
using AsaasBlazorAuthentication.Domain.Subscribers;
using AsaasBlazorAuthentication.Application.Subscribers.Models;
using AsaasBlazorAuthentication.Application.Subscribers.UpdateSubscriber;

namespace AsaasBlazorAuthentication.Application.Subscribers.Profiles;

internal sealed class SubscriberProfile : Profile
{
    public SubscriberProfile()
    {
        CreateMap<Subscriber, SubscriberDetailsViewModel>();
        CreateMap<Subscriber, SubscriberViewModel>();
        CreateMap<UpdateSubscriberInputModel, UpdateSubscriberCommand>();
    }
}