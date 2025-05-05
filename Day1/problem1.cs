using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Text;
using System;

class Result
{



    public static void plusMinus(List<int> arr)
    {
         double pos =0;
         double neg = 0;
         double zero =0;
         
         foreach(int num in arr)
         {
            if(num>0)
            {
                pos+=1;
            }
            else if(num<0)
            {
                neg+=1;
            }
            else
            zero+=1;
         }
         double len =arr.Count;
         double posratio=pos/len;
         double negratio=neg/len;
         double zeroratio=zero/len;
         Console.WriteLine(posratio.ToString("F6"));
         Console.WriteLine(negratio.ToString("F6"));
         Console.WriteLine(zeroratio.ToString("F6"));
    }

}

class Solution
{
    public static void Main(string[] args)
    {
        int n = Convert.ToInt32(Console.ReadLine().Trim());

        List<int> arr = Console.ReadLine().TrimEnd().Split(' ').ToList().Select(arrTemp => Convert.ToInt32(arrTemp)).ToList();

        Result.plusMinus(arr);
    }
}
