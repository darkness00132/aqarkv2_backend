using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.User
{
    public class UserDTO
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public required string Email { get; set; }

        public required int Credits { get; set; }
    }
}
