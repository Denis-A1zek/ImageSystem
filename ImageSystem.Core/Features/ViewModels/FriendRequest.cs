using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSystem.Core.Features.ViewModels;

public sealed record FriendRequest(Guid UserId, string Name);

