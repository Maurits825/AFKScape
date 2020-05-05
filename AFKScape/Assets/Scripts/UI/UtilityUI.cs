using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class UtilityUI
{
    public static (string, Color) FormatNumber(BigInteger num)
    {
        BigInteger numPrefix;
        if (num < 100_000)
        {
            return (num.ToString(), Color.yellow);
        }
        else if (num >= 100_000 && num < 10_000_000)
        {
            numPrefix = BigInteger.Divide(num, 1_000);
            return (numPrefix.ToString() + "K", Color.white);
        }
        else if (num >= 10_000_000 && num < 1_000_000_000)
        {
            numPrefix = BigInteger.Divide(num, 1_000_000);
            return (numPrefix.ToString() + "M", Color.green);
        }
        else if (num >= 1_000_000_000 && num < 1_000_000_000_000)
        {
            numPrefix = BigInteger.Divide(num, 1_000_000_000);
            return (numPrefix.ToString() + "B", Color.blue);
        }
        else
        {
            numPrefix = BigInteger.Divide(num, 1_000_000_000_000);
            return (numPrefix.ToString() + "T", Color.red);
        }
    }
}
