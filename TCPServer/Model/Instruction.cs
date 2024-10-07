using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer.Model
{
    public class Instruction
    {
        public string InstructionMessage {  get; set; }

        public Instruction() 
        {
           InstructionMessage = "Choose method: random (highest number last), add, or subtract (highest number first), then choose two numbers, e.g., random 1 30, subtract 2 1.";
        }
    }
}
