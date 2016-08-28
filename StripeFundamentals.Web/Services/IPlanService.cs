using System.Collections.Generic;
using StripeFundamentals.Web.Data;

namespace StripeFundamentals.Web.Services
{
    public interface IPlanService
    {
        Plan Find(int id);

        IList<Plan> List();
    }
}