using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WorkShop.Models
{
    public class OrderDetails
    {
        /// <summary>
        /// 訂單編號
        /// </summary>
        [DisplayName("訂單編號")]
        public int OrderID { get; set; }

        /// <summary>
        /// 產品代號
        /// </summary>
        [DisplayName("產品代號")]
        public int ProductID { get; set; }

        /// <summary>
        /// 產品名稱
        /// </summary>
        [DisplayName("產品名稱")]
        public string ProductName { get; set; }

        /// <summary>
        /// 單價
        /// </summary>
        [DisplayName("單價")]
        public string UnitPrice { get; set; }

        /// <summary>
        /// 數量
        /// </summary>
        [DisplayName("數量")]
        public decimal Qty { get; set; }

        /// <summary>
        /// 折扣
        /// </summary>
        [DisplayName("折扣")]
        public int Discount { get; set; }
    }
}