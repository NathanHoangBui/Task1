using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using Task1_SelfPractice_1.DataDB;

namespace Task1_SelfPractice_1.Controllers
{   
    [Route("api/[controller]")]
    [ApiController]
    
    public class ProductController : Controller
    {
        private readonly IConfiguration _configuration;

        public ProductController(IConfiguration configuration)
        {
            _configuration = configuration;

        }
        [HttpGet("get-all")]   // /api/product/get-all
        public List<Product>? GetAll()

        {
            List<Product> products = new List<Product>();       
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("Task1_SelfPractice_1")))
            {
                conn.Open();
                try
                {
                    string sql = $"SELECT Product.Name,ProductID,ProductNumber,FinishedGoodsFlag,Color,SafetyStockLevel,SafetyStockLevel,ListPrice,DaysToManufacture,SellStartDate,StandardCost" +
                        $" FROM Production.Product" +
                        $" ORDER BY ReorderPoint DESC, Name ASC;";
                    SqlCommand cmd = new SqlCommand(sql, conn); 
                    SqlDataReader sqlDataReader = cmd.ExecuteReader();
                    while (sqlDataReader.Read())
                    {
                        Product product = new Product();
                        product.Name =(string) sqlDataReader["Name"];
                        product.ProductId = (int)sqlDataReader["ProductId"];
                        product.ProductNumber = (string)sqlDataReader["ProductNumber"];
                        product.FinishedGoodsFlag = (bool)sqlDataReader["FinishedGoodsFlag"];
                    

                        if(sqlDataReader["Color"] is System.DBNull)
                        {
                            product.Color = "";
                        }
                        else
                        {
                            product.Color = (string)sqlDataReader["Color"]; 
                        }
                        product.SafetyStockLevel = (short)sqlDataReader["SafetyStockLevel"];
                        //! Index  out of range
                        product.StandardCost = (decimal)sqlDataReader["StandardCost"];
                        product.ListPrice = (decimal)sqlDataReader["ListPrice"];
                        product.DaysToManufacture = (int)sqlDataReader["DaysToManufacture"];
                        DateTime SellStartDate = (System.DateTime)sqlDataReader["SellStartDate"];
                        var SellDate = SellStartDate.Date;
                        product.SellStartDate = SellDate;
                        products.Add(product);  

               
                    }
                  
                 
                
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    conn.Close();
                }

            }
            return products;

        }
    }
}
