Imports DungeonItems.Repository

Namespace Global.DungeonItems.Views

    Public NotInheritable Class ImportResultDialog
        Inherits ContentDialog

        Public Property ViewModel As UpdateCounters

        Public Sub New(result As UpdateCounters)
            InitializeComponent()
            ViewModel = result
            DataContext = ViewModel

            Dim msg As String
            If result.Sum = 1 Then
                msg = "Es wurde ein Gegenstand verarbeitet."
            Else
                msg = "Es wurden & Gegenstände verarbeitet."
                msg = msg.Replace("&", result.Sum.ToString)
            End If
            Summary.Text = msg
        End Sub

        Private Sub ImportResultDialog_PrimaryButtonClick(sender As ContentDialog, args As ContentDialogButtonClickEventArgs)

        End Sub

        Private Sub ImportResultDialog_SecondaryButtonClick(sender As ContentDialog, args As ContentDialogButtonClickEventArgs)

        End Sub
    End Class

End Namespace
