using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsClubPlatform.Domain.Common
{
    /// <summary>
    /// Base entity for all domain entities.
    /// </summary>
    public abstract class BaseEntity
    {
        public int Id { get; protected set; }
    }
}
