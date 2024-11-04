﻿using System.ComponentModel.DataAnnotations;

namespace CLDVPart3.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        public string? Name { get; set; }

        public string? ProductDescription { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public decimal? Price { get; set; }

        public string? Category { get; set; }

        public bool? Availability { get; set; }

        public string? ImageUrlpath { get; set; }
    }
}
