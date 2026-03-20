using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IEmailRepository
    {
        Task AddAsync(EmailLog emailLog);
        Task<EmailLog?> GetByIdAsync(Guid id);

        Task UpdateAsync(EmailLog emailLog);
    }
}
