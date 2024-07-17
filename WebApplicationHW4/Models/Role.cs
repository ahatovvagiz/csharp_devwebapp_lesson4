namespace WebApplicationHW4.Models
{
    public class Role
    {
        public string Name { get; set; }
        public RoleId RoleId { get; set; }
        public virtual List<User> Users { get; set; }
    }
}
