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

                    //string sql = $"SELECT p.Name,p.ProductNumber,p.Color,p.StandardCost,p.ListPrice,p.SellStartDate,pc.Name,psc.Name " +
                    //              $"FROM Production.Product as p " +
                    //              $"CROSS JOIN " +
                    //              $"Production.ProductCategory as pc " +
                    //              $"LEFT JOIN " +
                    //              $"Production.ProductSubCategory as psc " +
                    //              $"on pc.ProductCategoryID = psc.ProductCategoryID " +
                    //              $"WHERE p.FinishedGoodsFlag=1;";
                    SqlCommand cmd = new SqlCommand(sql, conn); 
                    SqlDataReader sqlDataReader = cmd.ExecuteReader();
                    while (sqlDataReader.Read())
                    {
                        Product product = new Product();
                        product.Name =(string) sqlDataReader["Name"];
                        product.ProductId = (int)sqlDataReader["ProductId"];
                        product.ProductNumber = (string)sqlDataReader["ProductNumber"];
                        product.FinishedGoodsFlag = (bool)sqlDataReader["FinishedGoodsFlag"];


                        if (sqlDataReader["Color"] is System.DBNull)
                        {
                            product.Color = "";
                        }
                        else
                        {
                            product.Color = (string)sqlDataReader["Color"]; 
                        }
                        product.SafetyStockLevel = (short)sqlDataReader["SafetyStockLevel"];
                        ////! Index  out of range
                        product.StandardCost = (decimal)sqlDataReader["StandardCost"];
                        product.ListPrice = (decimal)sqlDataReader["ListPrice"];
                        //product.DaysToManufacture = (int)sqlDataReader["DaysToManufacture"];
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


        public class ProductInformation
        {
            public ProductInformation productInformation()
            {
                return new ProductInformation();
            }

            public Product product { get; set; }
            public ProductCategory category { get; set; }
            public ProductSubcategory subcategory { get; set; }
            public ProductInformation productInformation(Product product, ProductCategory productCategory, ProductSubcategory productSubcategory)
            {
                ProductInformation productInformation = new ProductInformation();
                productInformation.product = product;
                productInformation.category = productCategory;
                productInformation.subcategory = productSubcategory;
                return productInformation;
            }

        }
        [HttpGet("get-category")]   // /api/product/get-category
        public List<ProductInformation>? GetCategory()

        {

            List<ProductInformation> product = new List<ProductInformation>();
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("Task1_SelfPractice_1")))
            {
                conn.Open();
                try
                {
                   
                    //string sql = $"SELECT Product.Name,ProductID,ProductNumber,FinishedGoodsFlag,Color,SafetyStockLevel,SafetyStockLevel,ListPrice,DaysToManufacture,SellStartDate,StandardCost" +
                    //    $" FROM Production.Product" +
                    //    $" ORDER BY ReorderPoint DESC, Name ASC;";

                    string sql = $"SELECT p.Name,p.ProductNumber,p.Color,p.StandardCost,p.ListPrice,p.SellStartDate,pc.Name as categoryName,psc.Name as subCategoryName,psc.ProductSubcategoryId  " +
                                  $"FROM Production.Product as p " +
                                  $"LEFT JOIN " +
                                  $"Production.ProductSubCategory as psc " +
                                  $"on p.ProductSubcategoryID = psc.ProductSubcategoryID " +
                                  $"LEFT JOIN " +
                                  $"Production.ProductCategory as pc " +
                                  $"on pc.ProductCategoryID = psc.ProductCategoryID " +
                                  $"WHERE p.FinishedGoodsFlag=1;";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader sqlDataReader = cmd.ExecuteReader();
                    while (sqlDataReader.Read())

                    {
                   
                        ProductInformation productElement = new ProductInformation();

                        //! This to create a Product inside ProductInformation
                        Product productdata = new Product();
                  
                        productdata.Name = (string)sqlDataReader["Name"];
                        productdata.ProductNumber = (string)sqlDataReader["ProductNumber"];
                        if (sqlDataReader["Color"] is System.DBNull)
                        {
                            productdata.Color = "";
                        }
                        else
                        {
                            productdata.Color = (string)sqlDataReader["Color"];

                        }

                        productdata.StandardCost = (decimal)sqlDataReader["StandardCost"];
                        productdata.ListPrice = (decimal)sqlDataReader["ListPrice"];
                        DateTime SellStartDate = (System.DateTime)sqlDataReader["SellStartDate"];
                        var SellDate = SellStartDate.Date;
                        productdata.SellStartDate = SellDate;

                        ProductCategory category = new ProductCategory();
                        
                        if (sqlDataReader["categoryName"] is System.DBNull)
                        {
                            category.Name = "";
                        }
                        else
                        {
                            category.Name = (string)sqlDataReader["categoryName"];

                        }


                        ProductSubcategory subcategory = new ProductSubcategory();
                        subcategory.Name = (string)sqlDataReader["subCategoryName"];
                        if (sqlDataReader["subCategoryName"] is System.DBNull)
                        {
                            subcategory.Name = "";
                        }
                        else
                        {
                            subcategory.Name = (string)sqlDataReader["subCategoryName"];

                        }

                        subcategory.ProductSubcategoryId = (int)sqlDataReader["ProductSubcategoryId"];


                      
                        productElement.product=productdata;
                        productElement.subcategory=subcategory;
                        productElement.category = category;
                        product.Add(productElement);
                      








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
            return product;

        }

        [HttpGet("get-task3part2")]   // /api/product/get-category
        public List<ProductInformation>? GetAllTask3()

        {

            List<ProductInformation> product = new List<ProductInformation>();
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("Task1_SelfPractice_1")))
            {
                conn.Open();
                try
                {

                    //string sql = $"SELECT Product.Name,ProductID,ProductNumber,FinishedGoodsFlag,Color,SafetyStockLevel,SafetyStockLevel,ListPrice,DaysToManufacture,SellStartDate,StandardCost" +
                    //    $" FROM Production.Product" +
                    //    $" ORDER BY ReorderPoint DESC, Name ASC;";

                    string sql = $"SELECT p.FinishedGoodsFlag,p.Name,p.ProductNumber,p.Color,p.StandardCost,p.ListPrice,p.SellStartDate,pc.Name as categoryName,psc.Name as subCategoryName,psc.ProductSubcategoryId  " +
                                  $"FROM Production.Product as p " +
                                  $"LEFT JOIN " +
                                  $"Production.ProductSubCategory as psc " +
                                  $"on p.ProductSubcategoryID = psc.ProductSubcategoryID " +
                                  $"LEFT JOIN " +
                                  $"Production.ProductCategory as pc " +
                                  $"on pc.ProductCategoryID = psc.ProductCategoryID;";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader sqlDataReader = cmd.ExecuteReader();
                    while (sqlDataReader.Read())

                    {

                        ProductInformation productElement = new ProductInformation();

                        //! This to create a Product inside ProductInformation
                        Product productdata = new Product();

                        productdata.Name = (string)sqlDataReader["Name"];
                        productdata.ProductNumber = (string)sqlDataReader["ProductNumber"];
                        if (sqlDataReader["Color"] is System.DBNull)
                        {
                            productdata.Color = "";
                        }
                        else
                        {
                            productdata.Color = (string)sqlDataReader["Color"];

                        }

                        productdata.StandardCost = (decimal)sqlDataReader["StandardCost"];
                        productdata.ListPrice = (decimal)sqlDataReader["ListPrice"];
                        DateTime SellStartDate = (System.DateTime)sqlDataReader["SellStartDate"];
                        var SellDate = SellStartDate.Date;
                        productdata.SellStartDate = SellDate;
                        productdata.FinishedGoodsFlag = (bool)sqlDataReader["FinishedGoodsFlag"];

                        ProductCategory category = new ProductCategory();
                        if (sqlDataReader["categoryName"] is System.DBNull)
                        {
                            category.Name = "";
                        }
                        else
                        {
                            category.Name = (string)sqlDataReader["categoryName"];

                        }



                        ProductSubcategory subcategory = new ProductSubcategory();
                        if (sqlDataReader["subCategoryName"] is System.DBNull)
                        {
                            subcategory.Name = "";
                        }
                        else
                        {
                            subcategory.Name = (string)sqlDataReader["subCategoryName"];

                        }

                  
                        if (sqlDataReader["ProductSubcategoryId"] is System.DBNull)
                        {
                            subcategory.ProductSubcategoryId = -1;
                        }
                        else
                        {
                            subcategory.ProductSubcategoryId = (int)sqlDataReader["ProductSubcategoryId"];

                        }



                        productElement.product = productdata;
                        productElement.subcategory = subcategory;
                        productElement.category = category;
                        product.Add(productElement);









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
            return product;

        }
    }

}
