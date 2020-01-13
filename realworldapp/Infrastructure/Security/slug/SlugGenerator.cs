using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace realworldapp.Infrastructure.Security
{
    public class SlugGenerator: ISlugGenerator
    {
        public string Generate()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}
