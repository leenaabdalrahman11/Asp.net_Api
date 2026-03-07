using System;
using MyApi.DAL.Repository;
namespace MyApi.BLL.Service;

using MyApi.DAL.DTO.Response;
using MyApi.DAL.DTO.Requests;
using MyApi.DAL.Models;
using Stripe;
using Stripe.Checkout;
using Microsoft.AspNetCore.Identity;

public class CheckoutService : ICheckoutService
{
	private readonly ICartRepository _cartRepository;
	private readonly IOrderRepository _orderRepository;
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly IEmailSender _emailSender;
	private readonly IOrderItemRepository _orderItemRepository;
	private readonly IProductRepository _productRepository;
	public CheckoutService(ICartRepository cartRepository, IOrderRepository orderRepository,
	UserManager<ApplicationUser> userManager, IEmailSender emailSender,
	IOrderItemRepository orderItemRepository, IProductRepository productRepository)
	{
		_cartRepository = cartRepository;
		_orderRepository = orderRepository;
		_userManager = userManager;
		_emailSender = emailSender;
		_orderItemRepository = orderItemRepository;
		_orderItemRepository = orderItemRepository;
		_productRepository = productRepository;
	}
	public async Task<CheckoutResponse> ProccesPaymentAsync(CheckoutRequest request, string userId)
	{

		var CartItems = await _cartRepository.GetCartItemsByUserIdAsync(userId);
		if (!CartItems.Any())
		{
			return new CheckoutResponse
			{
				IsSuccess = false,
				Message = "Cart is empty."
			};
		}
		decimal totalAmount = 0;

		foreach (var item in CartItems)
		{
			if (item.Product.Quantity <= item.Count)
			{
				return new CheckoutResponse
				{
					IsSuccess = false,
					Message = $"Invalid quantity for product {item.ProductId}."
				};
			}
			totalAmount += item.Product.Price * item.Count;
		}
		DAL.Models.Order order = new DAL.Models.Order
		{
			UserId = userId,
			paymentMethod = request.PaymentMethod,
			AmountPaid = totalAmount,
			PaymentStatus = PaymentStatus.Unpaid,
		};
		if (request.PaymentMethod == DAL.Models.PaymentMethod.Cash)
		{
			return new CheckoutResponse
			{
				IsSuccess = true,
				Message = "Cash on delivery selected.",
			};
		}
		else if (request.PaymentMethod == DAL.Models.PaymentMethod.Visa)
		{
			var options = new SessionCreateOptions
			{
				PaymentMethodTypes = new List<string> { "card" },
				LineItems = new List<SessionLineItemOptions>(),
				Mode = "payment",

				SuccessUrl = "https://localhost:7291/api/checkout/success?session_id={CHECKOUT_SESSION_ID}",
				CancelUrl = "https://localhost:7291/api/checkout/cancel",

				Metadata = new Dictionary<string, string>
				{
					["userId"] = userId
				}
			};
			foreach (var item in CartItems)
			{
				options.LineItems.Add(new SessionLineItemOptions
				{
					PriceData = new SessionLineItemPriceDataOptions
					{
						Currency = "USD",
						ProductData = new SessionLineItemPriceDataProductDataOptions
						{
							Name = item.Product.Translations.FirstOrDefault(t => t.Language == "en")?.Name,
						},
						UnitAmount = (long)(item.Product.Price * 100),
					},
					Quantity = item.Count,
				});
			}
			var service = new SessionService();
			var session = service.Create(options);
			order.SessionId = session.Id;
			order.PaymentStatus = PaymentStatus.Paid;
			await _orderRepository.CreateOrderAsync(order);
			return new CheckoutResponse
			{
				IsSuccess = true,
				Message = "Checkout session created.",
				Url = session.Url,
				PaymentId = session.Id
			};
		}
		else
		{
			return new CheckoutResponse
			{
				IsSuccess = false,
				Message = "Invalid payment method."
			};
		}

	}
	public async Task<CheckoutResponse> HandleSuccessAsync(string sessionId)
	{
		var service = new SessionService();
		var session = service.Get(sessionId);
		var userId = session.Metadata["userId"];
		var order = await _orderRepository.GetBySessionIdAsync(sessionId);
		if (order == null)
		{
			return new CheckoutResponse
			{
				IsSuccess = false,
				Message = "Order not found."
			};
		}
		order.PaymentId = session.PaymentIntentId;
		order.OrderStatus = OrderStatus.Approved; 
		await _orderRepository.UpdateAsync(order);
		var user = await _userManager.FindByIdAsync(userId);
		var CartItems = await _cartRepository.GetCartItemsByUserIdAsync(userId);
		var orderItems = new List<OrderItem>();
		var productUpdated = new List<(int productId, int quantity)>();
		foreach (var item in CartItems)	{
			var orderItem = new OrderItem
			{
				OrderId = order.Id,
			    ProductId = item.ProductId,
				UnitPrice = item.Product.Price,
				Quantity = item.Count,
				TotalPrice = item.Product.Price * item.Count
			};
			orderItems.Add(orderItem);
			productUpdated.Add((item.ProductId, item.Count));
		}
		await _orderItemRepository.CreateRangeAsync(orderItems);
		await _cartRepository.ClearCartAsync(userId);
		await _productRepository.DecreaseQuantityAsync(productUpdated);

		await _emailSender.SendEmailAsync(user.Email, "Order Confirmation", "<h2>Your order has been confirmed!</h2><p>Thank you for your purchase.</p>");

		return new CheckoutResponse
		{
			IsSuccess = true,
			Message = "Payment successful."
		};
	}
}
