using System.Collections.Generic;
using System.Text;
using System;
using Utils;
using LiteDB;

namespace Models
{
    public class Blockchain
    {

 
        private byte[] LastHash;
      
        public LiteDatabase Db { set; get; }

        public Blockchain()
        {
            InitDb ();

            InitializeChain();
        }

        public void InitDb() {
            // Open database (or create if not exits)
            using (var db = new LiteDatabase(@"Node1a.db"))
            {
                this.Db = db;
            }
        }


        public void CloseDb()
        {
            this.Db.Dispose();                
        }


        private LiteCollection<BsonDocument> GetBlockchain()
        {
            var blockchain = this.Db.GetCollection(Constants.CHAIN);
            return blockchain;
        }

        private LiteCollection<BsonDocument> GetBlockchainState()
        {
            var bcstate = this.Db.GetCollection(Constants.STATE);
            return bcstate;
        }

        public Block GetLastBlock() {
            // Get blockhash collection
            var blockchain = this.GetBlockchain();

            // Get blockhash collection
            var lastHash = this.GetLastHash();
            var block = blockchain.FindOne(Query.EQ(Constants.KEY,lastHash));
            return (Block)block.AsBinary.DoDeSerialize();
        }

        public void InitializeChain()
        {

            // Open database (or create if not exits)
            using (this.Db)
            {
              
                // Get blockhash collection
                var blockchain = this.GetBlockchain();

                if (blockchain != null && blockchain.Count() < 1)
                {

                    // Create genesis block
                    var genesisblock = CreateGenesisBlock();
                    blockchain.Insert(new BsonDocument
                    {
                        { Constants.KEY, genesisblock.Hash },
                        { Constants.VALUE, genesisblock.DoSerialize() }
                    });

                    // Create, if not exists, new index on Key field
                    blockchain.EnsureIndex(Constants.KEY);

                    //Insert Last Hash to meta Info
                    this.AddOrUpdateLastHash(genesisblock.Hash);

                    //asign lash hash with genesis block hash
                    LastHash = genesisblock.Hash;

                }
                else
                {
                    this.LastHash = this.GetLastHash();
                }

            }

        }


     //Get LashHash in Blockchain
        public byte[] GetLastHash()
        {

            var bcState = this.GetBlockchainState();
            var blockHash = bcState.FindOne(Query.EQ(Constants.KEY, Constants.LastHashKey));

            if (null != blockHash)
            {
                // {"$binary":"0XrHFkXUlCDiwqgfEif/5+abtGhztZSiw2aiPiPtBPk="}
                return blockHash[Constants.VALUE];
            }
            else
            {
                return null;
            }

        }

        //Update LastHash in Blockchain
        public void AddOrUpdateLastHash(byte[] arg)
        {
            var bcState = this.GetBlockchainState();
            if (bcState != null && bcState.Count() > 0)
            {
                Console.WriteLine("--- Enter Here {0}", bcState.Count());
                var state = bcState.FindOne(Query.EQ(Constants.KEY, Constants.LastHashKey));
                if (null != state) {
                    Console.WriteLine("sate value {0}", state[Constants.VALUE]);
                    state[Constants.VALUE] = arg;
                    bcState.Update(state);
                }

            }
            else
            {
                Console.WriteLine("--- Create New {0}", arg);

                // Insert lastHash to blockchain
                bcState.Insert(new BsonDocument
                {
                    { Constants.KEY, Constants.LastHashKey },
                    { Constants.VALUE, arg }
                });

            }
        }


        private Block CreateGenesisBlock()
        {
            var lst = new List<Transaction>();
            var trx = new Transaction
            {
                Amount = 1000,
                Sender = "Founder",
                Recipient = "Genesis Account",
                Fee = 0.0001
            };
            lst.Add(trx);

            var trxByte = lst.ToArray().ConvertToByte();
            return new Block(1, String.Empty.ConvertToBytes(), lst.ToArray(), "Admin");
        }


        // Add new Block to blockchain 
        public void AddBlock(Transaction[] transactions)
        {

            var lastBlock = GetLastBlock();
            var nextHeight = lastBlock.Height + 1;
         
            // var lastHash = this.GetLastHash();
            var prevHash = lastBlock.Hash;
            var timestamp = DateTime.Now.Ticks;
            
            var block = new Block(nextHeight, prevHash, transactions, "Admin");
          
         
            var pow = new ProofOfWork(block);
            PoWResult result = pow.Run("Secreet");

            block.Hash = result.SolveHash.DoSerialize();
            block.Nonce = result.Nonce;


            var blockchain = this.GetBlockchain();
            blockchain.Insert(new BsonDocument
            {
                { Constants.KEY, block.Hash },
                { Constants.VALUE, block.DoSerialize() }
            });


            //update lastHash
            this.AddOrUpdateLastHash(block.Hash);

            // update LastHash
            this.LastHash = block.Hash;

        }

        // public int GetHeight()
        // {
        //     var lastBlock = this.Blocks[this.Blocks.Count - 1];
        //     return lastBlock.Height;
        // }

        // public bool IsBlocksValid(List<Block> blocks)
        // {
        //     if (blocks[0].Height != 1 && blocks[0].PrevHash.Equals(String.Empty.ConvertToBytes()))
        //     {
        //         return false;
        //     }

        //     for (int i = 0; i < blocks.Count; i++)
        //     {
        //         Block block = blocks[i];
        //         Block prevBlock = blocks[i - 1];

        //         if (block.PrevHash != prevBlock.Hash)
        //         {
        //             return false;
        //         }

        //         if (block.Hash != block.GenerateHash())
        //         {
        //             return false;
        //         }


        //     }
        //     return true;
        // }

        // public void ReplaceBlocks(List<Block> receivedBlocks)
        // {
        //     if (receivedBlocks.Count <= this.Blocks.Count)
        //     {
        //         return;
        //     }
        //     else if (!this.IsBlocksValid(receivedBlocks))
        //     {
        //         return;
        //     }

        //     this.Blocks = receivedBlocks;
        // }

        // public double GetBalance(string name) {

        //     double balance = 0;
        //     double spending = 0;
        //     double income = 0;

        //     foreach (Block block in this.Blocks)
        //     {
        //         var transactions = block.Transactions;
              
        //         foreach (Transaction transaction in transactions)
        //         {

        //             var sender = transaction.Sender;
        //             var recipient = transaction.Recipient;

        //             if (name.ToLower().Equals(sender.ToLower())) {
        //                 spending += transaction.Amount + transaction.Fee;                    
        //             }


        //             if (name.ToLower().Equals(recipient.ToLower())) {
        //                 income += transaction.Amount;
        //             }

        //             balance = income - spending;
        //         }
        //     }
        //     return balance;
        // }

        // public void PrintBlocks() {

        //     var sbf = new StringBuilder();

        //     foreach (Block block in this.Blocks)
        //     {
        //         Console.WriteLine("Height:      {0}", block.Height);
        //         Console.WriteLine("Timestamp:   {0}", block.TimeStamp.ConvertToDateTime());
        //         Console.WriteLine("Prev. Hash:  {0}", block.PrevHash.ConvertToHexString());
        //         Console.WriteLine("Hash:        {0}", block.Hash.ConvertToHexString());
        //         Console.WriteLine("Transactins: {0}", block.Transactions.ConvertToString());
        //         Console.WriteLine("Creator:     {0}", block.Creator);
        //         Console.WriteLine("--------------\n");

        //     }

        //     Console.WriteLine(sbf);


        // }
    }
}