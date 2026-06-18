namespace GymManagementSystemDAL.Data.Models
{
    public class Plan : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int DurationDays { get; set; }
        public decimal Price { get; set; }
        public bool isActive { get; set; }
        #region relationship 
        public ICollection<Membership> memberships { get; set; } = default!; 
        #endregion
    }
}
