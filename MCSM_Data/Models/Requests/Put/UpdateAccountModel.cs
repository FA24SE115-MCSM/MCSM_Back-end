namespace MCSM_Data.Models.Requests.Put
{
    public class UpdateAccountModel
    {
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }


        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }

        public string? Status { get; set; } 

    }
}
