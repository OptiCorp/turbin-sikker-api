namespace turbin.sikker.core.Model
{
    public class User
    {
        public int Id { get; set; }
        
        public UserRole UserRole { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

    }
}
