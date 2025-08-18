using iRechargeTestProject.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRechargeTestProject.Application.DTOs
{
    public class UserDto
    {
        public string Id { get; set; } = string.Empty;
        [MaxLength(50)]
        public string UserName { get; set; } = string.Empty;

        [MaxLength(300)]
        public string Password { get; set; } = string.Empty;


        [MaxLength(50)]
        public string Email { get; set; } = string.Empty;
        [MaxLength(50)]
        public string PhoneNumber { get; set; } = string.Empty;
        [MaxLength(30)]
        public string FirstName { get; set; } = string.Empty;
        [MaxLength(30)]
        public string LastName { get; set; } = string.Empty;
        [MaxLength(25)]
        public string State { get; set; } = string.Empty;
        [MaxLength(25)]
        public string Town { get; set; } = string.Empty;
        public GENDER Gender { get; set; }

    }
}
