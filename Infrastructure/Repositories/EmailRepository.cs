using Application.Interfaces;
using Domain.Entities;
using Infrastructure.BackgroundJobs;
using Infrastructure.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class EmailRepository : IEmailRepository
    {
        private readonly AppDbContext _context;
        public EmailRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(EmailLog emailLog)
        {
            await _context.EmailLogs.AddAsync(emailLog);
            await _context.SaveChangesAsync();
        }

        public async Task<EmailLog?> GetByIdAsync(Guid id)
        {
            return await _context.EmailLogs.FindAsync(id);
        }

        public async Task UpdateAsync(EmailLog emailLog)
        {
            _context.EmailLogs.Update(emailLog);
        }
    }
}
