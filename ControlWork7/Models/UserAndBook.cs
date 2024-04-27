namespace ControlWork7.Models;

public class UserAndBook
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int BookId { get; set; }
    public DateTime GetBookDate { get; set; }
    public DateTime? ReturnBookDate { get; set; }
    public User? User { get; set; }
    public Book? Book { get; set; }
}
