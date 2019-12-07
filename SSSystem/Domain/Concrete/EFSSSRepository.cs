using System.Collections.Generic;
using Domain.Entities;
using Domain.Abstract;

namespace Domain.Concrete
{
    public class EFSSSRepository: ISSSystem
    {
        Model context = new Model();
        public IEnumerable<t_SSS> SSSList
        {
            get { return context.t_SSS; }
        }
        public IEnumerable<t_Users> SSSUsers
        {
            get { return context.t_Users; }
        }
    }
}
