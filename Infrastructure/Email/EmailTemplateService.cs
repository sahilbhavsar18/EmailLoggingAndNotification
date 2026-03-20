using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Email
{
    public class EmailTemplateService : IEmailTemplateService
    {
        public async Task<string> RenderAsync(string templateName, object Data)
        {
            var path = Path.Combine("Templates", $"{templateName}.html");
            var content = await File.ReadAllTextAsync(path);
            foreach(var prop in Data.GetType().GetProperties())
            {
                var value = prop.GetValue(Data)?.ToString();
                content = content.Replace($"{{{{{prop.Name}}}}}",value);
            }
            return content;
        }
    }
}
