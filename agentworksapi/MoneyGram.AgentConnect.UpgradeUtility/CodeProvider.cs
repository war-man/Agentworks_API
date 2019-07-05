using Microsoft.CSharp;

namespace MoneyGram.AgentConnect.UpgradeUtility
{
    public class CodeProvider
    {
        private static CodeProvider instance;
        private CSharpCodeProvider cSharpCodeProvider;

        private CodeProvider()
        {
            this.cSharpCodeProvider = new CSharpCodeProvider();
        }

        public static CodeProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CodeProvider();
                }

                return instance;
            }
        }

        public bool IsValidIdentifier(string identifier)
        {
            return cSharpCodeProvider.IsValidIdentifier(identifier);
        }
    }
}