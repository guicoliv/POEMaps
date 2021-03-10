using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POEMaps
{
    public class UserResults
    {
        public string username;
        public List<MapResult> results;
        public Grid userGrid;
        public int id;
        public TextBox message;

        public UserResults(string username, int id)
        {
            this.id = id;
            this.username = username;
            Application.Current.Dispatcher.Invoke((Action)delegate {
            
                userGrid = new Grid();
                results = new List<MapResult>();
                userGrid.RowDefinitions.Add(new RowDefinition());
                userGrid.RowDefinitions.Add(new RowDefinition());

                Grid topRow = new Grid();
                topRow.ColumnDefinitions.Add(new ColumnDefinition());
                topRow.ColumnDefinitions.Add(new ColumnDefinition());
                topRow.ColumnDefinitions.Add(new ColumnDefinition());

                TextBox identifier = new TextBox();
                identifier.FontSize = 18;
                identifier.Margin = new Thickness(2, 2, 20, 5);
                identifier.FontWeight = FontWeights.Bold;
                identifier.Text = username;
                identifier.IsReadOnly = true;
                identifier.TextWrapping = TextWrapping.Wrap;
                Grid.SetColumn(identifier, 0);

                message = new TextBox();
                message.FontSize = 14;
                message.Text = "@"+username+" Hi, I'd like to buy your ... for my ... Chaos Orb in Ritual.";
                message.IsReadOnly = true;
                message.TextWrapping = TextWrapping.Wrap;
                Grid.SetColumnSpan(message, 2);
                Grid.SetColumn(message, 1);

                topRow.Children.Add(identifier);
                topRow.Children.Add(message);

                Grid.SetRow(topRow, 0);
                userGrid.Children.Add(topRow);

                Grid.SetRow(userGrid, id);
            });

        }

        public int getNResults()
        {
            return results.Count;
        }

        public void addMapResult(MapResult mr)
        {
            results.Add(mr);

            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                TextBlock identifier = new TextBlock();
                identifier.FontSize = 14;
                identifier.Height = 20;
                identifier.Text = mr.m.name + ": " + mr.mapAmount + " for " + mr.currencyAmount + " " + mr.currency;
                Grid.SetRow(identifier, getNResults());


                message.Text = "@" + username + " Hi, I'd like to buy your ";

                int price = 0;

                foreach(MapResult offer in results)
                {
                    price += offer.currencyAmount;
                    message.Text += offer.m.name + " (" + offer.m.tier + "), ";
                }

                message.Text += "for my " + price + " Chaos Orb in Ritual.";

                userGrid.RowDefinitions.Add(new RowDefinition());
                userGrid.Children.Add(identifier);
            });

        }
    }
}
