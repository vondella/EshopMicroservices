using buildingBlock.Entities;

namespace Play.Trading.Entities
{
    public class ApplicationUser : IEntity
    {
        public Guid Id { get; set; }

        public decimal Gil { get; set; }
    }
}
