// using System;
// using System.Security.Cryptography;
// using Utils;

// namespace Models
// {
//     [Serializable]
//     public class Block
//     {
//         public int Height { get; set; }
//         public Int64 TimeStamp { get; set; }
//         public byte[] PrevHash { get; set; } 
//         public byte[] Hash { get; set; }
//         public Transaction[] Transactions { get; set; }
//         public string Creator { get; set; }

//         public Block(int height, byte[] prevHash, Transaction[] transactions, string creator)
//         {
//             this.Height = height;
//             this.PrevHash = prevHash;
//             this.TimeStamp = DateTime.Now.Ticks;
//             this.Transactions = transactions;
//             this.Hash = GenerateHash();
//             this.Creator = creator;
//         }

//         public byte[] GenerateHash()
//         {
//             var sha = SHA256.Create();
//             byte[] timeStamp = BitConverter.GetBytes(TimeStamp);

//             var transactionHash = Transactions.ConvertToByte();

//             byte[] headerBytes = new byte[timeStamp.Length + PrevHash.Length + transactionHash.Length];

//             Buffer.BlockCopy(timeStamp, 0, headerBytes, 0, timeStamp.Length);
//             Buffer.BlockCopy(PrevHash, 0, headerBytes, timeStamp.Length, PrevHash.Length);
//             Buffer.BlockCopy(transactionHash, 0, headerBytes, timeStamp.Length + PrevHash.Length, transactionHash.Length);

//             byte[] hash = sha.ComputeHash(headerBytes);

//             return hash;

//         }


//     }
// }

using System;
using System.Security.Cryptography;
using Utils;

namespace Models
{
    [Serializable]
    public class Block
    {
        public int Height { get; set; }
        public Int64 TimeStamp { get; set; }
        public byte[] PrevHash { get; set; }
        public byte[] Hash { get; set; }
        // public byte[] Transactions { get; set; }
         public Transaction[] Transactions { get; set; }
        public int Nonce { set; get; }
        public string Creator { get; set; }
        // public Block(byte[] prevHash, byte[] transactions)
        // {
        //     this.PrevHash = prevHash;
        //     this.TimeStamp = DateTime.Now.Ticks;
        //     this.Transactions = transactions;
        //     this.Hash = GenerateHash();
        // }

        public Block(int height, byte[] prevHash, Transaction[] transactions, string creator)
        {
            this.Height = height;
            this.PrevHash = prevHash;
            this.TimeStamp = DateTime.Now.Ticks;
            this.Transactions = transactions;
            this.Hash = GenerateHash();
            this.Creator = creator;
        }

        public byte[] GenerateHash()
        {
            var sha = SHA256.Create();
            byte[] timeStamp = BitConverter.GetBytes(TimeStamp);

            var transactionHash = Transactions.ConvertToByte();

            byte[] headerBytes = new byte[timeStamp.Length + PrevHash.Length + transactionHash.Length];

            Buffer.BlockCopy(timeStamp, 0, headerBytes, 0, timeStamp.Length);
            Buffer.BlockCopy(PrevHash, 0, headerBytes, timeStamp.Length, PrevHash.Length);
            Buffer.BlockCopy(transactionHash, 0, headerBytes, timeStamp.Length + PrevHash.Length, transactionHash.Length);

            byte[] hash = sha.ComputeHash(headerBytes);

            return hash;

        }

        // public byte[] GenerateHash()
        // {
        //     SHA256 sha256 = SHA256.Create();
        //     byte[] tStamp = BitConverter.GetBytes(this.TimeStamp);
        //     byte[] sumBytes = new byte[tStamp.Length + this.PrevHash.Length + this.Transactions.Length];


        //     Buffer.BlockCopy(tStamp, 0, sumBytes, 0, tStamp.Length);
        //     Buffer.BlockCopy(PrevHash, 0, sumBytes, tStamp.Length, PrevHash.Length);
        //     Buffer.BlockCopy(Transactions, 0, sumBytes, tStamp.Length + PrevHash.Length, Transactions.Length);

        //     byte[] hashBytes = sha256.ComputeHash(sumBytes);

        //     return hashBytes;

        // }


    }
}
