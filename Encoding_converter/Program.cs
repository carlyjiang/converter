using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

/*
 * This Program is used for converting text file encoding from UTF8 To Default (GBK)
*/

namespace Encoding_converter
{
    class Program
    {
        static void Main(string[] args)
        {
            string prefix = @"C:\Users\carl\Desktop\contact_book\";
            DirectoryInfo dir = new DirectoryInfo(prefix);
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                Encoding encoding = Program.GetEncoding(prefix + file.ToString());

                using (StreamWriter sw = new StreamWriter(prefix + "new_" + file.ToString(), true, Encoding.GetEncoding("GBK")))
                {
                    byte[] content_array = Program.ReadTest(prefix + file.ToString());

                    string content_string = Encoding.UTF8.GetString(content_array);
                    //Console.Write(content_string);
                    sw.Write(content_string);
                    /*
                    int count = 15;
                    foreach (byte b in content_array)
                    {
                        Console.Write(b.ToString("X2") + " ");
                        if (count-- == 0)
                        {
                            Console.WriteLine();
                            count = 15;
                        }
                    }
                     */
                    //Console.WriteLine(content_array.ToString());
                    //string content_string = Encoding.UTF8.GetString(content_array);
                }

//                Console.WriteLine(encoding.ToString());
                /*
                Console.WriteLine(file.ToString());
                using (StreamReader sr = new StreamReader(prefix + "\\" + file.ToString(), encoding))
                using (StreamWriter sw = new StreamWriter(prefix + "\\new_" + file.ToString(), true, Encoding.GetEncoding("Unicode")))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        //byte[] utf8_array = Encoding.UTF8.
                        char[] utf8_array = line.ToArray();
                        Console.WriteLine();
                        byte[] utf8_byte = Encoding.Unicode.GetBytes(utf8_array);


                        Console.WriteLine(Encoding.Unicode.GetString(utf8_byte));

                        sw.WriteLine(line);
                    }
                }
                 * 
                 */
            }

        }

        public static byte[] ReadTest(string filename)
        {
            FileStream file = null;
            try
            {
                file = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
                BinaryReader br = new BinaryReader(file);
                br.BaseStream.Seek(0, SeekOrigin.Begin);
                return br.ReadBytes((int)br.BaseStream.Length);
            }
            catch (Exception ea)
            {
                Console.Write(ea.ToString());
            }
            finally
            {
                if(file != null)
                {
                    file.Close();
                }
            }
            return null;
        }

        public static Encoding GetEncoding(string filename)
        {
            Encoding result = System.Text.Encoding.Default;
            FileStream file = null;
            try
            {
                file = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
                if (file.CanSeek)
                {
                    //   get   the   bom,   if   there   is   one   
                    byte[] bom = new byte[4];
                    file.Read(bom, 0, 4);
                    //   utf-8   
                    if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf)
                        result = System.Text.Encoding.UTF8;
                    //   ucs-2le,   ucs-4le,   ucs-16le,   utf-16,   ucs-2,   ucs-4   
                    else if ((bom[0] == 0xff && bom[1] == 0xfe) ||
                    (bom[0] == 0xfe && bom[1] == 0xff) ||
                    (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff))
                        result = System.Text.Encoding.Unicode;
                    //   else   ascii   
                    else
                        result = System.Text.Encoding.Default;
                }
                else
                {
                    //   can't   detect,   set   to   default   
                    result = System.Text.Encoding.Default;
                }
                return result;
            }
            finally
            {
                if (null != file) file.Close();
            }
        }
    }
}
