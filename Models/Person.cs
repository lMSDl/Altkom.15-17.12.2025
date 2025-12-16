using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Person : Entity
    {
        public Person? Parent { get; set; }
        //public ICollection<Person> Children { get; set; } = [];

        [Required]
        public string FirstName { get; set; } = string.Empty;
        [StringLength(15)]
        public string LastName { get; set; } = string.Empty;

        [Range(18, 65, ErrorMessage = "Age must be between 18-65")]
        public int Age { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public DateOnly DateOfBirth { get; set; }
        public int DefaultInt { get; set; }
        public string? NullableString { get; set; }
    }
}
