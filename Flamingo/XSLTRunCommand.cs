using System;
using System.Reflection;
using System.ComponentModel.Design;
using System.IO;
using System.Windows.Media;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Saxon.Api;
using Task = System.Threading.Tasks.Task;

namespace Flamingo
{
    internal sealed class XSLTRunCommand
    {
        public const int CommandId = 0x0100;
        private readonly AsyncPackage package;
        public static readonly Guid CommandSet = new Guid("e59e425b-e696-45f6-aeaa-7fe1cddfb04b");
        
        private XSLTRunCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(this.Execute, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        public static XSLTRunCommand Instance
        {
            get;
            private set;
        }

        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        public static async Task InitializeAsync(AsyncPackage package)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new XSLTRunCommand(package, commandService);
        }

        private void Execute(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            string styleSheetFileName = GetActiveDocument();
            string extension = Path.GetExtension(styleSheetFileName);

            if (Path.GetExtension(styleSheetFileName).Equals(".xslt") || Path.GetExtension(styleSheetFileName).Equals(".xsl"))
            {
                Transform(styleSheetFileName, Settings.Default.Input, Settings.Default.Output);
            }
            else 
            {
                string infoMessage = $"Select the stylesheet as the active document, before running Flamingo";
                VsShellUtilities.ShowMessageBox(
                    this.package,
                    infoMessage,
                    "Flamiongo",
                    OLEMSGICON.OLEMSGICON_INFO,
                    OLEMSGBUTTON.OLEMSGBUTTON_OK,
                    OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
            }
        }

        public static void Transform(string styleSheet, string inputFile, string outputFile) 
        {
            Processor processor = new Processor();
            var uriInput = new System.Uri(inputFile);            
            var uriStyleSheet = new System.Uri(styleSheet);
            
            XdmNode inputXdmNode = processor.NewDocumentBuilder().Build(uriInput);
            XsltCompiler xsltCompiler = processor.NewXsltCompiler();
            XsltExecutable xsltExecutable = xsltCompiler.Compile(uriStyleSheet);
            XsltTransformer xsltTransformer = xsltExecutable.Load();
            xsltTransformer.InitialContextNode = inputXdmNode;

            Serializer serializer = processor.NewSerializer();
            StreamWriter streamWriter = new StreamWriter(outputFile);
            serializer.SetOutputWriter(streamWriter);
            xsltTransformer.Run(serializer);
            streamWriter.Close();
        }       

        public static string GetActiveDocument()
        {
            EnvDTE80.DTE2 dteObject = Package.GetGlobalService(typeof(Microsoft.VisualStudio.Shell.Interop.SDTE)) as EnvDTE80.DTE2;
            return dteObject.ActiveDocument.FullName;            
        }
    }
}
