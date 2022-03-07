using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PhonebookBL.Model
{
    [Index(nameof(Name), IsUnique =true)]
    public class Phonebook
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Kindly supply a phonebook name.")]
        public string Name { get; set; }

        public ICollection<PbEntry> Entries { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
