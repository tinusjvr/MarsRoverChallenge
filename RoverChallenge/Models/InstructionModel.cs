using RoverChallenge.Classes;

namespace RoverChallenge.Models
{
    public class InstructionModel : ObservableObject
    {
        public InstructionModel()
        {
            Input = new();
            Serviced = false;
        }

        public char Input { get; set; }
        public bool Serviced { get; set; }
    }
}
