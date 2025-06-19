using AutoMapper;
using IlmPath.Application.Payments.Commands.CreateCheckoutSession;
using IlmPath.Application.Payments.DTOs.Requests;
using IlmPath.Application.Payments.DTOs.Responses;

namespace IlmPath.Application.Mappings;

public class PaymentMappings : Profile
{
    public PaymentMappings()
    {
        // Request DTO to Command (UserId will be added from controller context)
        CreateMap<CreateCheckoutSessionRequest, CreateCheckoutSessionCommand>()
            .ConstructUsing((src, context) => new CreateCheckoutSessionCommand(
                "", // UserId will be set in controller
                src.SuccessUrl,
                src.CancelUrl));

        // Command result (string URL) to Response DTO
        CreateMap<string, CreateCheckoutSessionResponse>()
            .ConstructUsing((url, context) => new CreateCheckoutSessionResponse { CheckoutUrl = url, SessionId = context.Items["sessionId"] as string });
    }
} 