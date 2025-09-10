namespace Kiss
{
    public class ProductService
    {
        private decimal _total = 0;
        private const decimal _tipPercentageBase = 0.10m;


        public decimal CalculateTotal(decimal[] prices, decimal? tipPercentage)
        {


            foreach (var pr in prices)
            {
                _total += pr;
            }

            if (tipPercentage.HasValue)
            {
                _total += _total * (tipPercentage.Value / 100);
            }
            else
            {
                _total += _total * _tipPercentageBase;
            }

            return _total;
        }
    }
}
