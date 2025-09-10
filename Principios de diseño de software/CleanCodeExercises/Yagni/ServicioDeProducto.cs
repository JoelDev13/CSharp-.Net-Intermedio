namespace Yagni
{
    public class ServicioDeProducto
    {
        public void AddProduct(string name, decimal price)
        {
            Console.WriteLine($"Producto {name} agregado con el precio de {price}");

        }

        public void DeleteProduct(int productId)
        {
            Console.WriteLine($"Productp {productId} borrado");
        }
    }
}
