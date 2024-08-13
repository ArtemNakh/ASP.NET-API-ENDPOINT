
using APITest1.Data;
using APITest1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APITest1.Controllers

{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly DataContext _context;

        public ProductController(DataContext context)
        {
            _context = context;
        }

        /*get all products
add product
remove product
update price of a product
----------------
get sorted products by asc or desc
delete many products by ids
get ONE product by special work in product's description*/


        //0-asc
        //1-dec
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts([FromQuery] bool? sorting)
        {
            if (sorting != null)
                switch (sorting)
                {
                    case false:
                        return Ok(await _context.Products.OrderBy(p => p.Price).ToArrayAsync());
                    case true:
                        return Ok(await _context.Products.OrderByDescending(p => p.Price).ToArrayAsync());
                }
            return Ok(await _context.Products.ToArrayAsync());
        }

        [HttpPost]
        public async Task<ActionResult<Product>> AddProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return Created("created",product);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProduct([FromBody] List<int> ids)
        {
            var products = await _context.Products.Where(p => ids.Contains(p.Id)).ToListAsync();
            _context.Products.RemoveRange(products);
            await _context.SaveChangesAsync();
            return NoContent(); 
        }


        [HttpPatch("{id}")]
        public async Task<ActionResult<Product>> UpdatePartProduct(int id, [FromBody] int price)
        {
            var product = await _context.Products.FindAsync(id);    
            product.Price = price;
            await _context.SaveChangesAsync();
            return Ok(product);
        }



    }
}
