using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhonebookBL.Model
{
    [Index(nameof(PhonebookId),nameof(Name), IsUnique = true)]
    public class PbEntry
    {
        [Key]
        public int EntryId { get; set; }
        [Required(ErrorMessage = "Kindly supply a phonebook entry name.")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,100}$", ErrorMessage = "Please remove any numbers from the name.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Kindly supply a contact number.")]
        [RegularExpression(@"^[0-9'+'\s]{1,20}$", ErrorMessage = "Phone number should only consist of numbers.")]
        public string ContactNumber { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public int PhonebookId { get; set; }

        //Chris: I wanted to add the photos but I won't be able to finish today.
        //[NotMapped]
        //public IFormFile Image { get; set; }
        //public string ImageUrl { get; set; }

    }
}
