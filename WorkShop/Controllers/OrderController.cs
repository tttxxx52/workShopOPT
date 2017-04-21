using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkShop.Models;

namespace WorkShop.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult Index()
        {
            Models.OrderService orderService = new Models.OrderService();

            List<Models.Order> dataList = orderService.GetEmployeeData();
    

            List<SelectListItem> employeeList = new List<SelectListItem>();
            List<SelectListItem> shipperList = new List<SelectListItem>();
            //員工List
            foreach (var item in dataList)
            {
                employeeList.Add(new SelectListItem()
                {
                    Text = item.EmployeeName,
                    Value = item.EmployeeID.ToString(),
                });
            }
            ViewBag.empData = employeeList;
            
            //供應商List
            dataList = orderService.GetShipperData();
            foreach (var item in dataList)
            {
                shipperList.Add(new SelectListItem()
                {

                    Text = item.ShipperName,
                    Value = item.ShipperID.ToString()
                });
            }

            ViewBag.shipperData = shipperList;
            return View();
        }
        /// <summary>
        /// 回傳訂單資料
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetData(Models.Order order)
        {

            Models.OrderService orderService = new Models.OrderService();
            List<Models.Order> list = orderService.SearchOrder(order);
            return this.Json(list);
        }

        /// <summary>
        /// 刪除訂單
        /// </summary>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DoDelete(string OrderID)
        {
            Models.OrderService orderService = new Models.OrderService();
            orderService.DeleteOrderById(OrderID);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 新增訂單頁面
        /// </summary>
        /// <returns></returns>
        public ActionResult InsertPage()
        {
            Models.OrderService orderService = new Models.OrderService();

            List<Models.Order> dataList = orderService.GetEmployeeData();
            List<Models.OrderDetails> ProductList = orderService.GetProductData();

            List<SelectListItem> employeeList = new List<SelectListItem>();
            List<SelectListItem> shipperList = new List<SelectListItem>();
            List<SelectListItem> customerList = new List<SelectListItem>();
            List<SelectListItem> productList = new List<SelectListItem>();
            List<SelectListItem> unitpriceList = new List<SelectListItem>();

            //員工List
            foreach (var item in dataList)
            {
                employeeList.Add(new SelectListItem()
                {
                    Text = item.EmployeeName,
                    Value = item.EmployeeID.ToString(),
                });
            }
            ViewBag.empData = employeeList;

            //供應商List
            dataList = orderService.GetShipperData();
            foreach (var item in dataList)
            {
                shipperList.Add(new SelectListItem()
                {

                    Text = item.ShipperName,
                    Value = item.ShipperID.ToString()
                });
            }

            ViewBag.shipperData = shipperList;

            ///客戶List
            dataList = orderService.GetCustomerData();
            foreach (var item in dataList)
            {
                customerList.Add(new SelectListItem()
                {

                    Text = item.CustomerName,
                    Value = item.CustomerID.ToString()
                });
            }

            ViewBag.customerData = customerList;

            //產品List
            foreach (var item in ProductList)
            {
                productList.Add(new SelectListItem()
                {
                    Text = item.ProductName,
                    Value = item.ProductID.ToString()
                });
            }

            ViewBag.productData = productList;

            ProductList = orderService.GetPriceData();
            foreach (var item in ProductList)
            {
                unitpriceList.Add(new SelectListItem()
                {
                    Value = item.UnitPrice
                });
            }
            ViewBag.UnitPrice = unitpriceList;

            return View();
        }
        /// <summary>
        /// 新增訂單
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DoInsert(Models.Order order)
        {

            Models.OrderService orderService = new Models.OrderService();
            orderService.InsertOrder(order);
            return RedirectToAction("Index");
        }
    }
}