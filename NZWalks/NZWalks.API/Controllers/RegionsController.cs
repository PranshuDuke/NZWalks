using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
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
    
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(NZWalksDbContext dbContext,IRegionRepository regionRepository,IMapper mapper)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        //public NZWalksDbContext DbContext { get; }

        [HttpGet]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAll()
        {
            var regions = await regionRepository.GetAllAsync();
            //mapping Domian Models to DTOs
            /*var regionDto = new List<RegionDTO>();
            foreach (var regionDomian in regions)
            {
                regionDto.Add(new RegionDTO()
                {
                    Id = regionDomian.Id,
                    Code = regionDomian.Code,
                    Name = regionDomian.Name,
                    RegionImageUrl = regionDomian.RegionImageUrl
                });

            };*/
            var regionDto = mapper.Map<List<RegionDTO>>(regions);

            return Ok(regionDto);
        }

        //Get region by id
        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //var region = dbContext.Regions.Find(id);
            var region = await regionRepository.GetByIdAsync(id);
            if (region == null)
            {
                return NotFound();
            }

            // Map to DTO
            var regionDto = new RegionDTO
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                RegionImageUrl = region.RegionImageUrl
            };
            //we can also do the same by just one line of code
            // return Ok(mapper.Map<RegionDTO>(region);
            //this line will map automatically using automapper
            return Ok(regionDto);
        }

        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            //Map or convert DTO to Domain Model
            /*   var regionDomainModel = new Region
               {
                   Code = addRegionRequestDto.Code,
                   Name = addRegionRequestDto.Name,
                   RegionImageUrl = addRegionRequestDto.RegionImageUrl
               };*/

         
                //This line do the same work done by above commented code
                var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);

                //Use Domain Model to create Region
                regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

                //Map domain model back to DTO
                var regionDto = new RegionDTO
                {
                    Id = regionDomainModel.Id,
                    Code = regionDomainModel.Code,
                    Name = regionDomainModel.Name,
                    RegionImageUrl = regionDomainModel.RegionImageUrl
                };
                //Instead of this we can use
                //var regionDto = mapper.Map<RegionDTO>(regionDomainModel);

                return CreatedAtAction(nameof(GetById), new { id = regionDomainModel.Id }, regionDto);
         
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            if (ModelState.IsValid)
            {
                var regionDomainModel = new Region
                {
                    Code = updateRegionRequestDto.Code,
                    Name = updateRegionRequestDto.Name,
                    RegionImageUrl = updateRegionRequestDto.RegionImageUrl
                };
                regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);

                if (regionDomainModel == null)
                {
                    return NotFound();
                }

                var regionDto = new RegionDTO
                {
                    Id = regionDomainModel.Id,
                    Code = regionDomainModel.Code,
                    Name = regionDomainModel.Name,
                    RegionImageUrl = regionDomainModel.RegionImageUrl
                };
                return Ok(regionDto);
            }
            else
            {
                return BadRequest(ModelState);
            }
            

        }
        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id);
            if(regionDomainModel == null)
            {
                return NotFound();
            }
            
           
            //return the deleted region
            //for that we need to convert it to DTO
            var regionDto = new RegionDTO
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl

            };
            return Ok(regionDto);
        }
    }
}
