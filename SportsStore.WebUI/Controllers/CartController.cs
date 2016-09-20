using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        private IProductRepository repository;
        private IOrderProcessor orderProcessor;
        public CartController(IProductRepository repo, IOrderProcessor orderProcessor)
        {
            repository = repo;
            this.orderProcessor = orderProcessor;

        }

        public ViewResult Index(Cart cart, string returnURL)
        {
            return View(new CartIndexViewModel {
                Cart = cart,
                returnURL = returnURL
            });
        }

        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }

        public ViewResult Checkout(Cart cart)
        {
                return View(new ShippingDetail());            
        }

        [HttpPost]
        public ViewResult Checkout(ShippingDetail shippingDetails, Cart cart)
        {
            if (cart.AllCartLines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, Your Cart is empty");
            }
            if (ModelState.IsValid)
            {
                orderProcessor.ProcessOrder(cart, shippingDetails);
                cart.ClearCart();
                return View("Completed");
            }
            else
            {
                return View(shippingDetails);
            }
                
        }

        public RedirectToRouteResult AddToCart(Cart cart,int productID, string returnURL)
        {
            Product product = repository.Products
                            .FirstOrDefault(p => p.ProductID == productID);
            if (product != null)
            {
                cart.AddItem(product,1);
            }

            return RedirectToAction("Index", new { returnURL });
                            
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int productID, string returnURL)
        {
            Product product = repository.Products
                .FirstOrDefault(p => p.ProductID == productID);
            if (product != null)
            {
                cart.RemoveLine(product);
            }

            return RedirectToAction("Index", new { returnURL });
        }
    }
}