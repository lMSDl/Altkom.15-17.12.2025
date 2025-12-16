namespace Models
{
    public class Person : Entity
    {
        public Person? Parent { get; set; }
        public ICollection<Person> Children { get; set; } = [];

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string FullName => $"{FirstName} {LastName}";

        public DateOnly DateOfBirth { get; set; }
        public int DefaultInt { get; set; }
        public string? NullableString { get; set; }
    }
}
