﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stock.API.Application.Commands;
using Stock.API.Application.Queries;
using Stock.API.Infrastructure.Services;

namespace Stock.API.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize]
    [ApiController]
    public class OrdersController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IOrderQueries _orderQueries;
        private readonly IIdentityService _identityService;

        public OrdersController(IMediator mediator, IOrderQueries orderQueries, IIdentityService identityService)
        {

            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _orderQueries = orderQueries ?? throw new ArgumentNullException(nameof(orderQueries));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        }

        [Route("cancel")]
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CancelOrder([FromBody]CancelOrderCommand command, [FromHeader(Name = "x-requestid")] string requestId)
        {
            bool commandResult = false;
            if (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty)
            {
                var requestCancelOrder = new IdentifiedCommand<CancelOrderCommand, bool>(command, guid);
                commandResult = await _mediator.Send(requestCancelOrder);
            }

            return commandResult ? (IActionResult)Ok() : (IActionResult)BadRequest();

        }

        [Route("{orderId:int}")]
        [HttpGet]
        [ProducesResponseType(typeof(Order), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetOrder(int orderId)
        {
            try
            {
                var order = await _orderQueries
                    .GetOrderAsync(orderId);

                return Ok(order);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [Route("")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OrderSummary>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetOrders()
        {
            var userid = _identityService.GetUserIdentity();
            var orders = await _orderQueries.GetOrdersFromUserAsync(Guid.Parse(userid));
            return Ok(orders);
        }

        [Route("cardtypes")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CardType>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCardTypes()
        {
            var cardTypes = await _orderQueries
                .GetCardTypesAsync();

            return Ok(cardTypes);
        }

        [Route("draft")]
        [HttpPost]
        public async Task<IActionResult> GetOrderDraftFromBasketData([FromBody] CreateOrderDraftCommand createOrderDraftCommand)
        {
            var draft = await _mediator.Send(createOrderDraftCommand);
            return Ok(draft);
        }
    }
}