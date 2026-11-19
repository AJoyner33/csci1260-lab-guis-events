using Avalonia.Controls;
using Avalonia.Interactivity;

namespace GuiEventsLab;

// MainWindow: the code behind the window. It shows the coffee bar and asks
// it to do things. Every decision about orders stays in CoffeeBar.
//
// Part C and Part D are written along with the Session 2 demos.
// Exercise 3 and Exercise 4 are yours.
public partial class MainWindow : Window
{
    private readonly CoffeeBar bar = new CoffeeBar();
    private readonly Rewards rewards = new Rewards();

    // Builds the window, subscribes to the bar's and Rewards' events, and sets the starting button states
    public MainWindow()
    {
        InitializeComponent();

        // Part C: the window listens to the bar like any other subscriber.
        bar.OrderPlaced += Bar_OrderPlaced;

        // Exercise 3
        bar.OrderReady += Bar_OrderReady;
        bar.AllCaughtUp += Bar_AllCaughtUp;

        // Part D: typing in either box decides whether Place can work.
        CustomerBox.TextChanged += Boxes_TextChanged;
        DrinkBox.TextChanged += Boxes_TextChanged;

        // Exercise 4
        rewards.Connect(bar);
        rewards.FreeDrinkEarned += Rewards_FreeDrinkEarned;

        UpdateButtons();
    }

    // Part C: read the boxes, ask the bar, and show any refusal to the person.
    // Reads both boxes, asks the bar to place the order, and shows any refusal in StatusText.
    private void PlaceButton_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            bar.Place(CustomerBox.Text ?? "", DrinkBox.Text ?? "");
            CustomerBox.Text = "";
            DrinkBox.Text = "";
            StatusText.Text = "";
        }
        catch (ArgumentException ex)
        {
            StatusText.Text = ex.Message;
        }
    }

    // Part C: the bar announced a new order, so show the line again.
    // Refreshes the waiting list and button states when an order is placed
    private void Bar_OrderPlaced(object? sender, OrderEventArgs e)
    {
        ShowWaiting();
        UpdateButtons();
    }

    // Exercise 3: ask the bar to finish the oldest order.
    // Asks the bar to finish the oldest waiting order
    private void ReadyButton_Click(object? sender, RoutedEventArgs e)
    {
        bar.FinishNext();
    }

    // Exercise 3: move the finished order to the pickup list.
    // Adds the finished order to the pickup list, refreshes the waiting list, and updates the buttons
    private void Bar_OrderReady(object? sender, OrderEventArgs e)
    {
        PickupList.Items.Add($"#{e.Order.Number} {e.Order.Customer}");
        ShowWaiting();
        UpdateButtons();
    }

    // Exercise 3: the line is empty.
    // Shows "All caught up." in the status text
    private void Bar_AllCaughtUp(object? sender, EventArgs e)
    {
        StatusText.Text = "All caught up.";
    }

    // Part D: runs every time either box changes.
    // Re-checks which buttons should be enabled when either box changes
    private void Boxes_TextChanged(object? sender, TextChangedEventArgs e)
    {
        UpdateButtons();
    }

    // Exercise 4: a subscriber of a subscriber.
    // Shows which customer earned a free drink in RewardText
    private void Rewards_FreeDrinkEarned(object? sender, OrderEventArgs e)
    {
        RewardText.Text = $"Free drink earned: {e.Order.Customer}";
    }

    // Part C: rebuild the waiting list from the bar, oldest first.
    // Refills the Waiting list from the bar's line, oldest first
    private void ShowWaiting()
    {
        WaitingList.Items.Clear();
        foreach (Order order in bar.Waiting())
        {
            WaitingList.Items.Add(order.ToString());
        }
    }

    // Part D: a button is enabled only when pressing it can work.
    // Enables Place only when both boxes have text and Ready only when an order is waiting
    private void UpdateButtons()
    {
        PlaceButton.IsEnabled = !string.IsNullOrWhiteSpace(CustomerBox.Text)
                                && !string.IsNullOrWhiteSpace(DrinkBox.Text);
        ReadyButton.IsEnabled = bar.WaitingCount > 0;
    }
}
