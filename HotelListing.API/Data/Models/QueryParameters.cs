namespace HotelListing.API.Data.Models
{
    public class QueryParameters
    {
        private int _pageSize = 15;
        //public int StartIndex { get; set; }
        public int PageNumber { get; set; }
        internal int Skip
        {
            get
            {
                if (PageNumber <= 1)
                {
                    return 0;
                }
                else
                {
                    return (PageNumber - 1) * PageSize;
                }
            }
        }
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = value;
            }
        }
    }
}
