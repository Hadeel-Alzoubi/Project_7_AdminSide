using System.Text;

namespace E_Commerce_Clothes.DTOs
{
    public class PasswordHasher
    {
        public static void createPasswordHash(string password, out byte[] hash, out byte[] salt)
        {
            using (var h = new System.Security.Cryptography.HMACSHA512())
            {
                salt = h.Key;
                hash = h.ComputeHash(Encoding.UTF8.GetBytes(password));
            }

        }

        // انا ما اسخدمت الاوت هون لانو انا بس بدي اشيك على القيمة الي اصلا موجودة 
        public static bool VerifyPasswordHash(string password, byte[] PasswordHash, byte[] PasswordSalt) // ركزي انو هاي بوول مش فويد
        {
            //var hmac = new System.Security.Cryptography.HMACSHA256(storedSalt)
            using (var hmac = new System.Security.Cryptography.HMACSHA512(PasswordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(PasswordHash);
            }
        }
    }
}
