using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Microsoft.Spark.Sql;
using static Microsoft.Spark.Sql.Functions;
using System;
using System.Collections.Generic;

namespace TP_Sudoku_Spark
{
    class Program
    {
        static void Main(string[] args)
        {
            // Spark avec Benchmarks

            var summary = BenchmarkRunner.Run<SudokuBenchmarks>();

            // Spark sans Benchmarks
            //string filepath = "sudoku.csv";
            //SudokuSpark.runSpark(filepath, "1", "1", 1000);
        }


    }

    public class SudokuBenchmarks
    {
        string file_path ="sudoku.csv";

        public string cores= "1";
        [Params("1","5")]
        public string nodes { get; set; }
        [Params(800)]
        public int nrows { get; set; }

        [Benchmark]
        public void compute()
        {
            SudokuSpark.runSpark(file_path, cores, nodes, nrows);
           

        }

       





    }
}
