
namespace Application.Core
{
    public class PagingParams
    {
        private const int MaxPAgeSize = 5;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPAgeSize) ? MaxPAgeSize : value;
        }
        
    }
}