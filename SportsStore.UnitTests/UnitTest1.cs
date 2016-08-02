using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System.Linq;
using SportsStore.Domain.Abstract;
using Moq;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using System.Collections.Generic;
using System.Linq;
using SportsStore.WebUI.Models;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product { ProductID = 1, Name = "P1"},
                new Product { ProductID = 2, Name = "P2"},
                new Product { ProductID = 3, Name = "P3"},
                new Product { ProductID = 4, Name = "P4"},
                new Product { ProductID = 5, Name = "P5"},
                new Product { ProductID = 6, Name = "P6"}});

            ProductController controller = new ProductController(mock.Object);
            controller.pageSize = 3;

           ProductsListViewModel result = (ProductsListViewModel)controller.List(null,2).Model;
            Product[] productArray = result.Products.ToArray();
            Assert.IsTrue(productArray.Length == 3);
            Assert.AreEqual(productArray[0].Name, "P4");

        }

        [TestMethod]
        public void Can_Send_Pagination_Model()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product { ProductID = 1, Name = "P1"},
                new Product { ProductID = 2, Name = "P2"},
                new Product { ProductID = 3, Name = "P3"},
                new Product { ProductID = 4, Name = "P4"},
                new Product { ProductID = 5, Name = "P5"},
                new Product { ProductID = 6, Name = "P6"}});

            ProductController controller = new ProductController(mock.Object);
            controller.pageSize = 3;



           ProductsListViewModel result = (ProductsListViewModel)controller.List(null,2).Model;

            Assert.AreEqual(result.PagingInfo.ItemsPerPage, 3);
            Assert.AreEqual(result.PagingInfo.TotalItems, 6);
            Assert.AreEqual(result.PagingInfo.TotalPages, 2);
            Assert.AreEqual(result.PagingInfo.CurrentPage, 2);

        }

        [TestMethod]
        public void Can_Filter_On_Category()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product { ProductID = 1, Name = "P1", Category="PC1"},
                new Product { ProductID = 2, Name = "P2", Category="PC2"},
                new Product { ProductID = 3, Name = "P3", Category="PC2"},
                new Product { ProductID = 4, Name = "P4", Category="PC3"},
                new Product { ProductID = 5, Name = "P5", Category="PC3"},
                new Product { ProductID = 6, Name = "P6", Category="PC3"}});

            ProductController controller = new ProductController(mock.Object);
            controller.pageSize = 3;

            ProductsListViewModel result = (ProductsListViewModel)controller.List("PC3", 1).Model;          

            Product[] productArray = result.Products.ToArray();

            Assert.AreEqual(productArray.Length, 3);
            Assert.AreEqual(productArray[0].Category, "PC3");

        }

        [TestMethod]
        public void Inicates_Selected_Category()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
               new Product {ProductID=1, Category = "Apple" },
               new Product {ProductID=2, Category = "Orange"}});

            NavController target = new NavController(mock.Object);

            string CategoryToSelect = "Apple";

            string result = target.Menu(CategoryToSelect).ViewBag.SelectedCategory;

            Assert.AreEqual(CategoryToSelect, result);

        }
    }
}
