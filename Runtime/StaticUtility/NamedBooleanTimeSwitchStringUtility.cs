using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class NamedBooleanTimeSwitchStringUtility 
{
    public static void GetStringDescriptionByMillisecondsSegment(
        in INamedBooleanTimeSwitchRegisterGet target,
        in string namedBooleen,
        out string generatedText,
        in string falseToTrue = "▁",
        in string trueToFalse = "▔",
        in int charStopCheckPoint = 100
        )
    {

        target.IsBooleanRegistered(in namedBooleen, out bool exist);
        if (!exist)
        {
            generatedText = "xx";
            return;
        }
        target.GetCollectionOfSwitch(in namedBooleen, out INamedBooleanTimeSwitchCollectionGet collection);
        GetStringDescriptionByMillisecondsSegment(in collection, out generatedText, in falseToTrue, in trueToFalse, in charStopCheckPoint);

    }

    public static void GetStringDescriptionByMillisecondsSegment(
       in INamedBooleanTimeSwitchCollectionGet target,
       out string generatedText,
       in string falseToTrue = "▁",
       in string trueToFalse = "▔",
       in int charStopCheckPoint = 100
       )
    {
        
        target.HasAnySwitchHappened(out bool switchHappened);
        if (!switchHappened)
        {

            generatedText = "--";
            return;
        }

        target.GetAllSwitchKeyInCollection(
            out IBooleanDateStateSwitch[] sample);
        StringBuilder sb = new StringBuilder();

        long recent = DateTime.Now.Ticks, old;
        sample[0].GetWhenSwitchHappened(out old);
        long spaceInTick = recent - old;
        DateTimeTickUtility.TickToMilliseconds(in spaceInTick, out long milli);
        sb.Append(milli);
        sb.Append(sample[0].TurnedTrue() ? falseToTrue : trueToFalse);
        sb.Append(' ');

        for (int i = 1; i < sample.Length; i++)
        {
            sample[i - 1].GetWhenSwitchHappened(out recent);
            sample[i].GetWhenSwitchHappened(out old);
            spaceInTick = recent - old;
            DateTimeTickUtility.TickToMilliseconds(in spaceInTick, out milli);
            sb.Append(milli);
            sb.Append(sample[i].TurnedTrue() ? falseToTrue : trueToFalse);
            if (sb.Length > charStopCheckPoint) break;
            sb.Append(' ');
        }


        sb.Append('|');

        target.GetKeyAtBordersOfCollection(
           out IBooleanDateStateSwitch recentLimit,
           out IBooleanDateStateSwitch oldLimit);
        sb.Append(oldLimit.TurnedTrue() ? falseToTrue : trueToFalse);

        generatedText = sb.ToString();
    }

    public static void GetStringDescriptionCharPerSeconds(in INamedBooleanTimeSwitchRegisterGet target,
        in string namedBooleen,
        out string generatedText,
        in float charPerSeconds = 1,
        in string falseToTrue = "▁",
        in string trueToFalse = "▔",
        in int charStopCheckPoint = 100
        )
    {
        target.IsBooleanRegistered(in namedBooleen, out bool exist);
        if (!exist)
        {
            generatedText = "xx";
            return;
        }
        target.GetCollectionOfSwitch(in namedBooleen, out INamedBooleanTimeSwitchCollectionGet collection);
        GetStringDescriptionCharPerSeconds(in collection, out generatedText,
            in charPerSeconds, in falseToTrue, in trueToFalse, in charStopCheckPoint);
    }

    public static void GetStringDescriptionCharPerSeconds(
        in INamedBooleanTimeSwitchCollectionGet target,
        out string generatedText,
       in float charPerSeconds = 1,
       in string falseToTrue = "▁",
       in string trueToFalse = "▔",
       in int charStopCheckPoint = 100
       )
    {
        
        target.HasAnySwitchHappened( out bool switchHappened);
        if (!switchHappened)
        {

            generatedText = "--";
            return;
        }



        target.GetAllSwitchKeyInCollection(
            out IBooleanDateStateSwitch[] sample);
        StringBuilder sb = new StringBuilder();

        long recent = DateTime.Now.Ticks, old;
        sample[0].GetWhenSwitchHappened(out old);
        long spaceInTick = recent - old;
        DateTimeTickUtility.TickToSeconds(in spaceInTick, out double seconds);
        sb.Append(string.Format("{0:0.00}", seconds));

        string s = sample[0].TurnedTrue() ? falseToTrue : trueToFalse;
        for (int j = 0; j < 1f + (seconds * charPerSeconds); j++)
        {
            sb.Append(s);
        }
        sb.Append(' ');

        for (int i = 1; i < sample.Length; i++)
        {
            sample[i - 1].GetWhenSwitchHappened(out recent);
            sample[i].GetWhenSwitchHappened(out old);
            spaceInTick = recent - old;
            DateTimeTickUtility.TickToSeconds(in spaceInTick, out seconds);
            //sb.Append(seconds);
            s = sample[i].TurnedTrue() ? falseToTrue : trueToFalse;
            for (int j = 0; j < 1 + (seconds * charPerSeconds); j++)
            {
                sb.Append(s);
            }
            sb.Append(' ');
            if (sb.Length > charStopCheckPoint) break;
        }


        sb.Append('|');

        target.GetKeyAtBordersOfCollection(
           out IBooleanDateStateSwitch recentLimit,
           out IBooleanDateStateSwitch oldLimit);
        sb.Append(oldLimit.TurnedTrue() ? falseToTrue : trueToFalse);

        generatedText = sb.ToString();
    }
}
