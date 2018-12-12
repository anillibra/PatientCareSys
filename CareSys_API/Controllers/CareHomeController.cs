using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CareSys_API.Dtos;
using CareSys_API.Models;
using CareSys_API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CareSys_API.Controllers
{
    [Route("api/[controller]")]
    public class CareHomeController : Controller
    {
        private readonly ICareHomeRepository _repo;
        private readonly IMapper _mapper;

        public CareHomeController(ICareHomeRepository careHomeRepository, IMapper mapper)
        {
            _repo = careHomeRepository;
            _mapper = mapper;
        }

        // This is SECURED Action.
        //GET: api/carehome/all
        [Authorize(Policy = "CanReadCareHome")]
        [HttpGet("all")]
        public async Task<IActionResult> GetCareHomes()
        {
            var careHomesModel = await _repo.GetAllCareHomesAsync();

            // Map Model to Dtos
            var careHomeDtos = _mapper.Map<IEnumerable<CareHomesDtos>>(careHomesModel);

            return Ok(careHomeDtos);
        }

        // GET: api/carehome/category/old age
        [HttpGet("category/{category:length(1,100)}")]
        public async Task<IActionResult> GetCareHomeByCategory(string category)
        {
            if (category == null)
            {
                return BadRequest();
            }
            var careHomesModel = await _repo.GetCareHomesByCategoryAsync(category);

            // Map Model to Dtos
            // var careHomeDtos = Mapper.Map<IEnumerable<CareHomes>>(careHomesModel);
            var careHomeDtos = _mapper.Map<IEnumerable<CareHomesDtos>>(careHomesModel);

            return Ok(careHomeDtos);
        }

        // GET: api/carehome/active/{isactive:bool}
        [HttpGet("active/{isactive:bool}")]
        public async Task<IActionResult> GetCareHomeActive(Boolean isActive)
        {
            var careHomesModel = await _repo.GetCareHomesByStatusAsync(isActive);

            // Map Model to Dtos
            var careHomeDtos = _mapper.Map<IEnumerable<CareHomesDtos>>(careHomesModel);

            return Ok(careHomeDtos);
        }

        // Send isactive in query string
        // Get: api/carehome/active?isactive=true
        [HttpGet("active")]
        public async Task<IActionResult> GetCareHomeActiveByQuery([FromQuery]Boolean isActive)
        {
            var careHomesModel = await _repo.GetCareHomesByStatusAsync(isActive);

            // Map Model to Dtos
            var careHomeDtos = _mapper.Map<IEnumerable<CareHomesDtos>>(careHomesModel);

            return Ok(careHomeDtos);
        }

        // Create new CareHome
        // POST: api/carehome
        [Authorize(Policy = "CanCreateCareHome")]
        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateNewCareHome([FromBody] MessageCareHomeForCreationDtos newCareHomeDto)
        {
            // This is not best practise, duplicate code, have ActionFilter for this.
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest();
            //}
            var careHome = _mapper.Map<CareHome>(newCareHomeDto);
            if (!TryValidateModel(careHome))
            {
                return BadRequest();
            }
                await _repo.AddAsync(careHome);

            return Created("CareHomeCreated", new { id = careHome.Id });
        }

    }
}