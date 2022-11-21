/*

See the 'Rbfi.Usage' text for more information

*/

namespace FlightDynamics
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // default database file name
            var fname = "database.txt";

            // parse command line args
            foreach(var p in args)
            {
                // is it a database arg?
                if(p.Substring(0, 2) == "-db")
                {
                    fname = p.Substring(2);
                }
                else
                {
                    Console.WriteLine($"Error: Invalid command line argument.\n\n{Rbfi.Usage}");
                }
            }

            // create the radial basis function interpolator
            var rbf = new Rbfi(fname);

            Console.WriteLine($"For each line of exactly {rbf.ConditionNames.Count} input condition scalars, this will output a line of results. Enter 'quit' to terminate.");

            // loop on console entries
            while (true)
            {
                // get a line of conditions
                var line = Console.ReadLine();

                if(line == "quit")
                {
                    break;
                }

                var conditionStrs = line.Split(",");

                // make sure their number matches the databas
                if(conditionStrs.Length != rbf.ConditionNames.Count)
                {
                    Console.WriteLine($"Error: specify exactly {rbf.ConditionNames.Count} conditions");
                    continue;
                }

                // convert them to list of doubles
                var conditions = new List<double>();
                foreach (var c in conditionStrs)
                {
                    conditions.Add(Convert.ToDouble(c));
                }

                // interpolate
                var results = rbf.Calc(conditions);

                // show results
                var msg = "";
                foreach(var r in results)
                {
                    msg += r.ToString() + " ";
                }

                Console.WriteLine(msg);
            }
        }

    }
}
