using RegisterationForm.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegisterationForm.Domain.Model
{
    public class Person
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public int Age { get; set; }
        public string Address { get; set; }

        [Column(TypeName = "tinyint")]
        public Gender Gender { get; set; }

        [Column(TypeName = "tinyint")]
        public Status Status { get; set; }

        [NotMapped]
        public string personalities { get; set; }

        [NotMapped]
        public List<int> personalitiesIds { get; set; } 
        public List<PersonPersonality> PersonPersonalities { get; set; }
    }
}
