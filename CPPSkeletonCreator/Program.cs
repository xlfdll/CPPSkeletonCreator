using System;
using System.Text;
using System.Windows.Forms;
using System.IO;

using CPPSkeletonCreator.Properties;

namespace CPPSkeletonCreator
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(String[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            String headerFileName = String.Empty;
            String codeFileName = String.Empty;

            Boolean isOverwriteOn = false; // Switch "/overwrite" boolean value
            Boolean[] FileOverwriteFlags = new Boolean[2]; // Indicate which file will be overwritten

            FileOverwriteFlags[0] = FileOverwriteFlags[1] = true; // Initial value for writing is a pass

            // CPPSkeletonCreator /?
            if (args.Length == 1 && args[0] == Resources.HelpSwitchName)
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine(Application.ProductName);
                sb.AppendLine(String.Format("Version {0}", Application.ProductVersion.ToString()));
                sb.AppendLine("© 2009 Xlfdll Workstation");
                sb.AppendLine();
                sb.AppendLine("Create C/C++ header and code skeleton files");
                sb.AppendLine();
                sb.AppendLine("Usage:");
                sb.AppendLine();
                sb.AppendLine("CPPSkeletonCreator [<HeaderFilePath> <CodeFilePath>] [/overwrite]");
                sb.AppendLine();
                sb.AppendLine("Parameters:");
                sb.AppendLine();
                sb.AppendLine("<HeaderFilePath> - Specifies the path of the header file");
                sb.AppendLine("<CodeFilePath> - Specifies the path of the code file");
                sb.AppendLine();
                sb.AppendLine("/overwrite - Suppresses the prompt and overwrites all existing files automatically");

                MessageBox.Show(sb.ToString(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                return;
            }

            // CPPSkeletonCreator [/overwrite]
            if (args.Length == 0 || (args.Length == 1 && args[0] == Resources.OverwriteSwitchName))
            {
                using (SaveFileDialog headerSaveFileDialog = new SaveFileDialog())
                {
                    headerSaveFileDialog.Filter = "C / C++ Header file (*.h)|*.h|All Files (*.*)|*.*";
                    headerSaveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    headerSaveFileDialog.OverwritePrompt = !(args.Length == 1 && args[0] == Resources.OverwriteSwitchName);

                    if (headerSaveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        headerFileName = headerSaveFileDialog.FileName;

                        using (SaveFileDialog codeSaveFileDialog = new SaveFileDialog())
                        {
                            codeSaveFileDialog.Filter = "C Code file (*.c)|*.c|C++ Code file (*.cpp)|*.cpp|All Files (*.*)|*.*";
                            codeSaveFileDialog.InitialDirectory = Path.GetDirectoryName(headerSaveFileDialog.FileName);
                            codeSaveFileDialog.FileName = Path.GetFileNameWithoutExtension(headerSaveFileDialog.FileName);
                            codeSaveFileDialog.OverwritePrompt = !(args.Length == 1 && args[0] == Resources.OverwriteSwitchName);

                            if (codeSaveFileDialog.ShowDialog() == DialogResult.OK)
                            {
                                codeFileName = codeSaveFileDialog.FileName;
                            }
                        }
                    }
                }

                isOverwriteOn = true;
            }
            // CPPSkeletonCreator <HeaderFilePath> <CodeFilePath> [/overwrite]
            else
            {
                headerFileName = args[0];
                codeFileName = args[1];

                isOverwriteOn = (args[args.Length - 1] == Resources.OverwriteSwitchName);
            }

            // If the code file name is empty, the whole process must be stopped
            // If the code file name is not empty, the header file name must not be empty

            if (!String.IsNullOrEmpty(codeFileName) && !isOverwriteOn)
            {
                DialogResult dialogResult = DialogResult.None;

                if (File.Exists(headerFileName))
                {
                    dialogResult = ShowOverwritePromptDialog(headerFileName);

                    if (dialogResult == DialogResult.Cancel)
                    {
                        return; // Application.Exit() cannot be used here. It will not exit the program immediately.
                    }

                    FileOverwriteFlags[0] = (dialogResult == DialogResult.Yes);
                }
                if (File.Exists(codeFileName))
                {
                    dialogResult = ShowOverwritePromptDialog(codeFileName);

                    if (dialogResult == DialogResult.Cancel)
                    {
                        return; // Application.Exit() cannot be used here. It will not exit the program immediately.
                    }

                    FileOverwriteFlags[1] = (dialogResult == DialogResult.Yes);
                }

                if (!(FileOverwriteFlags[0] || FileOverwriteFlags[1]))
                {
                    return; // Application.Exit() cannot be used here. It will not exit the program immediately.
                }
            }


            if (!String.IsNullOrEmpty(codeFileName))
            {
                try
                {
                    if (FileOverwriteFlags[0])
                    {
                        File.WriteAllText(headerFileName, Helper.CreateHeaderContents(headerFileName));
                    }

                    if (FileOverwriteFlags[1])
                    {
                        File.WriteAllText(codeFileName, Helper.CreateCodeContents(headerFileName));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(String.Format("The following exception has occurred:{0}{0}{1}", Environment.NewLine, ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        internal static DialogResult ShowOverwritePromptDialog(String fileName)
        {
            return MessageBox.Show(String.Format("File {0} already exists.{1}{1}Do you want to overwrite?", Path.GetFileName(fileName), Environment.NewLine), Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
        }
    }
}