using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _7._72_dat_editor
{
    public static class DatManager
    {
        public class Item
        {
            public uint id;
            public bool ground;
            public uint groundSpeed;
            public bool clip;
            public bool top;
            public bool bottom;
            public bool container;
            public bool stackable;
            public bool corpse;
            public bool usable;
            public bool writable;
            public uint writableLenght;
            public bool readable;
            public uint readableLenght;
            public bool fluid;
            public bool splash;
            public bool blocking;
            public bool immovable;
            public bool blocksMissile;
            public bool blocksPath;
            public bool pickupable;
            public bool hangable;
            public bool vertical;
            public bool horizontal;
            public bool rotatable;
            public bool hasLight;
            public uint lightLevel;
            public uint lightColor;
            public bool floorchange;
            public bool hasOffset;
            public bool dontHide;
            public uint offsetX;
            public uint offsetY;
            public bool hasHeight;
            public uint height;
            public bool layer;
            public bool idleAnimated;
            public bool minimap;
            public uint minimapColor;
            public bool actioned;
            public uint actions;
            //actions?
            public bool groundItem;

            public Item(uint id)
            {
                this.id = id;
            }
        };

        public static List<Item> Items = new List<Item>();
        private static UInt32 datSignature;
        private static UInt16 looktypes;
        private static UInt16 effects;
        private static UInt16 missiles;

        public static bool ParseDat(string path)
        {
            try
            {
                using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
                {
                    datSignature = reader.ReadUInt32();
                    uint maxClientId = reader.ReadUInt16();
                    looktypes = reader.ReadUInt16();
                    effects = reader.ReadUInt16();
                    missiles = reader.ReadUInt16();

                    byte flag;
                    uint id = 100;
                    while (id <= maxClientId)
                    {
                        Item item = new Item(id);
                        do
                        {
                            flag = reader.ReadByte();
                            switch (flag)
                            {
                                case 0x00: // ground
                                    item.ground = true;
                                    item.groundSpeed = reader.ReadUInt16(); // ground speed
                                    break;
                                case 0x01: // clip
                                    item.clip = true;
                                    break;
                                case 0x02: // bottom
                                    item.bottom = true;
                                    break;
                                case 0x03: // top
                                    item.top = true;
                                    break;
                                case 0x04: // container
                                    item.container = true;
                                    break;
                                case 0x05: // stackable
                                    item.stackable = true;
                                    break;
                                case 0x06: // corpse (force use)
                                    item.corpse = true;
                                    break;
                                case 0x07: // usable
                                    item.usable = true;
                                    break;
                                case 0x08: // writable
                                    item.writable = true;
                                    item.writableLenght = reader.ReadUInt16(); // maxTextLenght
                                    break;
                                case 0x09: // readable
                                    item.readable = true;
                                    item.readableLenght = reader.ReadUInt16(); // maxTextLenght
                                    break;
                                case 0x0A: // fluid container
                                    item.fluid = true;
                                    break;
                                case 0x0B: // splash
                                    item.splash = true;
                                    break;
                                case 0x0C: // blocking
                                    item.blocking = true;
                                    break;
                                case 0x0D: // not movable
                                    item.immovable = true;
                                    break;
                                case 0x0E: // blocks missile
                                    item.blocksMissile = true;
                                    break;
                                case 0x0F: // blocks path
                                    item.blocksPath = true;
                                    break;
                                case 0x10: // pickupable
                                    item.pickupable = true;
                                    break;
                                case 0x11: // hangable
                                    item.hangable = true;
                                    break;
                                case 0x12: // horizontal
                                    item.horizontal = true;
                                    break;
                                case 0x13: // vertical
                                    item.vertical = true;
                                    break;
                                case 0x14: // rotatable
                                    item.rotatable = true;
                                    break;

                                case 0x15: // light info
                                    item.hasLight = true;
                                    item.lightLevel = reader.ReadUInt16(); // light level
                                    item.lightColor = reader.ReadUInt16(); // light color
                                    break;
                                case 0x16: // don't hide
                                    item.dontHide = true;
                                    break;
                                case 0x17: // translucent
                                    item.floorchange = true;
                                    break;
                                case 0x18: // offset
                                    item.hasOffset = true;
                                    item.offsetX = reader.ReadUInt16(); // X
                                    item.offsetY = reader.ReadUInt16(); // Y
                                    break;
                                case 0x19: // height (elevation)
                                    item.hasHeight = true;
                                    item.height = reader.ReadUInt16(); // height
                                    break;
                                case 0x1A: // lying object
                                    item.layer = true;
                                    break;
                                case 0x1B: // idle animated
                                    item.idleAnimated = true;
                                    break;
                                case 0x1C: // minimap
                                    item.minimap = true;
                                    item.minimapColor = reader.ReadUInt16(); // color
                                    break;
                                case 0x1D: // actions
                                    item.actioned = true;
                                    item.actions = reader.ReadUInt16();
                                    break;
                                case 0x1E: // full ground
                                    item.groundItem = true;
                                    break;
                                case 0xFF: // end of flags
                                    break;
                                default:
                                    // MessageBox.Show("Unknown byte " + flag.ToString("X2"));
                                    break;
                            }

                        } while (flag != 0xFF);

                        uint width = reader.ReadByte();
                        uint height = reader.ReadByte();
                        if (width > 1 || height > 1)
                        {
                            reader.ReadByte(); // skip byte
                        }

                        uint blendFrames = reader.ReadByte();
                        uint divX = reader.ReadByte();
                        uint divY = reader.ReadByte();
                        uint divZ = reader.ReadByte();
                        uint animations = reader.ReadByte();

                        uint sprites = (uint)width * height * blendFrames * divX * divY * divZ * animations;
                        for (int i = 0; i < sprites; i++)
                        {
                            reader.ReadUInt16(); //sprite id
                        }

                        Items.Add(item); //add current item to items list
                        ++id;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static bool SaveDat(string path)
        {
            try
            {
                using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(path)))
                {
                    writer.Write((UInt32)datSignature);
                    writer.Write((UInt16)(Items.Count + 100));
                    writer.Write((UInt16)looktypes);
                    writer.Write((UInt16)effects);
                    writer.Write((UInt16)missiles);

                    foreach (var item in Items)
                    {
                        if (item.ground)
                        {
                            writer.Write((byte)0x00);
                            writer.Write((UInt16)item.groundSpeed);
                        }

                        if (item.clip)
                        {
                            writer.Write((byte)0x01);
                        }

                        if (item.bottom)
                        {
                            writer.Write((byte)0x02);
                        }

                        if (item.top)
                        {
                            writer.Write((byte)0x03);
                        }

                        if (item.container)
                        {
                            writer.Write((byte)0x04);
                        }

                        if (item.stackable)
                        {
                            writer.Write((byte)0x05);
                        }

                        if (item.corpse)
                        {
                            writer.Write((byte)0x06);
                        }

                        if (item.usable)
                        {
                            writer.Write((byte)0x07);
                        }

                        if (item.writable)
                        {
                            writer.Write((byte)0x08);
                            writer.Write((UInt16)item.writableLenght);
                        }

                        if (item.readable)
                        {
                            writer.Write((byte)0x08);
                            writer.Write((UInt16)item.readableLenght);
                        }

                        if (item.fluid)
                        {
                            writer.Write((byte)0x0A);
                        }

                        if (item.splash)
                        {
                            writer.Write((byte)0x0B);
                        }

                        if (item.blocking)
                        {
                            writer.Write((byte)0x0C);
                        }

                        if (item.immovable)
                        {
                            writer.Write((byte)0x0D);
                        }

                        if (item.blocksMissile)
                        {
                            writer.Write((byte)0x0E);
                        }

                        if (item.blocksPath)
                        {
                            writer.Write((byte)0x0F);
                        }

                        if (item.pickupable)
                        {
                            writer.Write((byte)0x10);
                        }

                        if (item.hangable)
                        {
                            writer.Write((byte)0x11);
                        }

                        if (item.horizontal)
                        {
                            writer.Write((byte)0x12);
                        }

                        if (item.vertical)
                        {
                            writer.Write((byte)0x13);
                        }

                        if (item.rotatable)
                        {
                            writer.Write((byte)0x14);
                        }

                        if (item.hasLight)
                        {
                            writer.Write((byte)0x15);
                            writer.Write((UInt16)item.lightLevel);
                            writer.Write((UInt16)item.lightColor);
                        }

                        if (item.dontHide)
                        {
                            writer.Write((byte)0x16);
                        }

                        if (item.floorchange)
                        {
                            writer.Write((byte)0x17);
                        }

                        if (item.hasOffset)
                        {
                            writer.Write((byte)0x18);
                            writer.Write((UInt16)item.offsetX);
                            writer.Write((UInt16)item.offsetY);
                        }

                        if (item.hasHeight)
                        {
                            writer.Write((byte)0x19);
                            writer.Write((UInt16)item.height);
                        }

                        if (item.layer)
                        {
                            writer.Write((byte)0x1A);
                        }

                        if (item.idleAnimated)
                        {
                            writer.Write((byte)0x1B);
                        }

                        if (item.minimap)
                        {
                            writer.Write((byte)0x1C);
                            writer.Write((UInt16)item.minimapColor);
                        }

                        if (item.actioned)
                        {
                            writer.Write((byte)0x1D);
                            writer.Write((UInt16)0); // missing
                        }

                        if (item.groundItem)
                        {
                            writer.Write((byte)0x1E);
                        }

                        writer.Write((byte)0xFF); //end of flags
                    }
                }
            }
            catch (Exception)
            {
                //
            }
           return true;
        }

        public static void AddNewObject()
        {
            Item newItem = new Item(Items.Last().id + 1);
            Items.Add(newItem);
        }
    }
}
