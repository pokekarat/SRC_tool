﻿// ----------------------------------------------------
// This C# program reads and processes each sample in a
// PT4 file sequentially, from the first sample to the
// last.
//
// The name of the file to be processed is taken from
// the command line argument.
//
// Copyright (c) Monsoon Solutions, Inc.
// All Rights Reserved.
// ----------------------------------------------------
using System;
using System.IO;
using System.Diagnostics;
namespace Parse
{
    public static class PT4
    {
        // fixed file offsets
        public const long headerOffset = 0;
        public const long statusOffset = 272;
        public const long sampleOffset = 1024;
        // bitmasks
        private const short coarseMask = 1;
        private const ushort marker0Mask = 1;
        private const ushort marker1Mask = 2;
        private const ushort markerMask = marker0Mask | marker1Mask;
        // missing data indicators
        const short missingRawCurrent = unchecked((short)0x8001);
        const ushort missingRawVoltage = 0xffff;
        // Enums for file header
        public enum CalibrationStatus : int
        {
            OK, Failed
        }
        public enum VoutSetting : int
        {
            Typical, Low, High, Custom
        }
        [FlagsAttribute] // bitwise-maskable
        public enum SelectField : int
        {
            None = 0x00,
            Avg = 0x01,
            Min = 0x02,
            Max = 0x04,
            Main = 0x08,
            Usb = 0x10,
            Aux = 0x20,
            Marker = 0x40,
            All = Avg | Min | Max
        }
        public enum RunMode : int
        {
            NoGUI, GUI
        }
        [FlagsAttribute] // bitwise-maskable
        public enum CaptureMask : ushort
        {
            chanMain = 0x1000,
            chanUsb = 0x2000,
            chanAux = 0x4000,
            chanMarker = 0x8000,
            chanMask = 0xf000,
        }
        // File header structure
        public struct Pt4Header
        {
            public int headSize;
            public string name;
            public int batterySize;
            public DateTime captureDate;
            public string serialNumber;
            public CalibrationStatus calibrationStatus;
            public VoutSetting voutSetting;
            public float voutValue;
            public int hardwareRate;
            public float softwareRate; // ignore
            public SelectField powerField;
            public SelectField currentField;
            public SelectField voltageField;
            public string captureSetting;
            public string swVersion;
            public RunMode runMode;
            public int exitCode;
            public long totalCount;
            public ushort statusOffset;
            public ushort statusSize;
            public ushort sampleOffset;
            public ushort sampleSize;
            public ushort initialMainVoltage;
            public ushort initialUsbVoltage;
            public ushort initialAuxVoltage;
            public CaptureMask captureDataMask;
            public ulong sampleCount;
            public ulong missingCount;
            public float avgMainVoltage;
            public float avgMainCurrent;
            public float avgMainPower;
            public float avgUsbVoltage;
            public float avgUsbCurrent;
            public float avgUsbPower;
            public float avgAuxVoltage;
            public float avgAuxCurrent;
            public float avgAuxPower;
        }
        static public void ReadHeader(BinaryReader reader,
        ref Pt4Header header)
        {
            // remember original position
            long oldPos = reader.BaseStream.Position;
            // move to start of file
            reader.BaseStream.Position = 0;
            // read file header
            header.headSize = reader.ReadInt32();
            header.name = reader.ReadString().Trim();
            header.batterySize = reader.ReadInt32();
            header.captureDate =
            DateTime.FromBinary(reader.ReadInt64());
            header.serialNumber = reader.ReadString().Trim();
            header.calibrationStatus =
            (CalibrationStatus)reader.ReadInt32();
            header.voutSetting = (VoutSetting)reader.ReadInt32();
            header.voutValue = reader.ReadSingle();
            header.hardwareRate = reader.ReadInt32();
            header.softwareRate = (float)header.hardwareRate;
            reader.ReadSingle(); // ignore software rate
            header.powerField = (SelectField)reader.ReadInt32();
            header.currentField = (SelectField)reader.ReadInt32();
            header.voltageField = (SelectField)reader.ReadInt32();
            header.captureSetting = reader.ReadString().Trim();
            header.swVersion = reader.ReadString().Trim();
            header.runMode = (RunMode)reader.ReadInt32();
            header.exitCode = reader.ReadInt32();
            header.totalCount = reader.ReadInt64();
            header.statusOffset = reader.ReadUInt16();
            header.statusSize = reader.ReadUInt16();
            header.sampleOffset = reader.ReadUInt16();
            header.sampleSize = reader.ReadUInt16();
            header.initialMainVoltage = reader.ReadUInt16();
            header.initialUsbVoltage = reader.ReadUInt16();
            header.initialAuxVoltage = reader.ReadUInt16();
            header.captureDataMask = (CaptureMask)reader.ReadUInt16();
            header.sampleCount = reader.ReadUInt64();
            header.missingCount = reader.ReadUInt64();
            ulong count = Math.Max(1, header.sampleCount -
            header.missingCount);
            // convert sums to averages
            header.avgMainVoltage = reader.ReadSingle() / count;
            header.avgMainCurrent = reader.ReadSingle() / count;
            header.avgMainPower = reader.ReadSingle() / count;
            header.avgUsbVoltage = reader.ReadSingle() / count;
            header.avgUsbCurrent = reader.ReadSingle() / count;
            header.avgUsbPower = reader.ReadSingle() / count;
            header.avgAuxVoltage = reader.ReadSingle() / count;
            header.avgAuxCurrent = reader.ReadSingle() / count;
            header.avgAuxPower = reader.ReadSingle() / count;
            // restore original position
            reader.BaseStream.Position = oldPos;
        }
        // Enums for status packet
        public enum PacketType : byte
        {
            set = 1, start, stop,
            status = 0x10, sample = 0x20
        }
        public struct Observation
        {
            public short mainCurrent;
            public short usbCurrent;
            public short auxCurrent;
            public ushort voltage;
        }
        [FlagsAttribute]
        public enum PmStatus : byte
        {
            unitNotAtVoltage = 0x01,
            cannotPowerOutput = 0x02,
            checksumError = 0x04,
            followsLastSamplePacket = 0x08,
            responseToStopPacket = 0x10,
            responseToResetCommand = 0x20,
            badPacketReceived = 0x40
        }
        [FlagsAttribute]
        public enum Leds : byte
        {
            disableButtonPressed = 0x01,
            errorLedOn = 0x02,
            fanMotorOn = 0x04,
            voltageIsAux = 0x08,
        }
        public enum HardwareRev : byte
        {
            revA = 1, revB, revC, revD
        }
        public enum UsbPassthroughMode : byte
        {
            off, on, auto, trigger, sync
        }
        public enum EventCode : byte
        {
            noEvent = 0,
            usbConnectionLost,
            tooManyDroppedObservations,
            resetRequestedByHost
        }
        // Statuc packet structure
        public struct StatusPacket
        {
            public byte packetLength;
            public PacketType packetType;
            public byte firmwareVersion;
            public byte protocolVersion;
            public Observation fineObs;
            public Observation coarseObs;
            public byte outputVoltageSetting;
            public sbyte temperature;
            public PmStatus pmStatus;
            public byte reserved;
            public Leds leds;
            public sbyte mainFineResistorOffset;
            public ushort serialNumber;
            public byte sampleRate;
            public ushort dacCalLow;
            public ushort dacCalHigh;
            public ushort powerupCurrentLimit;
            public ushort runtimeCurrentLimit;
            public byte powerupTime;
            public sbyte usbFineResistorOffset;
            public sbyte auxFineResistorOffset;
            public ushort initialUsbVoltage;
            public ushort initialAuxVoltage;
            public HardwareRev hardwareRevision;
            public byte temperatureLimit;
            public UsbPassthroughMode usbPassthroughMode;
            public sbyte mainCoarseResistorOffset;
            public sbyte usbCoarseResistorOffset;
            public sbyte auxCoarseResistorOffset;
            public sbyte factoryMainFineResistorOffset;
            public sbyte factoryUsbFineResistorOffset;
            public sbyte factoryAuxFineResistorOffset;
            public sbyte factoryMainCoarseResistorOffset;
            public sbyte factoryUsbCoarseResistorOffset;
            public sbyte factoryAuxCoarseResistorOffset;
            public EventCode eventCode;
            public ushort eventData;
            public byte checksum;
        }
        static public void ReadStatusPacket(BinaryReader reader,
        ref StatusPacket status)
        {
            // remember origibal position
            long oldPos = reader.BaseStream.Position;
            // move to start of status packet
            reader.BaseStream.Position = statusOffset;
            // read status packet
            status.packetLength = reader.ReadByte();
            status.packetType = (PacketType)reader.ReadByte();
            Debug.Assert(status.packetType == PacketType.status);
            status.firmwareVersion = reader.ReadByte();
            status.protocolVersion = reader.ReadByte();
            Debug.Assert(status.protocolVersion >= 16);
            status.fineObs.mainCurrent = reader.ReadInt16();
            status.fineObs.usbCurrent = reader.ReadInt16();
            status.fineObs.auxCurrent = reader.ReadInt16();
            status.fineObs.voltage = reader.ReadUInt16();
            status.coarseObs.mainCurrent = reader.ReadInt16();
            status.coarseObs.usbCurrent = reader.ReadInt16();
            status.coarseObs.auxCurrent = reader.ReadInt16();
            status.coarseObs.voltage = reader.ReadUInt16();
            status.outputVoltageSetting = reader.ReadByte();
            status.temperature = reader.ReadSByte();
            status.pmStatus = (PmStatus)reader.ReadByte();
            status.reserved = reader.ReadByte();
            status.leds = (Leds)reader.ReadByte();
            status.mainFineResistorOffset = reader.ReadSByte();
            status.serialNumber = reader.ReadUInt16();
            status.sampleRate = reader.ReadByte();
            status.dacCalLow = reader.ReadUInt16();
            status.dacCalHigh = reader.ReadUInt16();
            status.powerupCurrentLimit = reader.ReadUInt16();
            status.runtimeCurrentLimit = reader.ReadUInt16();
            status.powerupTime = reader.ReadByte();
            status.usbFineResistorOffset = reader.ReadSByte();
            status.auxFineResistorOffset = reader.ReadSByte();
            status.initialUsbVoltage = reader.ReadUInt16();
            status.initialAuxVoltage = reader.ReadUInt16();
            status.hardwareRevision = (HardwareRev)reader.ReadByte();
            status.temperatureLimit = reader.ReadByte();
            status.usbPassthroughMode =
            (UsbPassthroughMode)reader.ReadByte();
            status.mainCoarseResistorOffset = reader.ReadSByte();
            status.usbCoarseResistorOffset = reader.ReadSByte();
            status.auxCoarseResistorOffset = reader.ReadSByte();
            status.factoryMainFineResistorOffset = reader.ReadSByte();
            status.factoryUsbFineResistorOffset = reader.ReadSByte();
            status.factoryAuxFineResistorOffset = reader.ReadSByte();
            status.factoryMainCoarseResistorOffset =
            reader.ReadSByte();
            status.factoryUsbCoarseResistorOffset =
            reader.ReadSByte();
            status.factoryAuxCoarseResistorOffset =
            reader.ReadSByte();
            status.eventCode = (EventCode)reader.ReadByte();
            status.eventData = reader.ReadUInt16();
            status.checksum = reader.ReadByte();
            // restore original position
            reader.BaseStream.Position = oldPos;
        }
        static public long BytesPerSample(CaptureMask captureDataMask)
        {
            long result = sizeof(ushort); // voltage always present
            if ((captureDataMask & CaptureMask.chanMain) != 0)
                result += sizeof(short);
            if ((captureDataMask & CaptureMask.chanUsb) != 0)
                result += sizeof(short);
            if ((captureDataMask & CaptureMask.chanAux) != 0)
                result += sizeof(short);
            return result;
        }
        static public long SamplePosition(long sampleIndex,
        CaptureMask captureDataMask)
        {
            long result = sampleOffset +
            BytesPerSample(captureDataMask) * sampleIndex;
            return result;
        }
        static public long SamplePosition(double seconds,
        CaptureMask captureDataMask,
        ref StatusPacket statusPacket)
        {
            seconds = Math.Max(0, seconds);
            long bytesPerSample = BytesPerSample(captureDataMask);
            long freq = 1000 * statusPacket.sampleRate;
            long result = (long)(seconds * freq * bytesPerSample);
            long err = result % bytesPerSample;
            if (err > 0) // must fall on boundary
                result += (bytesPerSample - err);
            result += sampleOffset;
            return result;
        }
        static public long SampleCount(BinaryReader reader,
        CaptureMask captureDataMask)
        {
            return (reader.BaseStream.Length - sampleOffset)
            / BytesPerSample(captureDataMask);
        }
        public struct Sample
        {
            public long sampleIndex; // 0...N-1
            public double timeStamp; // fractional seconds
            public bool mainPresent; // whether Main was recorded
            public double mainCurrent; // current in milliamps
            public double mainVoltage; // volts
            public bool usbPresent; // whether Usb was recorded
            public double usbCurrent; // current in milliamps
            public double usbVoltage; // volts
            public bool auxPresent; // whether Aux was recorded
            public double auxCurrent; // current in milliamps
            public double auxVoltage; // volts;
            public bool markerPresent; // whether markers/voltages
            // were recorded
            public bool marker0; // Marker 0
            public bool marker1; // Marker 1
            public bool missing; // true if this sample was
            // missing
        }
        static public void GetSample(long sampleIndex,
        CaptureMask captureDataMask,
        StatusPacket statusPacket,
        BinaryReader reader,
        ref Sample sample)
        {
            // remember the index and time
            sample.sampleIndex = sampleIndex;
            sample.timeStamp = sampleIndex
            / (1000.0 * statusPacket.sampleRate);
            // intial settings for all flags
            sample.mainPresent =
            (captureDataMask & CaptureMask.chanMain) != 0;
            sample.usbPresent =
            (captureDataMask & CaptureMask.chanUsb) != 0;
            sample.auxPresent =
            (captureDataMask & CaptureMask.chanAux) != 0;
            sample.markerPresent = true;
            sample.missing = false;
            // abort if no data was selected
            long bytesPerSample = BytesPerSample(captureDataMask);
            if (bytesPerSample == 0)
                return;
            // remember original position
            long oldPos = reader.BaseStream.Position;
            // position the file to the start of the desired sample
            long newPos = SamplePosition(sampleIndex, captureDataMask);
            if (oldPos != newPos)
                reader.BaseStream.Position = newPos;
            // get default voltages (V) for the three channels
            sample.mainVoltage =
            2.0 + statusPacket.outputVoltageSetting * 0.01;
            sample.usbVoltage =
            (double)statusPacket.initialUsbVoltage * 125 / 1e6f;
            if (statusPacket.hardwareRevision < HardwareRev.revB)
                sample.usbVoltage /= 2;
            sample.auxVoltage =
            (double)statusPacket.initialAuxVoltage * 125 / 1e6f;
            if (statusPacket.hardwareRevision < HardwareRev.revC)
                sample.auxVoltage /= 2;
            // Main current (mA)
            if (sample.mainPresent)
            {
                short raw = reader.ReadInt16();
                sample.missing = sample.missing ||
                raw == missingRawCurrent;
                if (!sample.missing)
                {
                    bool coarse = (raw & coarseMask) != 0;
                    raw &= ~coarseMask;
                    sample.mainCurrent = raw / 1000f; // uA -> mA
                    if (coarse)
                        sample.mainCurrent *= 250;
                }
            }
            // Aux1 current (mA)
            if (sample.usbPresent)
            {
                short raw = reader.ReadInt16();
                sample.missing = sample.missing ||
                raw == missingRawCurrent;
                if (!sample.missing)
                {
                    bool coarse = (raw & coarseMask) != 0;
                    raw &= ~coarseMask;
                    sample.usbCurrent = raw / 1000f; // uA -> mA
                    if (coarse)
                        sample.usbCurrent *= 250;
                }
            }
            // Aux2 current (mA)
            if (sample.auxPresent)
            {
                short raw = reader.ReadInt16();
                sample.missing = sample.missing ||
                raw == missingRawCurrent;
                if (!sample.missing)
                {
                    bool coarse = (raw & coarseMask) != 0;
                    raw &= ~coarseMask;
                    sample.auxCurrent = raw / 1000f; // uA -> mA
                    if (coarse)
                        sample.auxCurrent *= 250;
                }
            }
            // Markers and Voltage (V)
            {
                ushort uraw = reader.ReadUInt16();
                sample.missing = sample.missing ||
                uraw == missingRawVoltage;
                if (!sample.missing)
                {
                    // strip out marker bits
                    sample.marker0 = (uraw & marker0Mask) != 0;
                    sample.marker1 = (uraw & marker1Mask) != 0;
                    uraw &= unchecked((ushort)~markerMask);
                    // calculate voltage
                    double voltage = (double)uraw * 125 / 1e6f;
                    // assign the high-res voltage, as appropriate
                    if ((statusPacket.leds & Leds.voltageIsAux) != 0)
                    {
                        sample.auxVoltage = voltage;
                        if (statusPacket.hardwareRevision
                        < HardwareRev.revC)
                        {
                            sample.auxVoltage /= 2;
                        }
                    }
                    else
                    {
                        sample.mainVoltage = voltage;
                        if (statusPacket.hardwareRevision
                        < HardwareRev.revB)
                        {
                            sample.mainVoltage /= 2;
                        }
                    }
                }
            }
            // restore original position, if we moved it earlier
            if (oldPos != newPos)
                reader.BaseStream.Position = oldPos;
        }
    }
    /* class ReadPT4
     {
         static void Main(string[] args)
         {
             // open the file named in the command line argument
             string fileName = args[0];
             if (!File.Exists(fileName))
                 return;
             FileStream pt4Stream = File.Open(fileName,
             FileMode.Open,
             FileAccess.Read,
             FileShare.ReadWrite);
             BinaryReader pt4Reader = new BinaryReader(pt4Stream);
             // reader the file header
             PT4.Pt4Header header = new PT4.Pt4Header();
             PT4.ReadHeader(pt4Reader, ref header);
             // read the Status Packet
             PT4.StatusPacket statusPacket = new PT4.StatusPacket();
             PT4.ReadStatusPacket(pt4Reader, ref statusPacket);
             // determine the number of samples in the file
             long sampleCount = PT4.SampleCount(pt4Reader,
             header.captureDataMask);
             // pre-position input file to the beginning of the sample // data (saves a lot of repositioning in the GetSample // routine)
             pt4Reader.BaseStream.Position = PT4.sampleOffset;
             // process the samples sequentially, beginning to end
             PT4.Sample sample = new PT4.Sample();
             for (long sampleIndex = 0;
             sampleIndex < sampleCount;
             sampleIndex++)
             {
                 // read the next sample
                 PT4.GetSample(sampleIndex, header.captureDataMask,
                 statusPacket, pt4Reader, ref sample);
                 // process the sample
                 Console.WriteLine("#{0} {1} sec {2} mA {3} V",
                 sampleIndex,
                 sample.timeStamp,
                 sample.mainCurrent,
                 sample.mainVoltage);
             }
             // close input file
             pt4Reader.Close();
         }
     }*/
}