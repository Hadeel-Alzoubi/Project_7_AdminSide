using E_Commerce_Clothes.Models;

namespace E_Commerce_Clothes.DTOs
{
    public class VoucherRequestDto
    {
        public string? Name { get; set; }        // رمز القسيمة

        public decimal? DiscountAmount { get; set; }  // قيمة الخصم
      
        public DateTime ExpiryDate { get; set; }    // تاريخ انتهاء الصلاحية


    }
}
