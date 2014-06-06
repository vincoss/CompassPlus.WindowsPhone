using System.Windows;


namespace CompassPlus.Controls
{
    // TODO: Not used, possible to remove
    public class CompassTypeTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var value = (string)item;
            switch (value)
            {
                case "Plane":
                    {
                        return PlaneTemplate;
                    }
                case "Ship":
                    {
                        return ShipTemplate;
                    }
            }
            return TrekkingTemplate;
        }

        public DataTemplate ShipTemplate { get; set; }

        public DataTemplate PlaneTemplate { get; set; }

        public DataTemplate TrekkingTemplate { get; set; }
    }
}
