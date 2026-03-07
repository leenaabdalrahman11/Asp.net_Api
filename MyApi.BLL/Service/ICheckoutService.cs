using System;
using MyApi.DAL.Models;
using MyApi.DAL.DTO;
using MyApi.DAL.DTO.Response;
using MyApi.DAL.DTO.Requests;

namespace MyApi.BLL.Service;

public interface ICheckoutService
{
 Task<CheckoutResponse> ProccesPaymentAsync(CheckoutRequest request, string userId);
 Task<CheckoutResponse> HandleSuccessAsync(string sessionId);

}

