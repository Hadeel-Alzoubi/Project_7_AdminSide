using E_Commerce_Clothes.DTOs;
using E_Commerce_Clothes.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Clothes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly MyDbContext _db;
        private readonly IEmailService _emailService;
        public VoucherController(MyDbContext db, IEmailService emailService)
        {
            _db = db;
            _emailService = emailService;
        }


        //  ارسال البريد الالكتروني بشكل يدوي من قبل الادمن بالقسيمة يلي بدي ابعتها 
        [HttpPost("send-voucher/{userEmail}")]
        public async Task<IActionResult> SendVoucherToUser(string userEmail, string voucherCode)
        {
            // تحقق من وجود القسيمة
            var existingVoucher = _db.Copons.FirstOrDefault(v => v.Name == voucherCode);
            if (existingVoucher == null)
            {
                return NotFound("Voucher not found.");
            }

            // إعداد رسالة البريد الإلكتروني
            var subject = "Your Voucher Code";
            var body = $"Here is your voucher code: {existingVoucher.Name} with a discount of {existingVoucher.DiscountAmount}. " +
                       $"The voucher expires on {existingVoucher.ExpiryDate}.";

            // إرسال البريد الإلكتروني
            _emailService.SendEmail(userEmail, subject, body);

            return Ok("Voucher sent to user.");
        }






        [HttpPost]
        public IActionResult CreateVoucher([FromBody] VoucherRequestDto voucher, string userEmail)
        {
            var existingVoucher = _db.Copons.FirstOrDefault(v => v.Name == voucher.Name);
            if (existingVoucher != null)
            {
                return BadRequest("Voucher code already exists.");
            }

            if (string.IsNullOrEmpty(voucher.Name) || voucher.DiscountAmount <= 0 || voucher.ExpiryDate <= DateTime.Now)
            {
                return BadRequest("Invalid voucher data.");
            }

            var vouchers = new Copon
            {
                Name = voucher.Name,
                DiscountAmount = voucher.DiscountAmount,
                ExpiryDate = voucher.ExpiryDate,
                IsUsed = false,
                CreatedAt = DateTime.Now
            };

            _db.Add(vouchers);
            _db.SaveChanges();
            SendVoucherToUser(userEmail, voucher.Name);
            return Ok("Voucher created successfully.");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateVoucher(int id, [FromBody] VoucherRequestDto request)
        {
            var voucher = _db.Copons.Find(id);
            if (voucher == null)
            {
                return NotFound("Voucher not found.");
            }

            voucher.Name = request.Name;
            voucher.DiscountAmount = request.DiscountAmount;
            voucher.ExpiryDate = request.ExpiryDate;

            _db.Copons.Update(voucher);
            _db.SaveChanges();

            return Ok("Voucher updated successfully.");
        }



        [HttpDelete("{id}")]
        public IActionResult DeleteVoucher(int id)
        {
            var voucher = _db.Copons.Find(id);
            if (voucher == null)
            {
                return NotFound("Voucher not found.");
            }

            _db.Copons.Remove(voucher);
            _db.SaveChanges();

            return Ok("Voucher deleted successfully.");
        }


        //////////////////////////////////////////
        ///ارسال البريد الالكتروني لجميع اليوزر بمناسبة معينة
        [HttpPost("SendEmailALLUser")]
        public async Task<IActionResult> SendBulkEmail(string subject, string body)
        {
            var users = _db.Users.ToList(); // الحصول على قائمة جميع المستخدمين

            // قائمة للمهام غير المتزامنة
            var tasks = new List<Task>();

            foreach (var user in users)
            {
                // صياغة الرسالة المناسبة
                var personalizedBody = $@"
                        مرحبًا {user.Email}،

                        بمناسبة الانتخابات الوطنية، نود أن نقدم لك قسيمة خصم خاصة كعربون تقدير لمساندتك لنا.

                        استخدم رمز القسيمة التالي للحصول على خصم قيمته 10% على أي عملية شراء:

                        **رمز القسيمة: ELECTION10**

                        القسيمة صالحة لمدة أسبوع بدءًا من تاريخ إرسال هذه الرسالة.

                        شكرًا لك على دعمك، ونتمنى لك يوماً سعيداً!

                        مع تحياتنا،
                        فريق [اسم شركتك]
                        ";

                // إضافة مهمة غير متزامنة لإرسال البريد لكل مستخدم
                tasks.Add(Task.Run(() => _emailService.SendEmail(user.Email, subject, personalizedBody)));
            }

            // انتظار كل المهام حتى تكتمل
            await Task.WhenAll(tasks);

            return Ok("Emails sent to all users.");
        }

        ////////////////////////////////////////////
        ///هاد الكود بقوم بانشاء القسيمة تلقائيا وارسالها لجميع اليوزر بمناسبة معينة لازم اكوم نعرفة ميثود بتنشألأي القسيمة تلقائيا

        //public string GenerateVoucherCode()
        //{
        //    var guid = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10); // توليد رمز عشوائي مكون من 10 أحرف
        //    return $"VOUCHER-{guid}";
        //}

        //public async Task<IActionResult> SendALLUSEREmail(string subject, string body)
        //{
        //    var users = _db.Users.ToList(); // الحصول على قائمة جميع المستخدمين

        //    // قائمة للمهام غير المتزامنة
        //    var tasks = new List<Task>();

        //    foreach (var user in users)
        //    {
        //        // توليد رمز قسيمة فريد
        //        var voucherCode = GenerateVoucherCode();

        //        // تخزين القسيمة في قاعدة البيانات
        //        var voucher = new Copon
        //        {
        //            Name = voucherCode,
        //            DiscountAmount = 10, // قيمة الخصم
        //            ExpiryDate = DateTime.Now.AddDays(7), // فترة صلاحية القسيمة
        //            CreatedAt = DateTime.Now
        //        };

        //        _db.Copons.Add(voucher);
        //        await _db.SaveChangesAsync();

        //        // صياغة الرسالة المناسبة
        //        var personalizedBody = $@"
        //مرحبًا {user.Email}،

        //بمناسبة الانتخابات الوطنية، نود أن نقدم لك قسيمة خصم خاصة كعربون تقدير لمساندتك لنا.

        //استخدم رمز القسيمة التالي للحصول على خصم قيمته 10% على أي عملية شراء:

        //**رمز القسيمة: {voucherCode}**

        //القسيمة صالحة لمدة أسبوع بدءًا من تاريخ إرسال هذه الرسالة.

        //شكرًا لك على دعمك، ونتمنى لك يوماً سعيداً!

        //مع تحياتنا،
        //فريق [اسم شركتك]
        //";

        //        // إضافة مهمة غير متزامنة لإرسال البريد لكل مستخدم
        //        // لما اعبي هون فقط رح اعبي السبجكت هدولاك انا حددتهم
        //        tasks.Add(Task.Run(() => _emailService.SendEmail(user.Email, subject, personalizedBody)));
        //    }

        //    // انتظار كل المهام حتى تكتمل
        //    await Task.WhenAll(tasks);

        //    return Ok("Emails sent to all users.");
        //}

        [HttpGet("ApplyVoucher")]
        public IActionResult ApplyVoucher(string code)
        {
            // Dummy discount logic
            decimal discount = 0;
            if (code == "DISCOUNT10")
            {
                discount = 10; // 10% discount
            }
            else if (code == "DISCOUNT20")
            {
                discount = 20; // 20% discount
            }

            return Ok(new { Discount = discount });
        }
    }
}
