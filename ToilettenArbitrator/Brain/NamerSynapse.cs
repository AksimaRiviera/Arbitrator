using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ToilettenArbitrator.Brain
{
    internal class NamerSynapse
    {
        private static FileStream stream;
        private static StreamReader reader;

        private static string _namesFile = "F:\\GitProjects\\Arbitrator\\ToilettenArbitrator\\Brain\\SubNames.txt";
        private static string _namesLine = string.Empty;

        private List<string> _names = new List<string>();
        public string OneName => GetName();

        static NamerSynapse()
        {
            stream = new FileStream(_namesFile, FileMode.Open);
            reader = new StreamReader(stream, encoding: Encoding.UTF8);

            _namesLine = reader.ReadToEnd();
            
            reader.Close();
            stream.Close();
        }

        public NamerSynapse()
        {
            stream = new FileStream(_namesFile, FileMode.Open);
            reader = new StreamReader(stream, encoding: Encoding.UTF8);

            _namesLine = reader.ReadToEnd();

            reader.Close();
            stream.Close();

            _names.AddRange(_namesLine.Split("|"));
        }

        private string GetName()
        {
            return _names[new Random().Next(_names.Count)];
        }
    }
}
