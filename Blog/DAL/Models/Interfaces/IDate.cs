using System;

namespace Blog.DAL.Models.Interfaces
{
    public interface IDate
    {
        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
    }
}
