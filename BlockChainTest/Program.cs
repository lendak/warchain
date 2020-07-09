using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlockChainLib;
using ExcelDataReader;
using System.IO;
using System.Numerics;
using System.Threading;

namespace BlockChainTest
{

    class Program
    {
        static uint NUMBER_OF_VALIDATORS = 1;
        static uint NUMBER_OF_TRANSACTIONS_IN_BLOCK = 20000;
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            // simple unit test
            System.Collections.Generic.List<BlockChainLib.ProofOfStake.Validator> vals = new System.Collections.Generic.List<BlockChainLib.ProofOfStake.Validator>();
            BlockChainLib.ProofOfStake.Validator validator = null;
            for (uint i = 0; i < Program.NUMBER_OF_VALIDATORS; i++)
            {
                validator = new BlockChainLib.ProofOfStake.Validator();
                vals.Add(validator);

                Console.WriteLine("New validator with GUID = ({0})", validator.Identifier);
            }

            // TODO: put this back once implemented correctly
            /*
            for (int i=0; i<vals.Count; i++)
            {
                vals[i].AddPeers(vals);
            }*/

            if (vals.Count > 0) // TODO: configure which experiment to execute
            {
                BlockChainLib.ProofOfStake.Validator val0 = vals[0];
                Experiment1_ProofOfConcept(val0);
            }

            if (vals.Count > 1)
            {
                BlockChainLib.ProofOfStake.Validator val1 = vals[1];
                Experiment2_DocumentAlterations(val1);
            }


            if (vals.Count >= 4)
            {
                BlockChainLib.ProofOfStake.Validator val2 = vals[2];
                BlockChainLib.ProofOfStake.Validator val3 = vals[3];

                val2.AddPeer(val3);
                val3.AddPeer(val2);

                Experiment3_DuplicateManagement(val2, val3, 2, 3); // sleep 1 & 2 are in milliseconds for Thread.Sleep
            }

            /*BlockChainLib.ProofOfStake.Validator val1 = new BlockChainLib.ProofOfStake.Validator();
            System.Threading.Thread.Sleep(750);
            BlockChainLib.ProofOfStake.Validator val2 = new BlockChainLib.ProofOfStake.Validator();
            System.Threading.Thread.Sleep(750);
            BlockChainLib.ProofOfStake.Validator val3 = new BlockChainLib.ProofOfStake.Validator();
            System.Threading.Thread.Sleep(750);
            BlockChainLib.ProofOfStake.Validator val4 = new BlockChainLib.ProofOfStake.Validator();
            val1.AddPeer(val2);
            val1.AddPeer(val3);
            val1.AddPeer(val4);
            val2.AddPeer(val1);
            val3.AddPeer(val1);
            val4.AddPeer(val1); // intentionally do not add every node as every other node's peer*/

            Console.WriteLine("Press [Enter] to exit.");
            Console.ReadLine();

            /*Block bl1 = new Block(System.Guid.NewGuid(), val1, DateTime.Now, 0, "", 0, val1.GetLastBlock());
            Console.WriteLine("Block JSON: " + bl1.ToJson());

            for (uint i = 0; i < 25; i++)
            {
                val1.AddTransaction(new Transaction(val1, "", DateTime.Now));
            }*/
        }

        static void Experiment1_ProofOfConcept(BlockChainLib.ProofOfStake.Validator val0)
        {
            Program.PopulateSingleValidatorFromExcel(val0, @"c:\007 data\2020-02 Digital Humanities\2020-06-25_Origo_hashes_only.xlsx");
        }

        static void Experiment2_DocumentAlterations(BlockChainLib.ProofOfStake.Validator val0)
        {
            Program.PopulateSingleValidatorFromExcel(val0, @"c:\007 data\2020-02 Digital Humanities\2020-06-25_Origo_hashes_only.xlsx");

            Console.WriteLine("Modified documents after 467k docs [count]: {0}", val0.ModifiedDocs);
            Console.WriteLine("Duplicate documents after 467k docs  [count]: {0}", val0.DuplicateDocs);

            Program.PopulateSingleValidatorFromExcel(val0, @"c:\007 data\2020-02 Digital Humanities\2020-06-26_Origo_new_hashes_only.xlsx");

            Console.WriteLine("Modified documents after +592k docs [count]: {0}", val0.ModifiedDocs);
            Console.WriteLine("Duplicate documents after +592k docs  [count]: {0}", val0.DuplicateDocs);
        }

        /// <summary>
        /// Start two threads which populate two different validator nodes, i.e. send transactions to them
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <param name="sleep1"></param>
        /// <param name="sleep2"></param>
        static void Experiment3_DuplicateManagement(BlockChainLib.ProofOfStake.Validator val1, BlockChainLib.ProofOfStake.Validator val2, int sleep1, int sleep2)
        {
            Thread threadOrigo = new Thread(
                unused => Program.PopulateSingleValidatorFromExcel(val1, @"c:\007 data\2020-02 Digital Humanities\2020-06-25_Origo_hashes_only.xlsx", sleep1));
            threadOrigo.Start();

            Thread threadOrigoNew = new Thread(
                unused => Program.PopulateSingleValidatorFromExcel(val2, @"c:\007 data\2020-02 Digital Humanities\2020-06-26_Origo_new_hashes_only.xlsx", sleep2));
            threadOrigoNew.Start();

            threadOrigo.Join();
            threadOrigoNew.Join();
        }

        static void PopulateSingleValidatorFromExcel(BlockChainLib.ProofOfStake.Validator validator, string pathToExcel, int sleepMiliseconds = 0)
        {
            Console.WriteLine(DateTime.Now.ToString() + " - Populate single validator - Start.");
            using (var stream = File.Open(pathToExcel, FileMode.Open, FileAccess.Read))
            {
                // Auto-detect format, supports:
                //  - Binary Excel files (2.0-2003 format; *.xls)
                //  - OpenXml Excel files (2007 format; *.xlsx)
                /*ExcelReaderConfiguration erc = new ExcelReaderConfiguration()
                {
                    // Gets or sets the encoding to use when the input XLS lacks a CodePage
                    // record, or when the input CSV lacks a BOM and does not parse as UTF8. 
                    // Default: cp1252 (XLS BIFF2-5 and CSV only)
                    FallbackEncoding = Encoding.GetEncoding(1252),


                    // Gets or sets an array of CSV separator candidates. The reader 
                    // autodetects which best fits the input data. Default: , ; TAB | # 
                    // (CSV only)
                    AutodetectSeparators = new char[] { ',', ';', '\t', '|', '#' },

                    // Gets or sets a value indicating whether to leave the stream open after
                    // the IExcelDataReader object is disposed. Default: false
                    LeaveOpen = false,

                    // Gets or sets a value indicating the number of rows to analyze for
                    // encoding, separator and field count in a CSV. When set, this option
                    // causes the IExcelDataReader.RowCount property to throw an exception.
                    // Default: 0 - analyzes the entire file (CSV only, has no effect on other
                    // formats)
                    AnalyzeInitialCsvRows = 0,
                };*/

                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    uint counter = 0;

                    do
                    {
                        while (reader.Read())
                        {
                            if (counter == 0) continue; // skip first row

                            if ((counter > 0) && (counter % NUMBER_OF_TRANSACTIONS_IN_BLOCK == 0))
                            {
                                validator.FinalizeBlock(); // create blocks of 10000 document validations
                                //Console.WriteLine("Block finalized at article count {0}", counter);
                            }

                            DateTime dtTransactionDate = DateTime.Now;
                            //FormattableString message = $"{{ \"url_hash\": { reader.GetString(1)}, \"article_hash\": {reader.GetString(2)}, \"transaction_date\": { dtTransactionDate.ToString() }";
                            //Transaction tr = new Transaction(validator, message.ToString(), dtTransactionDate);

                            BlockChainLib.WARChain.WARCTransaction tr = new BlockChainLib.WARChain.WARCTransaction(validator, "", dtTransactionDate, BigInteger.Parse(reader.GetString(1), System.Globalization.NumberStyles.HexNumber), BigInteger.Parse(reader.GetString(2), System.Globalization.NumberStyles.HexNumber));

                            //Console.WriteLine("Hashes for (URL, article) = ({0}, {1})", reader.GetString(1), reader.GetString(2));
                            //Console.WriteLine(message.ToString());
                            validator.AddTransaction(tr);

                            if (sleepMiliseconds > 0) Thread.Sleep(sleepMiliseconds);

                            counter++;
                        }
                    } while (reader.NextResult());

                    // The result of each spreadsheet is in result.Tables
                }

                Console.WriteLine(DateTime.Now.ToString() + " - Populate single validator - End.");
            }
        }

        /*static void ValidateFromExcel(BlockChainLib.ProofOfStake.Validator validator, string pathTo2ndExcel)
        {
            Console.WriteLine(DateTime.Now.ToString() + " - Validate 2nd Excel - Start.");
            using (var stream = File.Open(pathTo2ndExcel, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    uint counter = 0;

                    do
                    {
                        while (reader.Read())
                        {
                            counter++;
                        }
                    } while (reader.NextResult());
                }
            }
            Console.WriteLine(DateTime.Now.ToString() + " - Validate 2nd Excel - End.");
        }*/
    }
}
