using System.Numerics;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Text;
using System.Collections;
using System.Threading;

namespace LASReader
{
    public class LASFile
    {
        public class PointDataFormat6
        {
            public double X { get; set; }
            public double Y { get; set; }
            public double Z { get; set; }
            public ushort Intensity { get; set; }
            public byte ReturnNumber_NumberofReturns { get; set; }
            public byte ClassificationFlags_ScannerChanel_ScanDirectionFlag_EdgeOfFlightLine { get; set; }
            public byte Classification { get; set; }
            public byte UserData { get; set; }
            public short ScanAngle { get; set; }
            public ushort PointSourceID { get; set; }
            public double GPSTime { get; set; }

            public PointDataFormat6(BinaryReader _reader, ushort _pointDataRecordLength)
            {
                long pos = _reader.BaseStream.Position;
                X = _reader.ReadInt32();
                Y = _reader.ReadInt32();
                Z = _reader.ReadInt32();
                Intensity = _reader.ReadUInt16();

                ReturnNumber_NumberofReturns = _reader.ReadByte();
                ClassificationFlags_ScannerChanel_ScanDirectionFlag_EdgeOfFlightLine = _reader.ReadByte();

                Classification = _reader.ReadByte();
                UserData = _reader.ReadByte();

                ScanAngle = _reader.ReadInt16();
                PointSourceID = _reader.ReadUInt16();

                GPSTime = _reader.ReadDouble();

                _reader.BaseStream.Position = pos + _pointDataRecordLength;
            }


            public PointDataFormat6(byte[] _data)
            {
                int offset = 0;
                X = BitConverter.ToInt32(_data, offset); offset += 4;
                Y = BitConverter.ToInt32(_data, offset); offset += 4;
                Z = BitConverter.ToInt32(_data, offset); offset += 4;
                Intensity = BitConverter.ToUInt16(_data, offset); offset += 2;

                ReturnNumber_NumberofReturns = _data[offset]; offset++;
                ClassificationFlags_ScannerChanel_ScanDirectionFlag_EdgeOfFlightLine = _data[offset]; offset++;

                Classification = _data[offset]; offset++;
                UserData = _data[offset]; offset++;

                ScanAngle = BitConverter.ToInt16(_data, offset); offset += 2;
                PointSourceID = BitConverter.ToUInt16(_data, offset); offset += 2;

                GPSTime = BitConverter.ToDouble(_data, offset);
            }
        }

        public class PointDataFormat7
        {
            public double X { get; set; }
            public double Y { get; set; }
            public double Z { get; set; }
            public ushort Intensity { get; set; }
            public byte ReturnNumber_NumberofReturns { get; set; }
            public byte ClassificationFlags_ScannerChanel_ScanDirectionFlag_EdgeOfFlightLine { get; set; }
            public byte Classification { get; set; }
            public byte UserData { get; set; }
            public short ScanAngle { get; set; }
            public ushort PointSourceID { get; set; }
            public double GPSTime { get; set; }
            public ushort R { get; set; }
            public ushort G { get; set; }
            public ushort B { get; set; }

        public PointDataFormat7(BinaryReader _reader, ushort _pointDataRecordLength)
            {
                long pos = _reader.BaseStream.Position;
                X = _reader.ReadInt32();
                Y = _reader.ReadInt32();
                Z = _reader.ReadInt32();
                Intensity = _reader.ReadUInt16();

                ReturnNumber_NumberofReturns = _reader.ReadByte();
                ClassificationFlags_ScannerChanel_ScanDirectionFlag_EdgeOfFlightLine = _reader.ReadByte();

                Classification = _reader.ReadByte();
                UserData = _reader.ReadByte();

                ScanAngle = _reader.ReadInt16();
                PointSourceID = _reader.ReadUInt16();

                GPSTime = _reader.ReadDouble();

                R = _reader.ReadUInt16();
                G = _reader.ReadUInt16();
                B = _reader.ReadUInt16();
                _reader.BaseStream.Position = pos + _pointDataRecordLength;
            }


            public PointDataFormat7(byte[] _data)
            {
                int offset = 0;
                X = BitConverter.ToInt32(_data, offset); offset += 4;
                Y = BitConverter.ToInt32(_data, offset); offset += 4;
                Z = BitConverter.ToInt32(_data, offset); offset += 4;
                Intensity = BitConverter.ToUInt16(_data, offset); offset += 2;

                ReturnNumber_NumberofReturns = _data[offset]; offset++;
                ClassificationFlags_ScannerChanel_ScanDirectionFlag_EdgeOfFlightLine = _data[offset]; offset++;

                Classification = _data[offset]; offset++;
                UserData = _data[offset]; offset++;

                ScanAngle = BitConverter.ToInt16(_data, offset); offset += 2;
                PointSourceID = BitConverter.ToUInt16(_data, offset); offset += 2;

                GPSTime = BitConverter.ToDouble(_data, offset); offset += 8;

                R = BitConverter.ToUInt16(_data, offset); offset += 2;
                G = BitConverter.ToUInt16(_data, offset); offset += 2;
                B = BitConverter.ToUInt16(_data, offset); 
            }
        }

        public class HeaderBlock
        {
            public char[] FileSignature { get; set; }
            public ushort FileSourceID { get; set; }
            public ushort GlobalEncoding { get; set; }

            public uint GUIDData1 { get; }
            public ushort GUIDData2 { get; }
            public ushort GUIDData3 { get; }
            public char[] GUIDData4 { get; }

            public byte VersionMajor { get; set; }
            public byte VersionMinor { get; set; }

            public char[] SystemIdentifier { get; set; }
            public char[] GeneratingSoftware { get; set; }

            public ushort FileCreationDay { get; }
            public ushort FileCreationYear { get; }
            public ushort HeaderSize { get; }
            public uint OffsetPointData { get; }
            public uint NumberVLR { get; }

            public byte PointDataRecordFormat { get; set; }
            public ushort PointDataRecordLength { get; set; }

            public uint LegacyNumberPointsRecords { get; set; }
            public BigInteger LegacyNumberPointsReturn { get; set; }

            public double XScaleFactor { get; }
            public double YScaleFactor { get; }
            public double ZScaleFactor { get; }

            public double XOffset { get; }
            public double YOffset { get; }
            public double ZOffset { get; }

            public double MaxX { get; }
            public double MaxY { get; }
            public double MaxZ { get; }

            public double MinX { get; }
            public double MinY { get; }
            public double MinZ { get; }

            public ulong StartWaveformDataPacket { get; set; }
            public ulong StartExtendedVLR { get; set; }
            public uint NumberExtendedVLR { get; set; }

            public ulong NumberPointRecords { get; set; }
            public BigInteger NumberPointsReturn { get; set; }

            public HeaderBlock(BinaryReader _reader)
            {
                FileSignature = _reader.ReadChars(4);
                FileSourceID = _reader.ReadUInt16();
                GlobalEncoding = _reader.ReadUInt16();

                GUIDData1 = _reader.ReadUInt32();
                GUIDData2 = _reader.ReadUInt16();
                GUIDData3 = _reader.ReadUInt16();
                GUIDData4 = _reader.ReadChars(8);

                VersionMajor = _reader.ReadByte();
                VersionMinor = _reader.ReadByte();

                SystemIdentifier = _reader.ReadChars(32);
                GeneratingSoftware = _reader.ReadChars(32);

                FileCreationDay = _reader.ReadUInt16();
                FileCreationYear = _reader.ReadUInt16();
                HeaderSize = _reader.ReadUInt16();
                OffsetPointData = _reader.ReadUInt32();
                NumberVLR = _reader.ReadUInt32();

                PointDataRecordFormat = _reader.ReadByte();
                PointDataRecordLength = _reader.ReadUInt16();

                LegacyNumberPointsRecords = _reader.ReadUInt32();
                LegacyNumberPointsReturn = new BigInteger(_reader.ReadBytes(20));

                XScaleFactor = _reader.ReadDouble();
                YScaleFactor = _reader.ReadDouble();
                ZScaleFactor = _reader.ReadDouble();

                XOffset = _reader.ReadDouble();
                YOffset = _reader.ReadDouble();
                ZOffset = _reader.ReadDouble();

                MaxX = _reader.ReadDouble();
                MinX = _reader.ReadDouble();

                MaxY = _reader.ReadDouble();
                MinY = _reader.ReadDouble();

                MaxZ = _reader.ReadDouble();
                MinZ = _reader.ReadDouble();

                StartWaveformDataPacket = _reader.ReadUInt64();
                StartExtendedVLR = _reader.ReadUInt64();
                NumberExtendedVLR = _reader.ReadUInt32();
                NumberPointRecords = _reader.ReadUInt64();
                NumberPointsReturn = new BigInteger(_reader.ReadBytes(120));

                _reader.BaseStream.Seek(this.OffsetPointData, 0);
            }

            public HeaderBlock(byte[] _data, BinaryReader _reader)
            {
                int offset = 0;
                FileSignature = Encoding.UTF8.GetString(_data[offset..(offset + 4)]).ToCharArray(); offset += 4;
                FileSourceID = BitConverter.ToUInt16(_data, offset); offset += 2;
                GlobalEncoding = BitConverter.ToUInt16(_data, offset); offset += 2;

                GUIDData1 = BitConverter.ToUInt32(_data, offset); offset += 4;
                GUIDData2 = BitConverter.ToUInt16(_data, offset); offset += 2;
                GUIDData3 = BitConverter.ToUInt16(_data, offset); offset += 2;
                GUIDData4 = Encoding.UTF8.GetString(_data[offset..(offset + 8)]).ToCharArray(); offset += 8;

                VersionMajor = _data[offset]; offset++;
                VersionMinor = _data[offset]; offset++;
                
                SystemIdentifier = Encoding.UTF8.GetString(_data[offset..(offset + 32)]).ToCharArray(); offset += 32;
                GeneratingSoftware = Encoding.UTF8.GetString(_data[offset..(offset + 32)]).ToCharArray(); offset += 32;

                FileCreationDay = BitConverter.ToUInt16(_data, offset); offset += 2;
                FileCreationYear = BitConverter.ToUInt16(_data, offset); offset += 2;
                HeaderSize = BitConverter.ToUInt16(_data, offset); offset += 2;
                OffsetPointData = BitConverter.ToUInt32(_data, offset); offset += 4;
                NumberVLR = BitConverter.ToUInt32(_data, offset); offset += 4;

                PointDataRecordFormat = _data[offset]; offset++;
                PointDataRecordLength = BitConverter.ToUInt16(_data, offset); offset += 2;

                LegacyNumberPointsRecords = BitConverter.ToUInt32(_data, offset); offset += 4;
                LegacyNumberPointsReturn = new BigInteger(_data[offset..(offset + 20)]); offset += 20;

                XScaleFactor = BitConverter.ToDouble(_data, offset); offset += 8;
                YScaleFactor = BitConverter.ToDouble(_data, offset); offset += 8;
                ZScaleFactor = BitConverter.ToDouble(_data, offset); offset += 8;

                XOffset = BitConverter.ToDouble(_data, offset); offset += 8;
                YOffset = BitConverter.ToDouble(_data, offset); offset += 8;
                ZOffset = BitConverter.ToDouble(_data, offset); offset += 8;

                MaxX = BitConverter.ToDouble(_data, offset); offset += 8;
                MinX = BitConverter.ToDouble(_data, offset); offset += 8;

                MaxY = BitConverter.ToDouble(_data, offset); offset += 8;
                MinY = BitConverter.ToDouble(_data, offset); offset += 8;

                MaxZ = BitConverter.ToDouble(_data, offset); offset += 8;
                MinZ = BitConverter.ToDouble(_data, offset); offset += 8;

                StartWaveformDataPacket = BitConverter.ToUInt64(_data, offset); offset += 8;
                StartExtendedVLR = BitConverter.ToUInt64(_data, offset); offset += 8;
                NumberExtendedVLR = BitConverter.ToUInt32(_data, offset); offset += 4;
                NumberPointRecords = BitConverter.ToUInt64(_data, offset); offset += 8;
                NumberPointsReturn = new BigInteger(_data[offset..(offset + 120)]);

                _reader.BaseStream.Seek(this.OffsetPointData, 0);
            }
        }

        public class RGBPoint
        {
            public double X { get; set; }
            public double Y { get; set; }
            public double Z { get; set; }
            public byte R { get; set; }
            public byte G { get; set; }
            public byte B { get; set; }

            public RGBPoint(PointDataFormat7 _point, HeaderBlock _header)
            {
                // TO DO: VERIFY THE ORDER OF THE OPERATIONS (MULTIPLICATION AND ADDITION)
                X = (_point.X * _header.XScaleFactor) + _header.XOffset;
                Y = (_point.Y * _header.YScaleFactor) + _header.YOffset;
                Z = (_point.Z * _header.ZScaleFactor) + _header.ZOffset;
                R = (byte)(_point.R * 255 / ushort.MaxValue);
                G = (byte)(_point.G * 255 / ushort.MaxValue);
                B = (byte)(_point.B * 255 / ushort.MaxValue);
            }
        }

        public class IntensityPoint
        {
            public double X { get; set; }
            public double Y { get; set; }
            public double Z { get; set; }
            public byte intensity { get; set; }

            public IntensityPoint(PointDataFormat6 _point, HeaderBlock _header)
            {
                // TO DO: VERIFY THE ORDER OF THE OPERATIONS (MULTIPLICATION AND ADDITION)
                X = (_point.X + _header.XOffset) * _header.XScaleFactor;
                Y = (_point.Y + _header.YOffset) * _header.YScaleFactor;
                Z = (_point.Z + _header.ZOffset) * _header.ZScaleFactor;
                intensity = (byte)(_point.Intensity * 255 / ushort.MaxValue);
            }
        }

        #nullable enable
            private HeaderBlock? header;
        #nullable disable
        private string Path { get; set; }
        private List<UnityEngine.Vector3> points;
        public List<UnityEngine.Vector3> Points
        { 
            get { return this.points; }
            set { this.points = value; }
        }
        private List<UnityEngine.Color32> colors;
        public List<UnityEngine.Color32> Colors
        {
            get { return this.colors; }
            set { this.colors = value; }
        }

        public bool hasColor;

        private UnityEngine.Vector3 offset;

        public UnityEngine.Vector3 Offset
        {
            get { return this.offset; }
            set { this.offset = value; }
        }
        
        private float progress;
        public float Progress { get { return this.progress; } }

        Thread thread;

        public LASFile(string _path)
        {
            this.Path = _path;
            this.progress = 0f;
            this.hasColor = false;
        }

        public ulong NumberOfPoints
        {
            get
            {
                if (this.header != null)
                    return this.header.NumberPointRecords;
                else
                    return 0;
            }
        }

        public double XScaleFactor 
        { 
            get
            {
                if (this.header != null)
                    return this.header.XScaleFactor;
                else
                    return 0;
            }
        }

        public double YScaleFactor
        {
            get
            {
                if (this.header != null)
                    return this.header.YScaleFactor;
                else
                    return 0;
            }
        }

        public double ZScaleFactor
        {
            get
            {
                if (this.header != null)
                    return this.header.ZScaleFactor;
                else
                    return 0;
            }
        }

        public double XOffset
        {
            get
            {
                if (this.header != null)
                    return this.header.XOffset;
                else
                    return 0;
            }
        }

        public double YOffset
        {
            get
            {
                if (this.header != null)
                    return this.header.YOffset;
                else
                    return 0;
            }
        }

        public double ZOffset
        {
            get
            {
                if (this.header != null)
                    return this.header.ZOffset;
                else
                    return 0;
            }
        }

        public double MaxX
        {
            get
            {
                if (this.header != null)
                    return this.header.MaxX;
                else
                    return 0;
            }
        }

        public double MaxY
        {
            get
            {
                if (this.header != null)
                    return this.header.MaxY;
                else
                    return 0;
            }
        }

        public double MaxZ
        {
            get
            {
                if (this.header != null)
                    return this.header.MaxZ;
                else
                    return 0;
            }
        }

        public double MinX
        {
            get
            {
                if (this.header != null)
                    return this.header.MinX;
                else
                    return 0;
            }
        }

        public double MinY
        {
            get
            {
                if (this.header != null)
                    return this.header.MinY;
                else
                    return 0;
            }
        }
        public double MinZ
        {
            get
            {
                if (this.header != null)
                    return this.header.MinZ;
                else
                    return 0;
            }
        }

        private static void ReadFile(string _path, 
            ref HeaderBlock _header,
            ref float _progress,
            ref List<UnityEngine.Vector3> _points,
            ref List<UnityEngine.Color32> _colors,
            ref UnityEngine.Vector3 _offset,
            ref bool _hasColor)
        {
            if (File.Exists(_path))
            {
                try
                {
                    using (BinaryReader reader = new BinaryReader(new FileStream(_path, FileMode.Open)))
                    {
                        //
                        int sBytes = 375;
                        byte[] data = reader.ReadBytes(sBytes);
                        //
                        _header = new HeaderBlock(data, reader);
                        // this.header = new HeaderBlock(reader);
                        byte pointFormat = _header.PointDataRecordFormat;
                        if (pointFormat == 7)
                        {
                            _hasColor = true;
                            int nPoints = (int)_header.NumberPointRecords;
                            _points = new List<UnityEngine.Vector3>(nPoints);
                            _colors = new List<UnityEngine.Color32>(nPoints);
                            PointDataFormat7 tempDataPoint;
                            RGBPoint rgbPoint;
                            _progress = 0f;
                            for (int i = 0; i < nPoints; i++)
                            {
                                //
                                _progress = (float)i / (float)nPoints;
                                data = reader.ReadBytes(_header.PointDataRecordLength);
                                //
                                tempDataPoint = new PointDataFormat7(data);
                                // tempDataPoint = new PointDataFormat7(reader, this.header.PointDataRecordLength);
                                rgbPoint = new RGBPoint(tempDataPoint, _header);

                                _points.Add(new UnityEngine.Vector3((float)rgbPoint.X, (float)rgbPoint.Y, (float)rgbPoint.Z));
                                _colors.Add(new UnityEngine.Color32(rgbPoint.R, rgbPoint.G, rgbPoint.B, 255));
                            }
                            // _offset = _points.Aggregate(new UnityEngine.Vector3(0, 0, 0), (s, v) => s + v) / (float)_points.Count;
                            _progress = 1f;
                        }
                        else if(pointFormat == 6)
                        {
                            _hasColor = false;
                            int nPoints = (int)_header.NumberPointRecords;
                            _points = new List<UnityEngine.Vector3>(nPoints);
                            _colors = new List<UnityEngine.Color32>(nPoints);
                            PointDataFormat6 tempDataPoint;
                            IntensityPoint iPoint;
                            _progress = 0f;
                            for (int i = 0; i < nPoints; i++)
                            {
                                //
                                _progress = (float)i / (float)nPoints;
                                data = reader.ReadBytes(_header.PointDataRecordLength);
                                //
                                tempDataPoint = new PointDataFormat6(data);
                                iPoint = new IntensityPoint(tempDataPoint, _header);
                                _points.Add(new UnityEngine.Vector3((float)iPoint.X, (float)iPoint.Y, (float)iPoint.Z));
                                //byte intensity = LASFile.UShortByte(tempDataPoint.Intensity);
                                _colors.Add(new UnityEngine.Color32(200, 200, 200, 255));
                            }
                            // _offset = _points.Aggregate(new UnityEngine.Vector3(0, 0, 0), (s, v) => s + v) / (float)_points.Count;
                            _progress = 1f;
                        }
                        else
                        {
                            Debug.Log("[info] Only supported Point Data Record Format 6, and 7!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("[error] Failed opening LAS file: " + ex.Message);
                }
            }
            else
            {
                Debug.Log("[info] File does not exists!");
            }   
        }

        public void Read(bool _wait = false) // if true, main thread wait to read the whole file
        {
            this.thread = new Thread(() => LASFile.ReadFile(this.Path,
                        ref this.header,
                        ref this.progress,
                        ref this.points,
                        ref this.colors,
                        ref this.offset,
                        ref this.hasColor));
            this.thread.Start();
            if (_wait)
                this.Close();
        }

        public void Close()
        {
            this.thread?.Join();
        }
        
        /*
        public IEnumerator ReadFileCorontine(System.Action<float> callback)
        {
            int unit = 1000000;
            int processed = 0;

            if (File.Exists(this.Path))
            {
                using (BinaryReader reader = new BinaryReader(new FileStream(this.Path, FileMode.Open)))
                {
                    int sBytes = 375;
                    byte[] data = reader.ReadBytes(sBytes);

                    this.header = new HeaderBlock(data, reader);
                    byte pointFormat = this.header.PointDataRecordFormat;
                    if (pointFormat == 7)
                    {
                        int nPoints = (int)this.NumberOfPoints;
                        this.Points = new List<UnityEngine.Vector3>(nPoints);
                        this.Colors = new List<UnityEngine.Color32>(nPoints);

                        PointDataFormat7 tempDataPoint;
                        RGBPoint rgbPoint;
                        UnityEngine.Vector3 offset = UnityEngine.Vector3.zero;

                        while (processed < nPoints)
                        {
                            for (int i = 0; i < unit && processed + i < nPoints; i++)
                            {
                                data = reader.ReadBytes(this.header.PointDataRecordLength);

                                tempDataPoint = new PointDataFormat7(data);

                                rgbPoint = new RGBPoint(tempDataPoint, this.header);
                                if (processed == 0 && i == 0)
                                {
                                    offset = new UnityEngine.Vector3((float)rgbPoint.X, (float)rgbPoint.Y, (float)rgbPoint.Z);
                                }
                                this.Points.Add(new UnityEngine.Vector3((float)rgbPoint.X, (float)rgbPoint.Y, (float)rgbPoint.Z) - offset);
                                this.Colors.Add(new UnityEngine.Color32(rgbPoint.R, rgbPoint.G, rgbPoint.B, 255));
                            }
                            processed += unit;
                            Debug.Log("Processed: " + processed + ", total: " + nPoints);
                            callback((float)processed / nPoints);
                            yield return new WaitForEndOfFrame();
                        }

                        // callback(1.0f);
                        yield return true;
                    }
                    else
                    {
                        Debug.Log("[info] Only supported Point Data Record Format 7!");
                    }
                }
            }
            else
                Debug.Log("[info] File does not exists!");
            yield return null;
        }
        */
    }
}