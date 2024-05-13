class Shop(Cart cart)
{
    public void AddToCartWithDiscount(Product product, int discount)
    {
        product.Discount = discount;
        cart.Add(product);
    }
}

class Cart
{
    public void Add(Product product) { }
}

class Product
{
    public int Discount { get; set; }
}
