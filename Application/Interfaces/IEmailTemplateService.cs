using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IEmailTemplateService
    {
        Task<string> RenderAsync(string templateName,object Data);
    }
}
