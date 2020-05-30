using Models;
using Secp256k1Net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Main
{
    class Program
    {
        static void Main()
        {

           using (var secp256k1 = new Secp256k1())
            {

                // Generate a private key.
                var privateKey = new byte[32];
                var rnd = System.Security.Cryptography.RandomNumberGenerator.Create();
                do { rnd.GetBytes(privateKey); }
                while (!secp256k1.SecretKeyVerify(privateKey));


                // Create public key from private key.
                var publicKey = new byte[64];
                Debug.Assert(secp256k1.PublicKeyCreate(publicKey, privateKey));


                // Sign a message hash.
                var messageBytes = Encoding.UTF8.GetBytes("Hello world.");
                var messageHash = System.Security.Cryptography.SHA256.Create().ComputeHash(messageBytes);
                var signature = new byte[64];
                Debug.Assert(secp256k1.Sign(signature, messageHash, privateKey));

                // Serialize a DER signature from ECDSA signature
                byte[] signatureDer = new byte[Secp256k1.SERIALIZED_DER_SIGNATURE_MAX_SIZE];
                int outL = 0;
                Debug.Assert(secp256k1.SignatureSerializeDer(signatureDer, signature, out outL));
                Array.Resize(ref signatureDer, outL);

                // Verify message hash.
                Debug.Assert(secp256k1.Verify(signature, messageHash, publicKey));

            }
            
            // Make blockchain
            var bc = new Blockchain();


            Guid g = System.Guid.NewGuid();
            Console.WriteLine("=== Trasaction Id: {0}", g.ToString());

            Menu();


            //Create new transaction
            var trx1 = new Transaction
            {
                Sender = "Genesis Account",
                Recipient = "Ricardo",
                Amount = 300,
                Fee = 0.001
            };

            //Create new transaction
            var trx2 = new Transaction
            {
                Sender = "Genesis Account",
                Recipient = "Frodo",
                Amount = 250,
                Fee = 0.001
            };

            //Create new transaction
            var trx3 = new Transaction
            {
                Sender = "Ricardo",
                Recipient = "Madona",
                Amount = 20,
                Fee = 0.0001
            };

            //create list of transactions
            var lsTrx = new List<Transaction>
            {
                trx1,
                trx2
            };

            var transactions = lsTrx.ToArray();
            bc.AddBlock(transactions);

            lsTrx = new List<Transaction>
            {
                trx3         
            };

            transactions = lsTrx.ToArray();
            bc.AddBlock(transactions);

            //Print all blocks
            bc.PrintBlocks();

            //check balance for each account account
            var balance = bc.GetBalance("Genesis Account");
            Console.WriteLine("Genesis Account balance: {0}", balance);

            balance = bc.GetBalance("Ricardo");
            Console.WriteLine("Ricardo balance: {0}", balance);

            balance = bc.GetBalance("Frodo");
            Console.WriteLine("Frodo  balance: {0}", balance);


            balance = bc.GetBalance("Madona");
            Console.WriteLine("Madona balance: {0}", balance);

            Console.ReadKey();
        }

        private static void MenuItem(){
            Console.WriteLine("=========================");
            Console.WriteLine("1. Create Wallet");
            Console.WriteLine("2. Add a transaction");
            Console.WriteLine("3. Get Balance");
            Console.WriteLine("4. Get Genesis Block");
            Console.WriteLine("5. Get Last Block");
            Console.WriteLine("6. Print Blockchain");
            Console.WriteLine("9. Exit");
            Console.WriteLine("=========================");
        }

        private static void Menu(){
     
            MenuItem();

            int selection = 0;
            while (selection != 100)
            {
                switch (selection)
                {
                    case 1:
                        Console.WriteLine("Get Gebesis Block: ");
                                            
                        MenuItem();                    
                        break;
                    case 2:
                        Console.WriteLine("GetLast Block");
                       
                        MenuItem();
                        break;    

                    case 3:
                        Console.WriteLine("Add Transaction ");
                        
                        MenuItem();
                        break;
                    case 4:
                        Console.WriteLine("Print Blockchain");

                        MenuItem();
                        break;

                    case 5:
                        Console.WriteLine("Get Balance, enter address:");
                        
                        MenuItem();
                        break;
                    case 6:

                        Console.WriteLine("=== Create Balance ");
                        MenuItem();

                        break;

                    default:
                        Console.WriteLine(" Select menu, Wrong code");
                        MenuItem();
                        break;    
                }

                Console.WriteLine("Please select an action");
                string action = Console.ReadLine();
                selection = int.Parse(action);
            }
        }

    }



}