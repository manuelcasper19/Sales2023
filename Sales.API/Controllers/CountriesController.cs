using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sales.API.Data;
using Sales.API.Helpers;
using Sales.Shared.DTOs;
using Sales.Shared.Entities;

namespace Sales.API.Controllers
{
    [ApiController]
    [Route("/api/countries")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class CountriesController : Controller
    {
        private readonly DataContext _context;
        public CountriesController( DataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public  async Task<ActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.Countries
                            .Include(x => x.States)
                            .AsQueryable();
            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }


            return Ok(await queryable
                .OrderBy( c => c.Name)
                .Paginate(pagination)
                .ToListAsync());
        }

        //metodo para llenar drop down list de paises de forma anonima, cuando se cree el usuario
        [AllowAnonymous]
        [HttpGet("combo")]
        public async Task<ActionResult> GetCombo()
        {
            return Ok(await _context.Countries.ToListAsync());
        }


        [HttpGet("totalPages")]
        public async Task<ActionResult> GetPages([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.Countries.AsQueryable();
            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            double count = await queryable.CountAsync();
            double totalPages = Math.Ceiling(count / pagination.RecordsNumber);
            return Ok(totalPages);
        }

        [HttpGet("full")]
        public async Task<ActionResult> GetFullAsync()
        {
            return Ok(await _context.Countries
                .Include(s => s.States!)
                .ThenInclude( c => c.Cities)
                .ToListAsync());
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetAsync( int id )
        {
            var country = await _context.Countries
                .Include(s => s.States!)
                .ThenInclude(c => c.Cities)
                .FirstOrDefaultAsync( c => c.Id == id);
            if(country == null)
            {
                return NotFound();
            }
            return Ok( country );
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var country = await _context.Countries.FirstOrDefaultAsync(c => c.Id == id);
            if (country == null)
            {
                return NotFound();
            }
            _context.Remove(country);
            await _context.SaveChangesAsync();
            return NoContent();
          
        }

        [HttpPost]
        public async Task<ActionResult> PostAsnc( Country country)
        {
            _context.Add(country);
            try
            {
                await _context.SaveChangesAsync();
                return Ok(country);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe un país con el mismo nombre.");
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
        [HttpPut]
        public async Task<ActionResult> PutAsync(Country country)
        {
            _context.Update(country);
            try
            {
                await _context.SaveChangesAsync();
                return Ok(country);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe un registro con el mismo nombre.");
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

        //private async Task<ActionResult> SaveAndEdit( Country country)
        //{
        //    try
        //    {
        //        if (country.Id.Equals(null))
        //        {
        //            _context.Add(country);
        //            await _context.SaveChangesAsync();
        //            return Ok(country);
        //        }
        //        else
        //        {
        //            _context.Update(country);
        //            await _context.SaveChangesAsync();
        //            return Ok(country);

        //        }


        //    }
        //    catch(DbUpdateException dbUpdateException)
        //    {
        //        if (dbUpdateException.InnerException!.Message.Contains("duplicate"))
        //        {
        //            return BadRequest("Ya existe un país con el mismo nombre.");
        //        }
        //        else
        //        {
        //            return BadRequest(dbUpdateException.InnerException.Message);
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        return BadRequest(exception.Message);
        //    }

        //}
    }
}
