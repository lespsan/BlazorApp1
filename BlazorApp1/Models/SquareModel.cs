namespace BlazorApp1.Models
{
    public class SquareModel
    {
        public bool isShown;

        public int Id { get; set; }

        public string Style { get; set; }

        public bool IsShown
        {
            get => isShown;
            set
            {
                isShown = value;
                if (isShown)
                {
                    Style = "mole";
                    Console.WriteLine($"Se cambió a mole {Id}");
                }
                else
                {
                    Style = "";
                }
            }
        }
    }
}
