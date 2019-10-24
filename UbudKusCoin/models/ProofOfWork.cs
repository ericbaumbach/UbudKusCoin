using System;
using System.Diagnostics;
using Utils;

namespace Models{
 
    public class ProofOfWork{
            
            public const int difficulty = 4;
            Block block {set; get;}
            Int64 target {set; get;}        
    
            public ProofOfWork(Block block){
                    this.block = block;
            }


            public PoWResult Run(string inputString)
            {
                var result = new PoWResult();
                string expectedHash = new string('0', difficulty);//if the difficulty=4 the expectedHash will be='0000'
                var stopwatch = new Stopwatch();
                stopwatch.Start(); // doing time measurement
           
                do
                {
                    // counting the number of iterations
                    result.Nonce++; 
                    // generating random string to test it
                    result.SolveString = inputString + result.Nonce; 
                    // generating sha-256 from the random string
                    result.SolveHash = Sha256Generator.GenerateSha256String(result.SolveString);
                    // checking if we found a solution
                    Console.WriteLine("=== Solved hash {0}", result.SolveHash);
                 

                } while (!result.SolveHash.StartsWith(expectedHash)); 
               
                result.SolveSeconds = stopwatch.Elapsed.TotalSeconds;
               
                return result;
            }
    
    }

}