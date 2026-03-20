using Application.Features.Email.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Email.Validators
{
    public class SendEmailValidator : AbstractValidator<SendEmailCommand>
    {
        public SendEmailValidator()
        {
            RuleFor(x=>x.To).NotEmpty().EmailAddress();
            RuleFor(x=>x.Subject).NotEmpty().MaximumLength(200);
            RuleFor(x=>x.To).NotEmpty();
        }
    }
}
