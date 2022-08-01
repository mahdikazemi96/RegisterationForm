using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegisterationForm.Domain.Model
{
    public class PersonPersonality
    {
        [Key]
        public int Id { get; set; }
        public int PersonId { get; set; }
        public virtual Person Person { get; set; }

        public int PersonalityId { get; set; }
        public virtual Personality Personality { get; set; }
    }
}
