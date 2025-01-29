using Store.Domain.Commands;
using Store.Domain.Handlers;
using Store.Domain.Repositories;
using Store.Tests.Repositories;

namespace Store.Tests.Handlers
{
    [TestClass]
    public class OrderHandlerTests
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IDeliveryFeeRepository _feeRepository;
        private readonly IProductRepository _productRepository;
        private readonly IDiscountRepository _discountRepository;
        public OrderHandlerTests()
        {
            _customerRepository = new FakeCustomerRepository();
            _orderRepository = new FakeOrderRepository();
            _feeRepository = new FakeDeliveryFeeRepository();
            _productRepository = new FakeProductRepository();
            _discountRepository = new FakeDiscountRepository();
        }
        [TestMethod]
        [TestCategory("Handlers")]
        public void ComandoValidoDeveGerarPedido()
        {
            var command = new CreateOrderCommand();
            command.Customer = "teste";
            command.ZipCode = "88802050";
            command.PromoCode = "12346578";
            command.Items.Add(new CreateOrderItemCommand(Guid.NewGuid(), 1));
            command.Items.Add(new CreateOrderItemCommand(Guid.NewGuid(), 1));

            var handler = new OrderHandler(
                _customerRepository,
                _orderRepository,
                _feeRepository,
                _productRepository,
                _discountRepository
                );
            handler.Handle(command);
            Assert.AreEqual(handler.Valid, true);
        }
    }
}
