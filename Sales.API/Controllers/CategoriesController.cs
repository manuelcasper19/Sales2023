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
        private readonly DataContext _context;

        public CategoriesController(DataContext context )
        {
            _context = context;
        }

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
