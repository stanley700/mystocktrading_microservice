namespace Stock.API.Application.Commands
{
    using global::Stock.API.Application.Models;
    using MediatR;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using static Stock.API.Application.Commands.CreateOrderCommand;

    // Regular CommandHandler
    public class CreateOrderDraftCommandHandler
        : IRequestHandler<CreateOrderDraftCommand, OrderDraftDTO>
    {
        private readonly IMediator _mediator;

        public Task<OrderDraftDTO> Handle(CreateOrderDraftCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }


    public class OrderDraftDTO
    {
        public IEnumerable<OrderItemDTO> OrderItems { get; set; }
        public decimal Total { get; set; }
    }
}
