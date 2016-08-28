using System.Data.Entity;

namespace StripeFundamentals.Web.Data
{
    public interface IPaymentsModel
    {
        DbSet<Feature> Features { get; set; }
        DbSet<Plan> Plans { get; set; }
    }
}