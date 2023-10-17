using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSystem.Core.Features;

public sealed record ImageInfo
{
    public ImageInfo(Guid ownerId, string name, string url)
    {
        OwnerId = ownerId;
        Name = name;
        Url = url;
    }

    public Guid OwnerId { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
}
