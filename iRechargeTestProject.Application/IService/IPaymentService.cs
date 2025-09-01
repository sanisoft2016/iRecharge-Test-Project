using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRechargeTestProject.Application.IService
{
    public interface IPaymentService
    {
        Task<string> ProcessPaymentAsync(object paymentInfo);
    }
}
