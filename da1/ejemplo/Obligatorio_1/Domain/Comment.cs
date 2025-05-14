namespace Domain;

public class Comment
{
    public int Id { get; set; }
    public bool Resolved { get; set; }
    private string _description;

    public string Description
    {
        get => _description;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Description cannot be null or empty");
            }
            _description = value;
        }
    }
    
    public int CreatorId { get; set; }
    public int? ResolverId { get; set; } = null;
    public DateTime? DateResolution { get; set; } = null;
    
    
}