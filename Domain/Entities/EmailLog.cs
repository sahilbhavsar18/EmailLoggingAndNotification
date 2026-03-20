using Application.Common.Enums;
using Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class EmailLog:BaseEntity
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public EmailStatus Status { get; set; }

        public int RetryCount { get; set; }

        public string? ErrorMessage { get; set; }
        public string? ErrorDetails { get; set; }

        public DateTime? SentAt { get; set; }
    }
}
