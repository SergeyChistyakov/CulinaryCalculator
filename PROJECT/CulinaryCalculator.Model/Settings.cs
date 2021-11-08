using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CulinaryCalculator.Model
{
    public class Settings
    {
        [Key]
        public int Id { get; set; }
        public bool UseCloud { get; set; }
        public string ServerUrl { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
