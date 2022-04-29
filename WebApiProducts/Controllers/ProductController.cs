using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebApiProducts.Data;
using WebApiProducts.Models;

namespace WebApiProducts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly UserDbContext _context;
        public ProductController(UserDbContext userDbContext)
        {
            _context = userDbContext;
        }

        [HttpGet("getProducts")]
        public IActionResult GetProducts()
        {
            var products = _context.productModels.AsQueryable();
            return Ok(new
            {
                StatusCode = 200,
                Products = products
            });
        }
        [HttpGet("getProduct/{id}")]
        public IActionResult GetProduct(int id)
        {
            var product = _context.productModels.Find(id);
            if (product == null)
            {
                return NotFound(new
                {
                    StatusMessage = 404, 
                    Message = "Product Not Found"
                });
            }
            return Ok(new
            {
                StatusCode = 200,
                Products = product
            });
        }
        [HttpPost("add_product")]
        public IActionResult AddProduct([FromBody] ProductModel product)
        {
            if (product == null)
            {
                return BadRequest();
            }
            else
            {
                _context.productModels.Add(product);
                _context.SaveChanges();
                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Product added Successfully"
                });
            }
        }
        [HttpPut("update_product")]
        public IActionResult UpdateProduct([FromBody] ProductModel product)
        {
            if (product == null)
            {
                return BadRequest();
            }
            var productObj = _context.productModels.AsNoTracking().FirstOrDefault(x => x.Id == product.Id);
            if(productObj == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Product Not Found"
                });
            }
            else
            {
                _context.Entry(product).State = EntityState.Modified;
                _context.SaveChanges();
                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Product Updated Successfully"
                });
            }
        }

        [HttpDelete("delete_product/{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var product = _context.productModels.Find(id);
            if (product == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Product Not Found"
                });
            }
            else
            {
                _context.Remove(product);
                _context.SaveChanges();
                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Product Deleted Successfully"
                });
            }
        }
    }
}
