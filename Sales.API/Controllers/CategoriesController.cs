using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sales.API.Data;
using Sales.Shared.Entities;

namespace Sales.API.Controllers
{
    [ApiController]
    [Route("/api/categories")]
    public class CategoriesController : ControllerBase
    {
        //attributes
        private readonly DataContext _context;

        //constructor
        public CategoriesController(DataContext context )
        {
            _context = context;
        }

        //method that queries and list all categories
        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            try
            {
                return Ok(await _context.Categories.ToListAsync());
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //method that queries a category by id
        [HttpGet("{id}")]
        public async Task<ActionResult> GetAsync(int id)
        {
            try
            {
                var category = await FindCategory(id);
                if( category == null)
                {
                    return NotFound();
                }


                return Ok(category);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //method that deletes a category by id
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            try 
            {
                var category = await FindCategory(id);
                if (category == null)
                {
                    return NotFound();
                }
                _context.Remove(category);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

        //method that saves and updates a category
        [HttpPost]
        public async Task<ActionResult> PostandPutAsync(Category category)
        {
            
            try
            {
                if( category.Id.Equals( null) || category.Id.Equals(0))
                {
                    _context.Add(category);
              
                }
                else
                { 

                    _context.Update(category);
           
                }
                await _context.SaveChangesAsync();
                return Ok(category);

            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe una categoría con el mismo nombre.");
                }
      
                else
                {
                    return BadRequest(dbUpdateException.InnerException.Message);
                }
            }

            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }


        }

        //method private that find a category by id
        private async Task<Category?> FindCategory( int id)
        {
           
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category != null)
            {
                return category;
               
            }

            return null;
        }

    }
}
