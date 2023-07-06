using BirdTrading.Domain.Models;
using BirdTrading.Interface;
using BirdTrading.Repository.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BirdTradingApp.Pages.Products
{
    public class ProductShopModel : PageModel
    {
        private IUnitOfWork _unitOfWork;
        public Shop Shop { get; set; }
        public List<Product> Products { get; set; }
        public List<List<Product>> ProductBatch { get; set; } = new List<List<Product>> { new List<Product>() };
        public ProductShopModel(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }
        public void OnGet(int id)
        {
            Shop = _unitOfWork.ShopRepository.GetShopById(id);
            Products = _unitOfWork.ProductRepository.GetProductByShopId(id);
            Product[] products = new Product[Products.Count];
            products = Products.ToArray();
            int r = products.Length % 3;
            for(int i = 0; i < products.Length -r; i+=3)
            {
              List<Product> list = new List<Product>();
                list.Add(products[i]);
                list.Add(products[i+1]);
                list.Add(products[i+2]);
                ProductBatch.Add(list);
            }
            if(r > 0)
            {
                List<Product> list = new List<Product>();
                for(int i = 1; i <= r; i++)
                {
                    list.Add(Products[products.Length - i]);  
                }
                ProductBatch.Add(list);
            }
        }
    }
}
