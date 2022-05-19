using Fitters.Models.Helpers;

namespace Fitters.Models
{
    public class FitterViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public FitterTypeEnum FitterType { get; set; }

        public IEnumerable<FitterViewModel>? UnderFitters { get; set; }
    }
}
