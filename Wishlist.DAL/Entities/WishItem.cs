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

    [Display(Name = "Наименование")]
    public string Name { get; set; } = null!;

    [Display(Name = "Степень желанности")]
    public DesirabilityDegree DesirabilityDegree { get; set; } = DesirabilityDegree.One;

    [Url]
    [Display(Name = "Ссылка")]
    public string? Link { get; set; }

    [Display(Name = "Описание")]
    public string? Description { get; set; }
    
    [Display(Name = "Стоимость")]
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