using Store.Domain.Entities;
using Store.Domain.Enums;

namespace Store.Tests.Entities
{
    [TestClass]
    public class OrderTests
    {
        private readonly Customer _customer = new Customer("Filipe", "Filipe@gmail.com");
        private readonly Product _product = new Product("Produto 1", 10, true);
        private readonly Discount _discount = new Discount(10, DateTime.Now.AddDays(5));

        [TestMethod]
        [TestCategory("Domain")]
        public void NovoPedidoDeveGerarNumeroCom8Caracteres()
        {
            var order = new Order(_customer, 0, null);
            Assert.AreEqual(8, order.Number.Length);
        }
        [TestMethod]
        [TestCategory("Domain")]
        public void NovoPedidoStatusDeveSerAguardandoPagamento()
        {
            var order = new Order(_customer, 0, null);
            Assert.AreEqual(EOrderStatus.WaitingPayment, order.Status);
        }
        [TestMethod]
        [TestCategory("Domain")]
        public void PedidoPagoStatusDeveSerAguardandoEntrega()
        {
            var order = new Order(_customer, 0, null);
            order.Additem(_product, 2);
            order.Pay(20);
            Assert.AreEqual(EOrderStatus.WaitingDelivery, order.Status);
        }
        [TestMethod]
        [TestCategory("Domain")]
        public void PedidoCanceladoStatusDeveSerCancelado()
        {
            var order = new Order(_customer, 0, null);
            order.Cancel();
            Assert.AreEqual(EOrderStatus.Cancelled, order.Status);
        }

        [TestMethod]
        [TestCategory("Domain")]
        public void PedidoSemProdutoNaoDeveSerAdicionado()
        {
            var order = new Order(_customer, 0, null);
            order.Additem(null, 2);
            Assert.AreEqual(order.Items.Count, 0);
        }

        [TestMethod]
        [TestCategory("Domain")]
        public void QuantidadeDeItensDeveSerMaiorQueZero()
        {
            var order = new Order(_customer, 0, null);
            order.Additem(_product, 0);
            Assert.AreEqual(order.Items.Count, 0);
        }

        [TestMethod]
        [TestCategory("Domain")]
        public void NovoPedidoTotalDeveSer50()
        {
            var order = new Order(_customer, 0, null);
            order.Additem(_product, 5);
            Assert.AreEqual(order.Total(), 50);
        }

        [TestMethod]
        [TestCategory("Domain")]
        public void DadoUmDescontoExpiradoOValorDoPedidoDeveSer60()
        {
            var discount = new Discount(10, DateTime.Now.AddDays(-1));
            var order = new Order(_customer, 0, discount);
            order.Additem(_product, 6);
            Assert.AreEqual(order.Total(), 60);
        }
    }
}
