using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PhonebookBL.Model
{
    public enum Severity
    {
        Information,
        Warning,
        Error
    }
    public class Log
    {
        public int Id { get; set; }

        public string Message { get; set; }

        [Column(TypeName = "nvarchar(24)")]
        public Severity Serverity { get; set; } = Severity.Information;
        public DateTime CreateDate { get; set; } = DateTime.Now;

    }
}
