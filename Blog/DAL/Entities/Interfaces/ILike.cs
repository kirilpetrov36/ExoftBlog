namespace Blog.DAL.Entities.Interfaces
{
    public interface ILike : IDate
    {
        long Id { get; set; }
        User User { get; set; }
        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
        bool isEnable { get; set; }

    }
}
