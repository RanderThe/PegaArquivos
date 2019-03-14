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
        static void Main(string[] args)
        {
            while (true)
            {
                CopyFiles();
            }          
            // Keep console window open in debug mode.
            Console.WriteLine("Arquivo vazio, fim das notas");
            Console.ReadKey();
        }

        private static void CopyFiles()
        {
            int vTotalCountOrigin;
            int vProgressCount = 0;
            string vDirOrigin = @"C:\Users\rander\Desktop\Notas Veneza\Arquivos XML 2016 á 2019 Filial 12";
            string vDirDestiny = @"\\192.168.0.200\ftpxml_clientes\veneza";
            try
            {
                if (System.IO.Directory.Exists(vDirOrigin))
                {
                    string[] files = System.IO.Directory.GetFiles(vDirOrigin, "*.xml", System.IO.SearchOption.AllDirectories);
                    vTotalCountOrigin = files.Length;
                    Console.WriteLine("Total de notas no diretório : " + vTotalCountOrigin);
                    using (var progress = new ProgressBar())
                    {
                        int vCountForeach = 0;
                        foreach (string s in files)
                        {
                            string fileName = System.IO.Path.GetFileName(s);
                            string destFile = System.IO.Path.Combine(vDirDestiny, fileName);
                            System.IO.File.Move(s, destFile);
                            vProgressCount++;
                            progress.Report((double)vProgressCount/100000);
                            vCountForeach++;

                            if (vCountForeach >= 10000)
                            {
                                Thread.Sleep(10 * 60000);
                                while (!VerifyDestiny(vDirDestiny)) Thread.Sleep(5 * 60000);
                                vCountForeach = 0;
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
            {  }
        }


        private static bool VerifyDestiny(string vDirDestiny)
        {
            bool vStatusPasta = false;
            int vCountFiles = 0;
            string[] filePath = Directory.GetFiles(vDirDestiny, "*.xml");
            vCountFiles = filePath.Length;

            using (FileSystemWatcher watcher = new FileSystemWatcher())
            {
                watcher.Path = vDirDestiny;
                int vCountDir = watcher.Path.Count();
                if (vCountFiles < 10000)
                {
                    vStatusPasta = true;
                }
            }
            return (vStatusPasta);
        }
    }
}

