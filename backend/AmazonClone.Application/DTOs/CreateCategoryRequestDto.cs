using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AmazonClone.Application.DTOs
{
    public class CreateCategoryRequestDto
    {
        [Required]
        [MaxLength(45)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(80)]
        public string Description { get; set; } = string.Empty;
    }
}
