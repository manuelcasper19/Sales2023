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
    [Route("/api/cities")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class CitiesController : ControllerBase
    {
        private readonly DataContext _context;

        public CitiesController (DataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.Cities
                            .Where(x => x.State!.Id == pagination.Id)
                            .AsQueryable();
            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            return Ok(await queryable
                .OrderBy(x => x.Name)
                .Paginate(pagination)
                .ToListAsync());

        }
        //metodo para llenar drop down list de ciudades de forma anonima, cuando se cree el usuario
        [AllowAnonymous]
        [HttpGet("combo/{stateId:int}")]
        public async Task<ActionResult> GetCombo(int stateId)
        {
            return Ok(await _context.Cities
                .Where(x => x.StateId == stateId)
                .ToListAsync());
        }


        [HttpGet("totalPages")]
        public async Task<ActionResult> GetPages([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.Cities
                .Where(x => x.State!.Id == pagination.Id)
                .AsQueryable();
            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            double count = await queryable.CountAsync();
            double totalPages = Math.Ceiling(count / pagination.RecordsNumber);
            return Ok(totalPages);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult> GetAsync(int id)
        {
            var city = await _context.Cities.FirstOrDefaultAsync(s => s.Id == id);
            if (city == null)
            {
                return NotFound();
            }
            return Ok(city);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var city = await _context.Cities.FirstOrDefaultAsync(s => s.Id == id);
            if (city == null)
            {
                return NotFound();
            }
            _context.Remove(city);
            await _context.SaveChangesAsync();
            return NoContent();

        }

        [HttpPost]
        public async Task<ActionResult> PostAsnc(City city)
        {
            _context.Add(city);
            try
            {
                await _context.SaveChangesAsync();
                return Ok(city);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe una ciudad con el mismo nombre.");
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
        public async Task<ActionResult> PutAsync(City city)
        {
            _context.Update(city);
            try
            {
                await _context.SaveChangesAsync();
                return Ok(city);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe una ciudad con el mismo nombre.");
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

        private async Task<ActionResult> SaveAndEdit(State state)
        {
            try
            {
                if (state.Id.Equals(null))
                {
                    _context.Add(state);
                    await _context.SaveChangesAsync();
                    return Ok(state);
                }
                else
                {
                    _context.Update(state);
                    await _context.SaveChangesAsync();
                    return Ok(state);

                }


            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe un estado con el mismo nombre.");
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
    }
}
