namespace TransactionRunner.ViewModels.Static
{
    public static class StaticModal
    {
        public static void ShowModal()
        {
            SetModals(System.Windows.Visibility.Visible);
        }
        public static void HideModal()
        {
            SetModals(System.Windows.Visibility.Collapsed);
        }
        private static void SetModals(System.Windows.Visibility visible)
        {
            StaticAgentSelectorVm.AgentSelectorViewModel.ModalVisibility = visible;
            StaticReceiveVm.ReceiveViewModel.ModalVisibility = visible;
            StaticSendParametersVm.SendParametersViewModel.ModalVisibility = visible;
            StaticStagedSendParametersVm.StagedSendParametersViewModel.ModalVisibility = visible;
            StaticSendReversalParametersVm.SendReversalParametersViewModel.ModalVisibility = visible;
            StaticStagedBillPayVm.StagedBillPayViewModel.ModalVisibility = visible;
            StaticTransactionPickerVm.TransactionPickerViewModel.ModalVisibility = visible;
            StaticBatchNumberVm.BatchNumberViewModel.ModalVisibility = visible;
            StaticStagedReceiveVm.StagedReceiveViewModel.ModalVisibility = visible;
            StaticBillPayVm.BillPayViewModel.ModalVisibility = visible;
        }
    }
}
