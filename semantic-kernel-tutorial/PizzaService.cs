using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace SemanticKernelTutorial;

public enum PizzaSize
{
    Small,
    Medium,
    Large
}

public class PizzaOrder
{
    public Guid OrderId { get; set; } = Guid.NewGuid();
    public string Size { get; set; } = string.Empty;
    public string Toppings { get; set; } = string.Empty;
    public int Quantity { get; set; } = 1;
    public string SpecialInstructions { get; set; } = string.Empty;
}

public class OrderPizzaPlugin(IPizzaService pizzaService)
{
    [KernelFunction("add_pizza_to_cart")]
    [Description("Adds a pizza to the user's cart; returns the order details.")]
    public async Task<PizzaOrder> AddPizzaToCartAsync(string toppings, string size = "Medium", int quantity = 1, string specialInstructions = "") =>
      await pizzaService.AddPizzaToCartAsync(size, toppings, quantity, specialInstructions);

    [KernelFunction("order_pizza")]
    [Description("Orders the pizza in the cart.")]
    public async Task<PizzaOrder> OrderPizzaAsync(Guid orderId) => await pizzaService.OrderPizzaAsync(orderId);
}

public interface IPizzaService
{
    List<PizzaOrder> Orders { get; }
    Task<PizzaOrder> AddPizzaToCartAsync(string size, string toppings, int quantity, string specialInstructions);
    Task<List<PizzaOrder>> GetCartAsync();
    Task<PizzaOrder> OrderPizzaAsync(Guid orderId);
}

public class PizzaService : IPizzaService
{
    public List<PizzaOrder> Orders { get; } = [];

    public async Task<PizzaOrder> AddPizzaToCartAsync(string size, string toppings, int quantity, string specialInstructions)
    {
        var Order = new PizzaOrder
        {
            Size = size,
            Toppings = toppings,
            Quantity = quantity,
            SpecialInstructions = specialInstructions
        };

        Orders.Add(Order);

        return await Task.FromResult(Order);
    }

    public async Task<List<PizzaOrder>> GetCartAsync() => await Task.FromResult(Orders);

    public async Task<PizzaOrder> OrderPizzaAsync(Guid orderId) =>
    await Task.FromResult(Orders.Single(o => o.OrderId == orderId));
}
