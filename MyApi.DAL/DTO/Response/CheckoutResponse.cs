using System;

namespace MyApi.DAL.DTO.Response;

public class CheckoutResponse : BaseResponse
{
    public string? Url { get; set; }
    public string? PaymentId { get; set; }

}
