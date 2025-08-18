using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRechargeTestProject.Application.DTOs
{
    public class ResponseDto
    {
        public string Status { get; set; } = string.Empty;
        public object Data { get; set; } = new object();
    }
}
