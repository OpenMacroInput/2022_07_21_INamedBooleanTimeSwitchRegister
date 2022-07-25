using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DateTimeTickUtility
{

    public static readonly byte tickValueInNanoseconds = 100;
    public static readonly int ratioTickInMilliseconds = 10000;
    public static readonly int ratioTickInSeconds = 10000000;


    public static void TickToNanoseconds(in long tick, out long nanoseconds)
        => nanoseconds = tick * tickValueInNanoseconds;
    public static void TickToMilliseconds(in long tick, out long milliseconds)
        => milliseconds = (long)(tick / ratioTickInMilliseconds);
    public static void TickToSeconds(in long tick, out long seconds)
        => seconds = tick / ratioTickInSeconds;
    public static void TickToNanoseconds(in long tick, out double nanoseconds)
         => nanoseconds = tick * tickValueInNanoseconds;
    public static void TickToMilliseconds(in long tick, out double milliseconds)
        => milliseconds =tick / (double) ratioTickInMilliseconds;
    public static void TickToSeconds(in long tick, out double seconds)
        => seconds = tick / (double)ratioTickInSeconds;
}
