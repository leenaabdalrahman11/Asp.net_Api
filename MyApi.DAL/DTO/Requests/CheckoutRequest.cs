using System;
using System.Text.Json.Serialization;
using MyApi.DAL.Models;

namespace MyApi.DAL.DTO.Requests;
public class CheckoutRequest
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PaymentMethod PaymentMethod { get; set; }

}
