using IlmPath.Application.Carts.Commands;
using IlmPath.Application.Carts.DTOs.Request;
using IlmPath.Application.Carts.DTOs.Response;
using IlmPath.Application.Carts.Queries;
using IlmPath.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IlmPath.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class CartsController(IMediator _mediator) : ControllerBase
    {

        private string GetCurrentUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);


        [HttpGet]
        public async Task<ActionResult<CartResponse>> GetMyCart()
        {
            var userId = GetCurrentUserId();
            var query = new GetCartByUserIdQuery(userId);
            var cart = await _mediator.Send(query);
            return Ok(cart);
        }

        [HttpPost("items")]
        public async Task<ActionResult<Cart>> AddItemToMyCart([FromBody] AddItemToCartRequest request)
        {
            var userId = GetCurrentUserId();
            var command = new AddItemToCartCommand(userId, request.CourseId);
            var cart = await _mediator.Send(command);
            return Ok(cart);
        }

        [HttpDelete("items/{courseId:int}")]
        public async Task<ActionResult<Cart>> RemoveItemFromMyCart(int courseId)
        {
            var userId = GetCurrentUserId();
            var command = new RemoveItemFromCartCommand(userId, courseId);
            var cart = await _mediator.Send(command);
            return Ok(cart);
        }

    }
}
