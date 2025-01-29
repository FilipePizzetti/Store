using Flunt.Notifications;
using Store.Domain.Commands;
using Store.Domain.Commands.Interfaces;
using Store.Domain.Entities;
using Store.Domain.Handlers.Interfaces;
using Store.Domain.Repositories;
using Store.Domain.Utils;

namespace Store.Domain.Handlers
{
    public class OrderHandler : Notifiable, IHandler<CreateOrderCommand>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IDeliveryFeeRepository _feeRepository;
        private readonly IProductRepository _productRepository;
        private readonly IDiscountRepository _discountRepository;

        public OrderHandler(ICustomerRepository customerRepository,
                            IOrderRepository orderRepository,
                            IDeliveryFeeRepository feeRepository,
                            IProductRepository productRepository,
                            IDiscountRepository discountRepository)
        {
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
            _feeRepository = feeRepository;
            _productRepository = productRepository;
            _discountRepository = discountRepository;
        }

        public ICommandResult Handle(CreateOrderCommand command)
        {
            command.Validate();
            if (command.Invalid)
                return new GenericCommandResult(false, "Pedido invalido", command.Notifications);

            var customer = _customerRepository.Get(command.Customer);

            var deliveryFee = _feeRepository.Get(command.ZipCode);

            var discount = _discountRepository.Get(command.PromoCode);

            var products = _productRepository.Get(ExtractGuids.Extract(command.Items)).ToList();
            var order = new Order(customer, deliveryFee, discount);
            foreach (var item in command.Items)
            {
                var product = products.Where(x => x.Id == item.Product).FirstOrDefault();
                order.Additem(product, item.Quantity);

            }

            AddNotifications(order.Notifications);

            if (Valid)
                return new GenericCommandResult(false, "Falha ao gerar pedido", Notifications);

            _orderRepository.Save(order);
            return new GenericCommandResult(true, $"Pedido {order.Number} gerado com sucesso", order);

        }
    }
}
