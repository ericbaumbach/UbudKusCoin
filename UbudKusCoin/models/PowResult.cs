namespace Models {
    public class PoWResult
    {
        // Contain result string 
        public string SolveString { get; set; }
    
        // Contain the number of seconds was spend to solve the challenge
        public double SolveSeconds { get; set; }
    
        // Contain hash of 'SolveString'
        public string SolveHash { get; set; }
    
        // Contain the number of iterations done to solve the challenge
        public int Nonce { get; set; }
    }
}