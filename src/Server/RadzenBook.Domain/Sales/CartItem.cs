﻿using RadzenBook.Domain.Catalog;

namespace RadzenBook.Domain.Sales;

public class CartItem : BaseEntity<Guid>
{
    public int Quantity { get; set; }
    public Guid? CartId { get; set; }
    public virtual Cart? Cart { get; set; }
    public Guid? ProductId { get; set; }
    public virtual Product? Product { get; set; }
}