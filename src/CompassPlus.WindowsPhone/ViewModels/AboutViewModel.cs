using CompassPlus.Globalization;
using System;
using System.Reflection;


namespace CompassPlus.ViewModels
{
    public class AboutViewModel
    {
        public Version Version
        {
            get { return new AssemblyName(typeof(AboutViewModel).Assembly.FullName).Version; }
        }

        public string VersionString
        {
            get { return string.Format("{0} : {1}", new LocalizedStrings()["Version"], Version); }
        }

        public string Copyright
        {
            get { return string.Format(new LocalizedStrings()["CopyrightShort"], DateTime.Now.Year); }
        }
    }
}