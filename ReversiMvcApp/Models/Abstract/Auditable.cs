using System;

namespace ReversiMvcApp.Models.Abstract
{
    public interface IAuditable
    {
        DateTime? CreatedOn { get; set; }
        DateTime? LastUpdated { get; set; }
    }
}
