using CompassPlus.Models;


namespace CompassPlus
{
    public static class Constants
    {
        public const string RemoteLogUri = "http://error.vincoss.com/error/log";

        public const string ExceptionLogToken = "A67F6D1C-91F2-478E-9F1E-4DFC986BBDAE";

        public const string Name = "CompassPlus";

        public const string DefaultCompassType = "Trekking";

        public const string DefaultCompassColor = "Orange";

        public const string DefaultCompassRose = CompassRose.Rose360Name;

        public const int DefaultTimeBetweenUpdates = 50;

        public const bool DefaultCollectErrors = true;

        public const string DefaultLanguage = "English";

        public const int Points360 = 360;
        public const int Points6000 = 6000;
        public const int Points6400 = 6400;

        /// <summary>
        /// Use this constant with compass data change to compare with previous value. If new value is below or equal to this value
        /// it will accept the change.
        /// </summary>
        public const double NotifyPropertyChangeDoubleAccurary = 0.001D;

        public static readonly CompassRose[] CompasRoses = new[]
            {
                new CompassRose {Name = CompassRose.Rose8Name, Points = 8},
                new CompassRose {Name = CompassRose.Rose360Name, Points = 12},
                new CompassRose {Name = CompassRose.Rose6000Name, Points = 12},
                new CompassRose {Name = CompassRose.Rose6400Name, Points = 16}
            };

        // TODO: Possible to put this into a text file: HotPink,#FFFFB6C1

        public static readonly NameValue[] Colors = new[]
            {
                new NameValue {Name = "HotPink", Value = "#FFFFB6C1"},
                new NameValue {Name = "Red", Value = "#FFFF0000"},
                new NameValue {Name = "Orange", Value = "#FFFFA500"},
                new NameValue {Name = "Yellow", Value = "#FFFFFF00"},
                new NameValue {Name = "Peru", Value = "#FFCD853F"},
                new NameValue {Name = "Lime", Value = "#FF00FF00"},
                new NameValue {Name = "Green", Value = "#FF008000"},
                new NameValue {Name = "Cyan", Value = "#FF00FFFF"},
                new NameValue {Name = "Blue", Value = "#FF0000FF"},
                new NameValue {Name = "Violet", Value = "#FFEE82EE"},
                new NameValue {Name = "White", Value = "#FFFFFFFF"},
                new NameValue {Name = "Gray", Value = "#FF808080"}
            };

        // TODO: Possible to put this into a text file: English,en-US

        public static readonly NameValue[] Languages = new[]
            {
                new NameValue {Name = "English", Value = "en-US"},
                new NameValue {Name = "Korean", Value = "ko-KR"},
                new NameValue {Name = "Slovak", Value = "sk-SK"},
                new NameValue {Name = "Japanese", Value = "ja-JP"},
                new NameValue {Name = "Russian", Value = "ru-RU"}
            };

        public static readonly string[] CompassTypes = new[] {"Trekking", "Plane", "Ship"};
    }
}