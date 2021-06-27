﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CulinaryCalculator.Model
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public byte[] Image { get; set; }

        public List<Recipe> Recipes { get; set; } = new List<Recipe>();
    }
}
