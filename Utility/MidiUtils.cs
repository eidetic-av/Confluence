using System;
using MidiJack;

namespace Eidetic.Confluence
{
    public static partial class Utility 
    {
        /// <summary>
        /// Returns a MidiChannel object from an int.
        /// If the channel doesn't exist, it will return MidiChannel.All.
        /// </summary>
        public static MidiChannel MidiChannelFromInt(int channelNumber)
        {
            var channels = (MidiChannel[]) Enum.GetValues(typeof(MidiChannel));
            if (channelNumber >= 1 && channelNumber <= 16)
                return channels[channelNumber - 1];
            else return channels[16];
        }

        /// <summary>
        /// Returns an int of the midi channel number.
        /// </summary>
        public static int ToNumber(this MidiChannel channel)
        {
            switch (channel)
            {
                case MidiChannel.Ch1:
                    return 1;
                case MidiChannel.Ch2:
                    return 2;
                case MidiChannel.Ch3:
                    return 3;
                case MidiChannel.Ch4:
                    return 4;
                case MidiChannel.Ch5:
                    return 5;
                case MidiChannel.Ch6:
                    return 6;
                case MidiChannel.Ch7:
                    return 7;
                case MidiChannel.Ch8:
                    return 8;
                case MidiChannel.Ch9:
                    return 9;
                case MidiChannel.Ch10:
                    return 10;
                case MidiChannel.Ch11:
                    return 11;
                case MidiChannel.Ch12:
                    return 12;
                case MidiChannel.Ch13:
                    return 13;
                case MidiChannel.Ch14:
                    return 14;
                case MidiChannel.Ch15:
                    return 15;
                case MidiChannel.Ch16:
                    return 16;
                case MidiChannel.All:
                    return 99;
                default:
                    return -1;
            }
        }
    }
}