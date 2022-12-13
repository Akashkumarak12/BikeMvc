﻿using BikeMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace DemoClient.Controllers
{
    public class ProductController : Controller
    {


        public async Task<IActionResult> Index()
        {
            List<Product> product = new List<Product>();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("https://localhost:44367/api/Product");
                if (Res.IsSuccessStatusCode)
                {
                    var response = Res.Content.ReadAsStringAsync().Result;
                    product = JsonConvert.DeserializeObject<List<Product>>(response);
                }
                return View(product);
            }
        }
        //public async Task<IActionResult> Search(DateTime date)
        //{
        //    List<MovieTbl> movie = new List<MovieTbl>();
        //    using (var client = new HttpClient())
        //    {
        //        client.DefaultRequestHeaders.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("applicaation/json"));
        //        HttpResponseMessage Res = await client.GetAsync("https://localhost:44341/api/Movie/" + date);
        //        if (Res.IsSuccessStatusCode)
        //        {
        //            var response = Res.Content.ReadAsStringAsync().Result;
        //            movie = JsonConvert.DeserializeObject<List<MovieTbl>>(response);
        //        }
        //        return View(movie);
        //    }
        //}
        public async Task<IActionResult> Details(int id)
        {
            Product m = new Product();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                using (var response = await client.GetAsync("https://localhost:44367/api/Product/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    m = JsonConvert.DeserializeObject<Product>(apiResponse);
                }
            }
            return View(m);
        }
        [HttpPost]
        public async Task<IActionResult> Search(string p)
        {
            List<Product> product = new List<Product>();
            Product p1= new Product();
            p1.ProductName = p;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                StringContent content = new StringContent(JsonConvert.SerializeObject(p1), Encoding.UTF8, "application/json");
                HttpResponseMessage Res = await client.PostAsync("https://localhost:44367/api/Product", content);
                //HttpResponseMessage Res = await client.GetAsync("https://localhost:44341/api/Movie?" + date);
                if (Res.IsSuccessStatusCode)
                {
                    var response = Res.Content.ReadAsStringAsync().Result;
                    product = JsonConvert.DeserializeObject<List<Product>>(response);
                }
                return View(product);
            }
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Product movietbl)
        {
            Product product = new Product();
            using (var client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(movietbl), Encoding.UTF8, "application/json");
                using (var response = await client.PostAsync("https://localhost:44367/api/Product/PostProduct", content))
                {
                    string apiresponse = await response.Content.ReadAsStringAsync();
                    product = JsonConvert.DeserializeObject<Product>(apiresponse);
                }
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int id)
        {
            TempData["Id"] = id;
            Product? m = new Product();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                using (var response = await client.GetAsync("https://localhost:44367/api/Product/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    m = JsonConvert.DeserializeObject<Product>(apiResponse);
                }
            }
            return View(m);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Product m)
        {
            int Id = Convert.ToInt32(TempData["Id"]);
            //Employee emp = new Employee();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                await client.DeleteAsync("https://localhost:44367/api/Product/" + Id);

            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int id)
        {
            Product? p = new Product();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:44367/api/Product/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    p = JsonConvert.DeserializeObject<Product>(apiResponse);
                }
            }
            return View(p);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            Product? p = new Product();

            using (var httpClient = new HttpClient())
            {
                int id = product.ProductId;
                httpClient.DefaultRequestHeaders.Clear();
                StringContent content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync("https://localhost:44367/api/Product", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    ViewBag.Result = "Success";
                    p = JsonConvert.DeserializeObject<Product>(apiResponse);
                }
            }
            return RedirectToAction("Index");
        }

    }
}