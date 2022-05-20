namespace Fitters.Models
{
    public class DetailedFitterViewModel
    {
        public FitterViewModel Fitter { get; set; }

        //These are used for adding an underFitter
        public int OverFitterId { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public int FitterType { get; set; }

        public DetailedFitterViewModel()
        {
            FitterType = 2;
        }
    }
}
