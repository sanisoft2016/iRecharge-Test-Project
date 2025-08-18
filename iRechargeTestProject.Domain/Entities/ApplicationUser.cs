using iRechargeTestProject.Domain.Enum;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRechargeTestProject.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [Key, MaxLength(40)]
        public override string Id { get => base.Id; set => base.Id = value; }
        public ACCESS_STATUS AccessStatus { get; set; }
        [MaxLength(30)]
        public string FirstName { get; set; } = string.Empty;
        [MaxLength(30)]
        public string LastName { get; set; } = string.Empty;
        public GENDER Gender { get; set; }
        public CURRENT_STATUS CurrentStatus { get; set; }
        public USER_TYPE UserType { get; set; }
        [MaxLength(25)]
        public string State { get; set; } = string.Empty;
        [MaxLength(25)]
        public string Town { get; set; } = string.Empty;
    }
}
