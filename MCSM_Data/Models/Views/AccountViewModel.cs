namespace MCSM_Data.Models.Views
{
    public class AccountViewModel
    {
        public Guid Id { get; set; }

        public string Role { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public DateTime DateOfBirth { get; set; }

        public string Gender { get; set; } = null!;

        public string? Avatar { get; set; }

        public string Status { get; set; } = null!;

        public bool IsOnline { get; set; }


        public DateTime CreateAt { get; set; }

        public DateTime? UpdateAt { get; set; }

    }
}
