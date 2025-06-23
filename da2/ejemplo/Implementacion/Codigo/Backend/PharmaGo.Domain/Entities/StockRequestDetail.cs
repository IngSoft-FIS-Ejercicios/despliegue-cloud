using PharmaGo.Exceptions;

namespace PharmaGo.Domain.Entities
{
    public class StockRequestDetail
    {
        public int Id { get; set; }
        public Drug Drug { get; set; }

        private int _quantity;

        public int Quantity
        {
            get => _quantity;
            set
            {
                ValidateQuantity(value);
                _quantity = value;
            }
        }

        private void ValidateQuantity(int quantity)
        {
            if (quantity <= 0)
            {
                throw new InvalidResourceException("The amount must be a positive number");
            }
        }
    }
}