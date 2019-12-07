using System.Collections.Generic;
using Domain.Entities;

namespace Domain.Abstract
{
    public interface ISSSystem
    {
        IEnumerable<t_SSS> SSSList { get; }
        IEnumerable<t_Users> SSSUsers { get; }
    }
}
