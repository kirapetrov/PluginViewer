using System.ComponentModel.Composition;
using System.Text;
using Caliburn.Micro;
using Common;

namespace MessengerPlugin.ViewModels
{
    //[Export(typeof(IPluginViewModel))]
    public class MessengerViewModel : Screen, IPluginViewModel
    {
        private string inputText;
        private readonly StringBuilder fullText;

        public string FullText => fullText.ToString();

        public string InputText
        {
            get => inputText;
            set
            {
                inputText = value;
                NotifyOfPropertyChange(nameof(CanAppendText));
            }
        }

        public bool CanAppendText => !string.IsNullOrWhiteSpace(inputText);

        public MessengerViewModel()
        {
            fullText = new StringBuilder();
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            AppendInitialText();
        }

        private void AppendInitialText()
        {
            fullText.AppendLine("У лукоморья дуб зелёный;");
            fullText.AppendLine("Златая цепь на дубе том:");
            fullText.AppendLine("И днём и ночью кот учёный");
            fullText.AppendLine("Всё ходит по цепи кругом;");
            fullText.AppendLine("Идёт направо -песнь заводит,");
            fullText.AppendLine("Налево - сказку говорит.");
            fullText.AppendLine("Там чудеса: там леший бродит,");
            fullText.AppendLine("Русалка на ветвях сидит;");
            fullText.AppendLine();
        }

        public void AppendText()
        {
            fullText.AppendLine(inputText);
            fullText.AppendLine();
            inputText = string.Empty;

            NotifyOfPropertyChange(nameof(FullText));
            NotifyOfPropertyChange(nameof(InputText));
            NotifyOfPropertyChange(nameof(CanAppendText));
        }
    }
}
