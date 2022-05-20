namespace FittersService.Models
{
    public class AddUnderFitterDTO
    {
        public Fitter Fitter { get; set; } 
        public int OverFitterId { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public int FitterType { get; set; }

        public AddUnderFitterDTO()
        {
            FitterType = 2;
        }
    }
}
