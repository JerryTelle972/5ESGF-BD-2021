﻿using Microsoft.Spark.Sql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TP_Sudoku_Spark
{
    public class Sudoku : ISudokuSolver
    {
        static string rows = "ABCDEFGHI";
        static string cols = "123456789";
        static string digits = "123456789";
        static string[] squares = cross(rows, cols);
        static Dictionary<string, IEnumerable<string>> peers;
        static Dictionary<string, IGrouping<string, string[]>> units;
        private string grid;
        private Dictionary<string, string> values;


        static string[] cross(string A, string B)
        {
            return (from a in A from b in B select "" + a + b).ToArray();
        }

        public Sudoku(string grid)
        {
            //this.grid = from c in grid where "0.-123456789".Contains(c) select c;
            this.grid = grid;

            var unitlist = ((from c in cols select cross(rows, c.ToString()))
                               .Concat(from r in rows select cross(r.ToString(), cols))
                               .Concat(from rs in (new[] { "ABC", "DEF", "GHI" }) from cs in (new[] { "123", "456", "789" }) select cross(rs, cs)));

           
            units = (from s in squares from u in unitlist where u.Contains(s) group u by s into g select g).ToDictionary(g => g.Key);

            
            peers = (from s in squares from u in units[s] from s2 in u where s2 != s group s2 by s into g select g).ToDictionary(g => g.Key, g => g.Distinct());

        }



        public Sudoku Solve(Sudoku s)
        {
            s.parse_grid();
            return s;
        }
        public override string ToString()
        {
            return string.Join("", values.Select(kv => kv.Value).ToArray());
        }

        string[][] zip(string[] A, string[] B)
        {
            var n = Math.Min(A.Length, B.Length);
            string[][] sd = new string[n][];
            for (var i = 0; i < n; i++)
            {
                sd[i] = new string[] { A[i].ToString(), B[i].ToString() };
            }
            return sd;
        }
      
        /// <summary>Given a string of 81 digits (or . or 0 or -), return a dict of {cell:values}</summary>
        public void parse_grid()
        {
            var grid2 = from c in this.grid where "0.-123456789".Contains(c) select c;
            values = squares.ToDictionary(s => s, s => digits); //To start, every square can be any digit

            foreach (var sd in zip(squares, (from s in this.grid select s.ToString()).ToArray()))
            {
                var s = sd[0];
                var d = sd[1];

                if (digits.Contains(d) && assign(values, s, d) == null)
                {
                    return;
                }
            }
        }

    
        /// <summary>Using depth-first search and propagation, try all possible values.</summary>
        public Dictionary<string, string> search(Dictionary<string, string> values)
        {
            if (values == null)
            {
                return null; // Failed earlier
            }
            if (all(from s in squares select values[s].Length == 1 ? "" : null))
            {
                return values; // Solved!
            }

            // Chose the unfilled square s with the fewest possibilities
            var s2 = (from s in squares where values[s].Length > 1 orderby values[s].Length ascending select s).First();

            return some(from d in values[s2]
                        select search(assign(new Dictionary<string, string>(values), s2, d.ToString())));
        }
   
     
        /// <summary>Eliminate all the other values (except d) from values[s] and propagate.</summary>
        Dictionary<string, string> assign(Dictionary<string, string> values, string s, string d)
        {
            if (all(
                    from d2 in values[s]
                    where d2.ToString() != d
                    select eliminate(values, s, d2.ToString())))
            {
                return values;
            }
            return null;
        }

        // Eliminate d from values[s]; propagate when values or places <= 2.
     
        /// <summary>Eliminate d from values[s]; propagate when values or places &lt;= 2.</summary>
        Dictionary<string, string> eliminate(Dictionary<string, string> values, string s, string d)
        {
            if (!values[s].Contains(d))
            {
                return values;
            }
            values[s] = values[s].Replace(d, "");
            if (values[s].Length == 0)
            {
                return null; //Contradiction: removed last value
            }
            else if (values[s].Length == 1)
            {
                //If there is only one value (d2) left in square, remove it from peers
                var d2 = values[s];
                if (!all(from s2 in peers[s] select eliminate(values, s2, d2)))
                {
                    return null;
                }
            }

            //Now check the places where d appears in the units of s
            foreach (var u in units[s])
            {
                var dplaces = from s2 in u where values[s2].Contains(d) select s2;
                if (dplaces.Count() == 0)
                {
                    return null;
                }
                else if (dplaces.Count() == 1)
                {
                    // d can only be in one place in unit; assign it there
                    if (assign(values, dplaces.First(), d) == null)
                    {
                        return null;
                    }
                }
            }
            return values;
        }

       
        bool all<T>(IEnumerable<T> seq)
        {
            foreach (var e in seq)
            {
                if (e == null) return false;
            }
            return true;
        }

      
        T some<T>(IEnumerable<T> seq)
        {
            foreach (var e in seq)
            {
                if (e != null) return e;
            }
            return default(T);
        }
      
        string Center(string s, int width)
        {
            var n = width - s.Length;
            if (n <= 0) return s;
            var half = n / 2;

            if (n % 2 > 0 && width % 2 > 0) half++;

            return new string(' ', half) + s + new String(' ', n - half);
        }
      
        /// <summary>Used for debugging.</summary>
        public void print_board()
        {
            if (values == null) return;

            var width = 1 + (from s in squares select values[s].Length).Max();
            var line = "\n" + String.Join("+", Enumerable.Repeat(new String('-', width * 3), 3).ToArray());

            foreach (var r in rows)
            {
                Console.WriteLine(String.Join("",
                    (from c in cols
                     select Center(values["" + r + c], width) + ("36".Contains(c) ? "|" : "")).ToArray())
                        + ("CF".Contains(r) ? line : ""));
            }

            Console.WriteLine();
        }
    }




}

