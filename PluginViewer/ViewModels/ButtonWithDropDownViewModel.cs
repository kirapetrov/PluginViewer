using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Runtime.Remoting.Messaging;
using Caliburn.Micro;

namespace PluginViewer.ViewModels
{
    [Export]
    public class ButtonWithDropDownViewModel : Screen
    {
        public ButtonWithDropDownViewModel()
        {
            Number = "11223344";

            Properties = new ObservableCollection<NameValueType>
            {
                new NameValueType("Short name", "Value"),
                new NameValueType("Very very very very very very very long name", "Value"),
                new NameValueType("Long name", "Very very very very very very very long value")
            };
        }

        public string Number { get; }

        public ObservableCollection<NameValueType> Properties { get; }
    }

    public class NameValueType
    {
        public string Header { get; }
        public string Value { get; }

        public NameValueType(
            string header, string value)
        {
            Header = header;
            Value = value;
        }
    }
}
