using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace PegaNotas
{
    class Program
    {
        static public int vTotalCountDirOrigin;
        static public int vCountNotesMoved = 0;
        static public bool vValidFirstExec = true;
        static public int vProgressCount = 0;
        static void Main(string[] args)
        {
            while (true)
            {
                MoveFiles();
                if (vTotalCountDirOrigin == 0)
                {
                    break;
                }
            }
            Console.Write("                100% das notas movidas");
            Console.WriteLine("Arquivo vazio, fim das notas");
            Console.ReadKey();
        }

        private static void MoveFiles()
        {
            string vDirOrigin = @"diretório origem";
            string vDirDestiny = @"diretório destino";
            try
            {
                if (System.IO.Directory.Exists(vDirOrigin))
                {
                    string[] files = System.IO.Directory.GetFiles(vDirOrigin, "*.xml", System.IO.SearchOption.AllDirectories);
                    if (vValidFirstExec == true)
                    {
                        vTotalCountDirOrigin = files.Length;
                        Console.WriteLine("Total de notas no diretório : " + vTotalCountDirOrigin);
                        Console.WriteLine("Quantidade  |   |   Porcentagem de notas transferidas: ");
                    }
                    vValidFirstExec = false;
                    using (var progress = new ProgressBar())
                    {
                        foreach (string s in files)
                        {
                            string fileName = System.IO.Path.GetFileName(s);
                            string destFile = System.IO.Path.Combine(vDirDestiny, fileName);
                            System.IO.File.Move(s, destFile);
                            vProgressCount++;
                            vCountNotesMoved++;
                            progress.Report((double)vProgressCount / vTotalCountDirOrigin);
                            if (vCountNotesMoved >= 10000)
                            {
                                Thread.Sleep(8 * 60000);
                                while (!VerifyDestiny(vDirDestiny)) Thread.Sleep(4 * 60000);
                                vCountNotesMoved = 0;
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Pasta de origem não existe");
                }
            }
            catch (Exception)
            {
                Console.Write("\r{0}   ", vCountNotesMoved);
            }
        }

        private static bool VerifyDestiny(string vDirDestiny)
        {
            bool vStatusPasta = false;
            int vCountFiles = 0;
            string[] filePath = Directory.GetFiles(vDirDestiny, "*.xml");
            vCountFiles = filePath.Length;
            if (vCountFiles < 2000)
            {
                vStatusPasta = true;
            }
            return (vStatusPasta);
        }
    }
}

