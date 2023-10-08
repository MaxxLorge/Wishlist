using System.ComponentModel.DataAnnotations;

namespace Wishlist.DAL.Entities;

public enum DesirabilityDegree
{
    One = 1,
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Ten
}

public class WishItem
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DesirabilityDegree DesirabilityDegree { get; set; } = DesirabilityDegree.One;

    [Url]
    public string? Link { get; set; }

    public string? Description { get; set; }
    
    public decimal? Cost { get; set; }

    public int UserId { get; set; }

    public User User { get; set; } = null!;

    public override string ToString()
    {
        return
            $"Наименование: {Name}{Environment.NewLine}" +
            $"Степень желанности: {(int)DesirabilityDegree}{Environment.NewLine}" +
            $"Ссылка: {Link}{Environment.NewLine}" +
            $"Описание: {Description}{Environment.NewLine}" +
            $"Стоимость: {Cost}";
    }
}