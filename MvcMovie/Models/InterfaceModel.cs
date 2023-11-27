

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}

public class Product
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Category { get; set; }
    public float Quantity { get; set; }
    public string UnitOfMeasure { get; set; }
    public float UnitPrice { get; set; }
    public int UserId { get; set; }
}
