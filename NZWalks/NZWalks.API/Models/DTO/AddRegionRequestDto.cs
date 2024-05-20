using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NZWalks.API.Models.DTO
{
    public class AddRegionRequestDto
    {
        [Required]
        [MinLength(3,ErrorMessage ="Code has to be a minimunm of 3 char")]
        [MaxLength(3, ErrorMessage = "Code has to be a maximunm of 3 char")]
        public string Code { get; set; } 
        [Required]
        [MaxLength(100,ErrorMessage ="Name has to be a maximum of 100 char")]
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
