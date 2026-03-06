using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface ICurrentUser
    {
        string? UserId { get; }
        bool IsAuthenticated { get; }
    }

}
