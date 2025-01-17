﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NZWalks.API.Models.DTO
{
    public class WalkDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Desciption { get; set; }
        public double LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }
        

        public RegionDTO Region { get; set; }
        public DifficultyDto Difficulty { get; set; }

    }
}
