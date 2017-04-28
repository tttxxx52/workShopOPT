using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WorkShop.Models
{
    public class OrderService
    {
        /// <summary>
        /// 連結資料庫
        /// </summary>
        /// <returns></returns>
        private string GetDBConnectionString()
        {
            return
                System.Configuration.ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
        }


        /// <summary>
        /// 取得產品資料
        /// </summary>
        /// <returns></returns>
        public List<Models.OrderDetails> GetProductData()
        {
            DataTable dataTable = new DataTable();
            string sql = @"Select ProductID, ProductName
                            From Production.Products";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dataTable);
                conn.Close();
            }
            return this.MapProductDataToList(dataTable);
        }
        /// <summary>
        /// 取得產品資料變成List
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        private List<OrderDetails> MapProductDataToList(DataTable dataTable)
        {
            List<Models.OrderDetails> result = new List<OrderDetails>();
            foreach (DataRow row in dataTable.Rows)
            {
                result.Add(new OrderDetails
                {
                    ProductID = (int)row["ProductID"],
                    ProductName = row["ProductName"].ToString(),
                });
            }
            return result;
        }
        /// <summary>
        /// 獲取單價
        /// </summary>
        /// <returns></returns>
        public List<Models.OrderDetails> GetPriceData()
        {
            DataTable dataTable = new DataTable();
            string sql = @"Select UnitPrice
                           From Production.Products";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dataTable);
                conn.Close();
            }
            return this.MapUntiPriceToList(dataTable);
        }
        /// <summary>
        /// 取得產品價格變成List
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        private List<OrderDetails> MapUntiPriceToList(DataTable dataTable)
        {
            List<Models.OrderDetails> result = new List<OrderDetails>();
            foreach (DataRow row in dataTable.Rows)
            {
                result.Add(new OrderDetails
                {
                    UnitPrice = row["UnitPrice"].ToString(),
                });
            }
            return result;
        }
        /// <summary>
        /// 取得客戶資料
        /// </summary>
        /// <returns></returns>
        public List<Models.Order> GetCustomerData()
        {
            DataTable dataTable = new DataTable();
            string sql = @"Select CustomerID, CompanyName
                            From Sales.Customers";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dataTable);
                conn.Close();
            }
            return this.MapCustomerDataToList(dataTable);
        }
        /// <summary>
        /// 取得客戶資料變成List
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        private List<Order> MapCustomerDataToList(DataTable dataTable)
        {
            List<Models.Order> result = new List<Order>();
            foreach (DataRow row in dataTable.Rows)
            {
                result.Add(new Order
                {
                    CustomerID = (int)row["CustomerID"],
                    CustomerName = row["CompanyName"].ToString()
                });
            }
            return result;
        }
        /// <summary>
        /// 取得員工資料
        /// </summary>
        public List<Models.Order> GetEmployeeData()
        {
            DataTable dataTable = new DataTable();
            string sql = @"Select EmployeeID, FirstName + ' ' + LastName As EmployeeName
                         From HR.Employees";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dataTable);
                conn.Close();
            }
            return this.MapEmployeeDataToList(dataTable);
        }

        /// <summary>
        /// 取得員工資料變成List
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        private List<Order> MapEmployeeDataToList(DataTable dataTable)
        {
            List<Models.Order> result = new List<Order>();
            foreach (DataRow row in dataTable.Rows)
            {
                result.Add(new Order
                {
                    EmployeeID = (int)row["EmployeeID"],
                    EmployeeName = row["EmployeeName"].ToString()
                });
            }
            return result;
        }

        /// <summary>
        /// 取得供應商資料
        /// </summary>
        /// <returns></returns>
        public List<Models.Order> GetShipperData()
        {
            DataTable dataTable = new DataTable();
            string sql = @"Select ShipperID, CompanyName As ShipperName
                           From Sales.Shippers";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dataTable);
                conn.Close();
            }
            return this.MapShipperDataToList(dataTable);
        }

        /// <summary>
        /// 取得供應商資料變成List
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        private List<Order> MapShipperDataToList(DataTable dataTable)
        {
            List<Models.Order> result = new List<Order>();
            foreach (DataRow row in dataTable.Rows)
            {
                result.Add(new Order
                {
                    ShipperID = (int)row["ShipperID"],
                    ShipperName = row["ShipperName"].ToString()
                });
            }
            return result;
        }

        /// <summary>
        /// 取得搜尋條件資料
        /// </summary>
        /// <returns></returns>
        public List<Models.Order> SearchOrder(Models.Order order)
        {
            DataTable dataTable = new DataTable();
            string sql = @"Select OrderID, CompanyName AS CustomerName,  CONVERT(VARCHAR,OrderDate,23) as OrderDate, CONVERT(VARCHAR,ShippedDate,23) as ShippedDate
                            From Sales.Orders O join Sales.Customers C on O.CustomerID = C.CustomerID 
                            Where (OrderID = @OrderID OR @OrderID = 0)
                              AND (CompanyName = @CompanyName OR @CompanyName IS NULL)
                              AND (EmployeeID = @EmployeeID OR @EmployeeID = 0)
                              AND (ShipperID = @ShipperID OR @ShipperID = 0)
                              AND (OrderDate = @OrderDate OR @OrderDate IS NULL)
                              AND (ShippedDate = @ShippedDate OR @ShippedDate IS NULL)
                              AND (RequiredDate = @RequiredDate OR @RequiredDate IS NULL)";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@OrderID", order.OrderID));
                cmd.Parameters.Add(new SqlParameter("@CompanyName", order.CustomerName == null ? (object)DBNull.Value : order.CustomerName));
                cmd.Parameters.Add(new SqlParameter("@EmployeeID", order.EmployeeID));
                cmd.Parameters.Add(new SqlParameter("@ShipperID", order.ShipperID));
                cmd.Parameters.Add(new SqlParameter("@OrderDate", order.OrderDate == null ? (object)DBNull.Value : order.OrderDate));
                cmd.Parameters.Add(new SqlParameter("@ShippedDate", order.ShippedDate == null ? (object)DBNull.Value : order.ShippedDate));
                cmd.Parameters.Add(new SqlParameter("@RequiredDate", order.RequiredDate == null ? (object)DBNull.Value : order.RequiredDate));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dataTable);
                conn.Close();
            }
            return this.MapOrderDataToList(dataTable);
        }

        /// <summary>
        /// 取得訂單資料變成List
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        private List<Order> MapOrderDataToList(DataTable dataTable)
        {
            List<Models.Order> result = new List<Order>();
            foreach (DataRow row in dataTable.Rows)
            {
                result.Add(new Order
                {
                    OrderID = (int)row["OrderID"],
                    CustomerName = row["CustomerName"].ToString(),
                    OrderDate = row["OrderDate"].ToString(),
                    ShippedDate = row["ShippedDate"] == DBNull.Value ? null : row["ShippedDate"].ToString()
                });
            }
            return result;
        }

        /// <summary>
        /// 新增訂單
        /// </summary>
        /// <param name="order"></param>
        public string InsertOrder(Models.Order order)
        {
            string sql = @"Insert INTO Sales.Orders
                         (
                            CustomerID,EmployeeID,OrderDate,RequiredDate,ShippedDate,ShipperID,Freight,
                            ShipCountry,ShipCity,ShipRegion,ShipPostalCode,ShipAddress,ShipName
                         )
                         VALUES
                         (
                            @CustomerID,@EmployeeID,@OrderDate,@RequiredDate,@ShippedDate,@ShipperID,@Freight,
                            @ShipCountry,@ShipCity,@ShipRegion,@ShipPostalCode,@ShipAddress,@ShipName
                         )
                         Select SCOPE_IDENTITY()
                         ";

            string sql2 = @"Insert INTO Sales.OrderDetails
                         (
                            OrderID,ProductID,UnitPrice,Qty,Discount
                         )
                        
                         VALUES
                         (
                            @OrderID,@ProductID,@UnitPrice,@Qty,@Discount
                         )
                         ";

            string OrderID;
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlCommand cmd2 = new SqlCommand(sql2, conn);
                cmd.Parameters.Add(new SqlParameter("@CustomerID", order.CustomerID));
                cmd.Parameters.Add(new SqlParameter("@EmployeeID", order.EmployeeID));
                cmd.Parameters.Add(new SqlParameter("@OrderDate", order.OrderDate == null ? (object)DBNull.Value : order.OrderDate));
                cmd.Parameters.Add(new SqlParameter("@RequiredDate", order.RequiredDate == null ? (object)DBNull.Value : order.RequiredDate));
                cmd.Parameters.Add(new SqlParameter("@ShippedDate", order.ShippedDate == null ? (object)DBNull.Value : order.ShippedDate));
                cmd.Parameters.Add(new SqlParameter("@ShipperID", order.ShipperID));
                cmd.Parameters.Add(new SqlParameter("@Freight", order.Freight ?? string.Empty));
                cmd.Parameters.Add(new SqlParameter("@ShipCountry", order.ShipCountry ?? string.Empty));
                cmd.Parameters.Add(new SqlParameter("@ShipCity", order.ShipCity ?? string.Empty));
                cmd.Parameters.Add(new SqlParameter("@ShipRegion", order.ShipRegion == null ? (object)DBNull.Value : order.ShipRegion));
                cmd.Parameters.Add(new SqlParameter("@ShipPostalCode", order.ShipPostalCode == null ? (object)DBNull.Value : order.ShipPostalCode));
                cmd.Parameters.Add(new SqlParameter("@ShipAddress", order.ShipAddress ?? string.Empty));
                cmd.Parameters.Add(new SqlParameter("@ShipName", order.ShipName ?? string.Empty));

                OrderID = cmd.ExecuteScalar().ToString();

                for (int i = 0; i < order.OrderDetails.Count; i++)
                {
                    cmd2 = new SqlCommand(sql2, conn);
                    cmd2.Parameters.Add(new SqlParameter("@OrderID", OrderID));
                    cmd2.Parameters.Add(new SqlParameter("@ProductID", order.OrderDetails[i].ProductID));
                    cmd2.Parameters.Add(new SqlParameter("@UnitPrice", order.OrderDetails[i].UnitPrice));
                    cmd2.Parameters.Add(new SqlParameter("@Qty", order.OrderDetails[i].Qty));
                    cmd2.Parameters.Add(new SqlParameter("@Discount", order.OrderDetails[i].Discount));
                    cmd2.ExecuteNonQuery();
                }

                conn.Close();
            }
            return OrderID;
        }


        /// <summary>
        /// 更新訂單
        /// </summary>
        public void UpdateOrderById(Models.Order order)
        {
            string sql = @"
                    UPDATE Sales.Orders
                    SET
                        CustomerID = @CustomerID,
                        EmployeeID = @EmployeeID,
                        OrderDate = @OrderDate,
                        RequiredDate = @RequiredDate,
                        ShippedDate = @ShippedDate,
                        ShipperID = @ShipperID,
                        Freight = @Freight,
                        ShipCountry = @ShipCountry,
                        ShipCity = @ShipCity,
                        ShipRegion = @ShipRegion,
                        ShipPostalCode = @ShipPostalCode,
                        ShipAddress = @ShipAddress,
                        ShipName = @ShipName
                    WHERE OrderID = @OrderID";

            string sql2 = @"DELETE
                           FROM Sales.OrderDetails
                           Where OrderID = @OrderID";

            string sql3 = @"Insert INTO Sales.OrderDetails
                         (
                            OrderID,ProductID,UnitPrice,Qty,Discount
                         )
                        
                         VALUES
                         (
                            @OrderID,@ProductID,@UnitPrice,@Qty,@Discount
                         )
                         ";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlCommand cmd2 = new SqlCommand(sql2, conn);
                SqlCommand cmd3;

                cmd.Parameters.Add(new SqlParameter("@OrderID", order.OrderID));
                cmd.Parameters.Add(new SqlParameter("@CustomerID", order.CustomerID));
                cmd.Parameters.Add(new SqlParameter("@EmployeeID", order.EmployeeID));
                cmd.Parameters.Add(new SqlParameter("@OrderDate", order.OrderDate == null ? (object)DBNull.Value : order.OrderDate));
                cmd.Parameters.Add(new SqlParameter("@RequiredDate", order.RequiredDate == null ? (object)DBNull.Value : order.RequiredDate));
                cmd.Parameters.Add(new SqlParameter("@ShippedDate", order.ShippedDate == null ? (object)DBNull.Value : order.ShippedDate));
                cmd.Parameters.Add(new SqlParameter("@ShipperID", order.ShipperID));
                cmd.Parameters.Add(new SqlParameter("@Freight", order.Freight ?? string.Empty));
                cmd.Parameters.Add(new SqlParameter("@ShipCountry", order.ShipCountry ?? string.Empty));
                cmd.Parameters.Add(new SqlParameter("@ShipCity", order.ShipCity ?? string.Empty));
                cmd.Parameters.Add(new SqlParameter("@ShipRegion", order.ShipRegion == null ? (object)DBNull.Value : order.ShipRegion));
                cmd.Parameters.Add(new SqlParameter("@ShipPostalCode", order.ShipPostalCode == null ? (object)DBNull.Value : order.ShipPostalCode));
                cmd.Parameters.Add(new SqlParameter("@ShipAddress", order.ShipAddress ?? string.Empty));
                cmd.Parameters.Add(new SqlParameter("@ShipName", order.ShipName ?? string.Empty));
                cmd.ExecuteNonQuery();
                cmd2.Parameters.Add(new SqlParameter("OrderID", order.OrderID));
                cmd2.ExecuteNonQuery();
                for (int i = 0; i < order.OrderDetails.Count; i++)
                {
                    cmd3 = new SqlCommand(sql3, conn);
                    cmd3.Parameters.Add(new SqlParameter("@OrderID", order.OrderID));
                    cmd3.Parameters.Add(new SqlParameter("@ProductID", order.OrderDetails[i].ProductID));
                    cmd3.Parameters.Add(new SqlParameter("@UnitPrice", order.OrderDetails[i].UnitPrice));
                    cmd3.Parameters.Add(new SqlParameter("@Qty", order.OrderDetails[i].Qty));
                    cmd3.Parameters.Add(new SqlParameter("@Discount", order.OrderDetails[i].Discount));
                    cmd3.ExecuteNonQuery();
                }
                conn.Close();
            }
        }

        /// <summary>
        /// 依照Id 取得訂單明細資料
        /// </summary>
        /// <returns></returns>
        public List<Models.OrderDetails> GetOrderDetialById(string OrderID)
        {
            DataTable dataTable = new DataTable();
            string sql2 = @"SELECT
                    O.OrderID,D.ProductID,D.Qty,D.UnitPrice
                    From Sales.Orders As O
                    JOIN Sales.OrderDetails As D ON O.OrderID = D.OrderID
                    Where O.OrderId = @OrderID";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql2, conn);
                cmd.Parameters.Add(new SqlParameter("@OrderID", OrderID));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dataTable);
                conn.Close();
            }
            return MapUpdateOrderDetialData(dataTable);

        }

        private List<OrderDetails> MapUpdateOrderDetialData(DataTable dataTable)
        {
            List<Models.OrderDetails> result = new List<OrderDetails>();
            foreach (DataRow row in dataTable.Rows)
            {
                result.Add(new OrderDetails
                {
                    OrderID = (int)row["OrderID"],
                    ProductID = (int)row["ProductID"],
                    Qty = Convert.ToInt16(row["Qty"]),
                    UnitPrice = row["UnitPrice"].ToString()
                });
            }
            return result;
        }



        /// <summary>
        /// 依照Id 取得訂單資料
        /// </summary>
        /// <returns></returns>
        public Models.Order GetOrderById(string OrderID)
        {
            DataTable dataTable = new DataTable();
            string sql = @"SELECT
                    O.OrderID,O.CustomerID,C.Companyname As CustomerName,
					E.EmployeeID,E.lastname + E.firstname As EmployeeName,
                    O.Orderdate,O.RequiredDate,O.ShippedDate,
					O.ShipperID,S.companyname As ShipperName,O.Freight,
					O.ShipName,O.ShipAddress,O.ShipCity,O.ShipRegion,O.ShipPostalCode,O.ShipCountry
                    From Sales.Orders As O
                    JOIN Sales.Customers As C ON O.CustomerID = C.CustomerID
                    JOIN HR.Employees As E On O.EmployeeID = E.EmployeeID
                    JOIN Sales.Shippers As S ON O.ShipperID = S.ShipperID
                    Where O.OrderId = @OrderID";


            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@OrderID", OrderID));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dataTable);
                conn.Close();
            }
            return MapUpdateOrderData(dataTable);
        }

        private Order MapUpdateOrderData(DataTable dataTable)
        {
            Models.Order result = new Order();
            foreach (DataRow row in dataTable.Rows)
            {
                result.OrderID = (int)row["OrderID"];
                result.CustomerID = (int)row["CustomerID"];
                result.CustomerName = row["CustomerName"].ToString();
                result.EmployeeID = (int)row["EmployeeID"];
                result.EmployeeName = row["EmployeeName"].ToString();
                result.OrderDate = row["OrderDate"].ToString();
                result.RequiredDate = (DateTime)row["RequiredDate"];
                result.ShippedDate = row["ShippedDate"] == DBNull.Value ? null : row["ShippedDate"].ToString();
                result.ShipperID = (int)row["ShipperID"];
                result.ShipperName = row["ShipperName"].ToString();
                result.Freight = row["Freight"].ToString();
                result.ShipName = row["ShipName"].ToString();
                result.ShipAddress = row["ShipAddress"].ToString();
                result.ShipCity = row["ShipCity"].ToString();
                result.ShipRegion = row["ShipRegion"].ToString();
                result.ShipPostalCode = row["ShipPostalCode"].ToString();
                result.ShipCountry = row["ShipCountry"].ToString();
            }
            return result;
        }
        /// <summary>
        /// 刪除訂單
        /// </summary>
        public void DeleteOrderById(string OrderID)
        {
            try
            {
                string sql = @"DELETE
                               FROM Sales.OrderDetails
                               Where OrderID = @OrderID";
                string sql2 = @"DELETE
                               FROM Sales.Orders
                               Where OrderID = @OrderID";
                using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlCommand cmd2 = new SqlCommand(sql2, conn);
                    cmd.Parameters.Add(new SqlParameter("OrderID", OrderID));
                    cmd.ExecuteNonQuery();
                    cmd2.Parameters.Add(new SqlParameter("OrderID", OrderID));
                    cmd2.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
