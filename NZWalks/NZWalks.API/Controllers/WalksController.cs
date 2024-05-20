using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;

        public WalksController(IMapper mapper,IWalksRepository walksRepository)
        {
            this.mapper = mapper;
            WalksRepository = walksRepository;
        }

        public IWalksRepository WalksRepository { get; }

        //Create Walk
        //Post: /api/walks
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            //Map DTO to Domain Model
            if (ModelState.IsValid)
            {
                var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);
                await WalksRepository.CreateAsync(walkDomainModel);

                //Map Domain model to DTO
                return Ok(mapper.Map<WalkDto>(walkDomainModel));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        [HttpGet]
        //Get Walks
        //Get: /api/walks?filterOn=Name&filterQuery=Track&sortby=Name&isAscending=true&pageNumber=1&pagesize=10
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn,[FromQuery] string? filerQuery,
            [FromQuery] string? sortBy,[FromQuery] bool? isAscending,
            [FromQuery] int pageNumber =1,[FromQuery] int pageSize = 1000)
        {
            var walksDomainModel =  await WalksRepository.GetAllAsync(filterOn,filerQuery,sortBy,isAscending ?? true,pageNumber,pageSize);
            //Map Domain model to dto
            return Ok(mapper.Map<List<WalkDto>>(walksDomainModel));
        }
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id) 
        {
            var walkDomainModel = await WalksRepository.GetByIdAsync(id);
            if (walkDomainModel == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id,UpdateWalkRequestDto updateWalkRequestDto)
        {
            //Map Dto to domain model
            if (ModelState.IsValid)
            {
                var walkDomainModel = mapper.Map<Walk>(updateWalkRequestDto);
                walkDomainModel = await WalksRepository.UpdateAsync(id, walkDomainModel);
                if (walkDomainModel == null)
                {
                    return NotFound();
                }
                return Ok(mapper.Map<WalkDto>(walkDomainModel));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deletedWalkDomainModel = await WalksRepository.DeleteAsync(id);
            if(deletedWalkDomainModel == null)
            {
                return NotFound();
            }
            //map domain to dto
            return Ok(mapper.Map<WalkDto>(deletedWalkDomainModel));
        }
    }
}
