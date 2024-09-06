using E_Commerce_Clothes.DTOs;
using E_Commerce_Clothes.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Clothes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminControllers : ControllerBase
    {
        private MyDbContext _db;
        public AdminControllers(MyDbContext db)
        {
            _db = db;
        }

        //////////// Get Product  //////////////////////////////

        [HttpGet("Product")]
        public IActionResult GetAdminProduct()
        {
            var product = _db.Products.ToList();
            return Ok(product);
        }

        //////////// Get Category  //////////////////////////////

        [HttpGet("Category")]
        public IActionResult GetAdminCategory()
        {
            var category = _db.Categories.ToList();
            return Ok(category);
        }

        //////////// Get User  //////////////////////////////

        [HttpGet("User")]
        public IActionResult GetUsers()
        {
            var user = _db.Users.ToList();
            return Ok(user);
        }


        [HttpGet("Admin")]
        public IActionResult GetAdmin()
        {
            var admin = _db.Admins.ToList();
            return Ok(admin);
        }
        //------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------


        //////////// Add Product  //////////////////////////////
        [HttpPost("Product")]
        public IActionResult PostAdminProduct([FromForm] ProductsRequestDTO products)
        {
            var uploadImageFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            if (!Directory.Exists(uploadImageFolder))
            {
                Directory.CreateDirectory(uploadImageFolder);
            }
            var imageFile = Path.Combine(uploadImageFolder, products.Image.FileName);
            using (var stream = new FileStream(imageFile, FileMode.Create))
            {
                products.Image.CopyToAsync(stream);
            }

            var data = new Product
            {
                Price = products.Price,
                Name = products.Name,
                Image = products.Image.FileName,
                CategoryId = products.CategoryId,
                Description = products.Description,
                Brand = products.Brand,
                PriceWithDiscount = products.PriceWithDiscount,
            };

            _db.Products.Add(data);
            _db.SaveChanges();
            return Ok(data);
        }

        // Add Category ///////////////////////  
        [HttpPost("Category")]
        public IActionResult PostAdminCategory([FromForm] CategoryRequestDTO category)
        {
            var uploadImageFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            if (!Directory.Exists(uploadImageFolder))
            {
                Directory.CreateDirectory(uploadImageFolder);
            }
            var imageFile = Path.Combine(uploadImageFolder, category.Image.FileName);
            using (var stream = new FileStream(imageFile, FileMode.Create))
            {
                category.Image.CopyToAsync(stream);
            }

            var data = new Category
            {
               Image = category.Image.FileName,
               Name = category.Name,
               Description = category.Description,  
            };

            _db.Categories.Add(data);
            _db.SaveChanges();
            return Ok(data);
        }


        /////// Add User ///////////////////////  
        [HttpPost("USer")]
        public IActionResult PostUser([FromForm] UserRequestDTO user)
        {
            byte[] hash;
            byte[] salt;

            PasswordHasher.createPasswordHash(user.Password, out hash, out salt);

            var uploadImageFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            if (!Directory.Exists(uploadImageFolder))
            {
                Directory.CreateDirectory(uploadImageFolder);
            }
            var imageFile = Path.Combine(uploadImageFolder, user.Image.FileName);
            using (var stream = new FileStream(imageFile, FileMode.Create))
            {
                user.Image.CopyToAsync(stream);
            }

            var data = new User
            {
               Name = user.Name,
               Email = user.Email,
               Password = user.Password,
                PasswordHash = hash,
                PasswordSalt = salt,
                Address = user.Address,
               PhoneNumber = user.PhoneNumber,
               Image = user.Image.FileName,
            };

            _db.Users.Add(data);
            _db.SaveChanges();
            return Ok(data);
        }

        //------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------



        //////////// Edit Product  //////////////////////////////
        [HttpPut("Product")]
        public IActionResult EditProduct(int id, [FromForm] ProductsRequestDTO products)
        {
            var EditProduct = _db.Products.FirstOrDefault(p => p.ProductId == id);

            var uploadImageFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            if (!Directory.Exists(uploadImageFolder))
            {
                Directory.CreateDirectory(uploadImageFolder);
            }
            var imageFile = Path.Combine(uploadImageFolder, products.Image.FileName);
            using (var stream = new FileStream(imageFile, FileMode.Create))
            {
                products.Image.CopyToAsync(stream);
            }

            EditProduct.Name = products.Name;
            EditProduct.Description = products.Description;
            EditProduct.Price = products.Price;
            EditProduct.Image = products.Image.FileName;
            EditProduct.Brand = products.Brand;
            EditProduct.PriceWithDiscount = products.PriceWithDiscount;
            EditProduct.CategoryId = products.CategoryId;

            _db.SaveChanges();
            return Ok();
        }

        //////////// Edit Category  //////////////////////////////
        [HttpPut("Category")]
        public IActionResult EditCategory(int id, [FromForm] CategoryRequestDTO category)
        {
            var EditCategory = _db.Categories.FirstOrDefault(p => p.CategoryId == id);

            var uploadImageFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            if (!Directory.Exists(uploadImageFolder))
            {
                Directory.CreateDirectory(uploadImageFolder);
            }
            var imageFile = Path.Combine(uploadImageFolder, category.Image.FileName);
            using (var stream = new FileStream(imageFile, FileMode.Create))
            {
                category.Image.CopyToAsync(stream);
            }

            EditCategory.Name = category.Name;
            EditCategory.Description = category.Description;
            EditCategory.Image = category.Image.FileName;
          

            _db.SaveChanges();
            return Ok();
        }

        //////////// Edit Category  //////////////////////////////
        [HttpPut("User")]
        public IActionResult EditUser(int id, [FromForm] UserRequestDTO user)
        {
            var EditUser = _db.Users.FirstOrDefault(p => p.UserId == id);

            var uploadImageFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            if (!Directory.Exists(uploadImageFolder))
            {
                Directory.CreateDirectory(uploadImageFolder);
            }
            var imageFile = Path.Combine(uploadImageFolder, user.Image.FileName);
            using (var stream = new FileStream(imageFile, FileMode.Create))
            {
                user.Image.CopyToAsync(stream);
            }

            EditUser.Name = user.Name;
            EditUser.Password = user.Password;
            EditUser.Email = user.Email;
            EditUser.Address = user.Address;
            EditUser.PhoneNumber = user.PhoneNumber;
            EditUser.Image = user.Image.FileName;


            _db.SaveChanges();
            return Ok();
        }

        //************************************************************************
        /**********            Admin Profile Update                ***********////
        //**********************************************************************
        [HttpPut("Admin")]
        public IActionResult EditAdmin(int id, [FromForm] AdminDTO admin)
        {


            var EditAdmin = _db.Admins.FirstOrDefault(p => p.AdminId == id);

            var uploadImageFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            if (!Directory.Exists(uploadImageFolder))
            {
                Directory.CreateDirectory(uploadImageFolder);
            }
            var imageFile = Path.Combine(uploadImageFolder, admin.Img.FileName);
            using (var stream = new FileStream(imageFile, FileMode.Create))
            {
                admin.Img.CopyToAsync(stream);
            }

            EditAdmin.Name = admin.Name;
            EditAdmin.Password = admin.Password;
            EditAdmin.Email = admin.Email;
            EditAdmin.Img = admin.Img.FileName;
            
            _db.SaveChanges();
            return Ok(EditAdmin);
        }

        /*/*//*      Add Admin       *//*/*/

        [HttpPost("Admin")]
        public IActionResult AddAdmin(int id, [FromForm] AdminDTO admin)
        {

            byte[] hash;
            byte[] salt;

            PasswordHasher.createPasswordHash(admin.Password, out hash, out salt);

            var EditAdmin = _db.Admins.FirstOrDefault(p => p.AdminId == id);

            var uploadImageFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            if (!Directory.Exists(uploadImageFolder))
            {
                Directory.CreateDirectory(uploadImageFolder);
            }
            var imageFile = Path.Combine(uploadImageFolder, admin.Img.FileName);
            using (var stream = new FileStream(imageFile, FileMode.Create))
            {
                admin.Img.CopyToAsync(stream);
            }

            var data = new Admin
            {
                Name = admin.Name,
                Password = admin.Password,
                Email = admin.Email,
                PasswordHash = hash,
                PasswordSalt = salt,
            };


            _db.Admins.Add(data);
            _db.SaveChanges();
            return Ok();
        }
        //------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------

        //////////// Delete Product  //////////////////////////////
        [HttpDelete("Product")]
        public IActionResult DeleteProduct(int id)
        {
            if (id < 0)
            {
                return BadRequest();
            }
            var DeleteProduct = _db.Products.FirstOrDefault(p => p.ProductId == id);
            if (DeleteProduct == null)
            {
                return NotFound();
            }
            _db.Products.Remove(DeleteProduct);
            _db.SaveChanges();
            return Ok();
        }

        //////////// Delete Category  //////////////////////////////
        [HttpDelete("Category")]
        public IActionResult DeleteCategory(int id)
        {
            if (id < 0)
            {
                return BadRequest();
            }
            var DeleteCategory = _db.Categories.FirstOrDefault(c => c.CategoryId == id);
            if (DeleteCategory == null)
            {
                return NotFound();
            }
            _db.Categories.Remove(DeleteCategory);
            _db.SaveChanges();
            return Ok();
        }


        //////////// Delete User  //////////////////////////////
        [HttpDelete("User")]
        public IActionResult DeleteUser(int id)
        {
            if (id < 0)
            {
                return BadRequest();
            }
            var DeleteUser = _db.Users.FirstOrDefault(c => c.UserId == id);

            if (DeleteUser == null)
            {
                return NotFound();
            }
            _db.Users.Remove(DeleteUser);
            _db.SaveChanges();
            return Ok();
        }


        /****************************************************************/
        /****************************************************************/
        /****************************************************************/


        [HttpGet("Commint")]
        public IActionResult GetComment()
        {
            var comment = _db.Comments.ToList();
            return Ok(comment);
        }
    }
}
