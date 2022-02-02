using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Task1_SelfPractice_1.DataDB
{
    /// <summary>
    /// Product subcategories. See ProductCategory table.
    /// </summary>
    /// 
    [JsonObject]
    public partial class ProductSubcategory
    {
        public ProductSubcategory()
        {
            Products = new HashSet<Product>();
        }

        /// <summary>
        /// Primary key for ProductSubcategory records.
        /// </summary>
        public int ProductSubcategoryId { get; set; }
        /// <summary>
        /// Product category identification number. Foreign key to ProductCategory.ProductCategoryID.
        /// </summary>
        /// 
        [JsonProperty]
        public int ProductCategoryId { get; set; }
        /// <summary>
        /// Subcategory description.
        /// </summary>
        /// 
        [JsonProperty]
        public string Name { get; set; } = null!;
        /// <summary>
        /// ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.
        /// </summary>
        public Guid Rowguid { get; set; }
        /// <summary>
        /// Date and time the record was last updated.
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        public virtual ProductCategory ProductCategory { get; set; } = null!;
        public virtual ICollection<Product> Products { get; set; }
    }
}
