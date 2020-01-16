using System;

namespace realworldapp.Infrastructure.Security.slug
{
    public class SlugGenerator: ISlugGenerator
    {
        public string Generate()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}
