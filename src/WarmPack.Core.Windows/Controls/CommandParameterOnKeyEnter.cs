using System.Windows.Input;

namespace WarmPack.Windows.Controls
{
    public class CommandParameterOnKeyDown
    {
        public bool Handled { get; set; }
        public object CommandParameter { get; set; }
        public Key Key { get; set; }
    }
}
