namespace FittersService.Models
{
    public class Fitter
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public int FitterType { get; set; }
        public IEnumerable<Fitter> UnderFitters { get; set; }
    }
}
