// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

internal class Shop( Cart cart )
{
    public void AddToCartWithDiscount( Product product, int discount )
    {
        product.Discount = discount;
        cart.Add( product );
    }
}

internal class Cart
{
    public void Add( Product product ) { }
}

internal class Product
{
    public int Discount { get; set; }
}