using System;

namespace Models
{

    [Serializable]
    public class Transaction
    {
        public int Id { get; set; }
        public string Sender { set; get; }
        public string Recipient { set; get; }
        public double Amount { set; get; }
        public double Fee { set; get; }


        public string toString()
        {
            string str = $"Transaction - Id:{this.Id}, Sender:{this.Sender}, Recipient:{this.Recipient}, Amount:{this.Amount}, Fee:{this.Fee}";
            return str;
        }
    }

}