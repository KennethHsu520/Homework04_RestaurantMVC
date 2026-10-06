using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace 餐廳.Models
{
    public class SupplyRecord
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "日期")]
        public DateTime SupplyDate { get; set; } = DateTime.Now;

        [Display(Name = "店面")]
        public string StoreName { get; set; } = string.Empty;

        [Display(Name = "品項")]
        public string ItemName { get; set; } = string.Empty;

        [Display(Name = "供應商")]
        public string Supplier { get; set; } = string.Empty;

        [Display(Name = "供貨數量")]
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "總價")]
        public decimal TotalPrice { get; set; }
    }
}