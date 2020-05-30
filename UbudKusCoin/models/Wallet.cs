using System;
using NBitcoin;
namespace Models
{
   

    public class Wallet
    {
        public string address{set; get;}
        public decimal balance {set; get;}
        public Key privateKey {set; get;}
        public PubKey publicKey {set; get;}
    
        public Wallet(string secreet)
        {
            BitcoinSecret bscreet = new BitcoinSecret(secreet);
            this.privateKey = bscreet.PrivateKey;
            this.publicKey = this.privateKey.PubKey;
        }


    }

}   