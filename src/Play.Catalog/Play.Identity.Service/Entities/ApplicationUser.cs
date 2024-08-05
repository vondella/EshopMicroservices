using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace Play.Identity.Service.Entities
{
    [CollectionName("users")]
    public class ApplicationUser:MongoIdentityUser<Guid>
    {
        public decimal Gil { get; set; }
        public HashSet<Guid> MessageIds { get; set; } = new();
    }
}
