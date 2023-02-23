using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SignalRAssignment.Models
{
    public partial class Account
    {
        public int AccountId { get; set; }
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;

        [NotMapped]
        public string ConfirmPassword { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public short Type { get; set; }
    }
}
