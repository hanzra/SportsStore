using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStore.Domain.Abstract;
using Moq;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using System.Linq;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void can_Add_Product_To_Cart()
        {
            //Arrange

            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] { new Product { ProductID = 1, Name = "Apple" },}.AsQueryable());

            Cart cart = new Cart();

            CartController target = new CartController(mock.Object,null);

            target.AddToCart(cart, 1, null);

            Assert.AreEqual(cart.AllCartLines.Count(), 1);
            Assert.AreEqual(cart.AllCartLines.ToArray()[0].Product.ProductID, 1);
            
        }

    }
}
