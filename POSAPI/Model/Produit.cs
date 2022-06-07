namespace POSAPI.Model
{
    public class Produit
    {
        public int Id { get; set; }
        public Category Category { get; set; }
        public DateOnly ExpiryDate { get; set; }
    }
}
