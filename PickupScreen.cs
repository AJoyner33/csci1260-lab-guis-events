namespace GuiEventsLab;

// PickupScreen: a subscriber. It listens for OrderReady and keeps the
// lines the screen above the counter would show, like "Ready: #1 Ada".
//
// Part B. Write Connect and OnOrderReady along with the Session 1 demo.
public class PickupScreen
{
    private readonly List<string> lines = new List<string>();

    // Given.
    public List<string> Lines()
    {
        return new List<string>(lines);
    }

    // Subscribe OnOrderReady to the bar's OrderReady event.
    // Subscribes this screen's handler to the bar's OrderReady event
    public void Connect(CoffeeBar bar)
    {
        bar.OrderReady += OnOrderReady; 
    }

    // Runs every time the bar raises OrderReady.
    // Adds a "Ready: #number customer" line to the screen when an order is ready
    private void OnOrderReady(object? sender, OrderEventArgs e)
    {
        lines.Add($"Ready: #{e.Order.Number} {e.Order.Customer}");
    
    }
}
