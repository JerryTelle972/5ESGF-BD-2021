using Microsoft.Spark.Sql;
using System;
using System.Collections.Generic;
using System.Text;

namespace TP_Sudoku_Spark
{
    class SudokuSpark
    {
        static int exec_count = 0;
        public static void runSpark(string file_path, string cores, string nodes, int nrows)
        {

            // Create Spark session 
            SparkSession spark =
                SparkSession
                    .Builder()
                    .AppName("word_count_sample")
                    .Config("spark.executor.cores", cores)
                    .Config("spark.executor.instances", nodes)
                    .GetOrCreate();

            // // Create initial DataFrame

            DataFrame dataFrame = spark
                .Read()
                .Option("header", true)
                .Option("inferSchema", true)
                .Schema("quizzes string, solutions string")
                .Csv(file_path);

            DataFrame dataFrame2 = dataFrame.Limit(nrows);

            spark.Udf().Register<string, string>(
                "SukoduUDF",
                (sudoku) => sudokusolution(sudoku));

            dataFrame2.CreateOrReplaceTempView("Resolved");
            DataFrame sqlDf = spark.Sql("SELECT quizzes, SukoduUDF(quizzes) as Resolution from Resolved");
            sqlDf.Show();

            spark.Stop();
            Console.WriteLine("SCRAPY");
        }

        static string sudokusolution(string grid)
        {
            exec_count++;
            Console.WriteLine("Nombre d'execution : " + exec_count);
            Sudoku sudoku = new Sudoku(grid);
            Console.WriteLine(grid);
            sudoku = sudoku.Solve(sudoku);
            sudoku.print_board();
            return sudoku.ToString();
        }

    }
}
