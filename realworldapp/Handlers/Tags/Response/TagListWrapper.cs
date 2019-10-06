using System.Collections;
using System.Collections.Generic;

namespace realworldapp.Handlers.Tags.Response
{
    public class TagListWrapper
    {
        public IList<string> Tags { get; set; }
        public int Count => this.Tags.Count;
    }
}