using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace 餐廳.Models
{
    public class InventoryItem
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "請選擇或輸入店面")]
        [Display(Name = "店面")]
        public string StoreName { get; set; } = string.Empty;

        [Required(ErrorMessage = "請選擇分類")]
        [Display(Name = "分類")]
        public string Category { get; set; } = string.Empty;

        [Required(ErrorMessage = "請輸入品項名稱")]
        [Display(Name = "品項")]
        public string ItemName { get; set; } = string.Empty;

        [Display(Name = "現有數量")]
        public int CurrentQuantity { get; set; }

        [Display(Name = "供貨標準")]
        public int MinRequiredQuantity { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "供貨價格")]
        public decimal UnitPrice { get; set; }

        [Display(Name = "單位")]
        public string Unit { get; set; } = "包";

        [Display(Name = "供應商")]
        public string Supplier { get; set; } = string.Empty;
    }
}