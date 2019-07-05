namespace TransactionRunner.ImportExport
{
    public class StaticImportExport
    {
        private static ImportExportSvc _myImportExportSvc;

        public static ImportExportSvc ImportExportSvc
        {
            get => _myImportExportSvc ?? (_myImportExportSvc = new ImportExportSvc());
            set => _myImportExportSvc = value;
        }
    }
}