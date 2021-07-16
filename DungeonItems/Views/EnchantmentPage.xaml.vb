Imports DungeonItems.ViewModels

Namespace Global.DungeonItems.Views

    Public NotInheritable Class EnchantmentPage
        Inherits Page

        Public Property ViewModel As EnchantmentViewModel

        Protected Overrides Async Sub OnNavigatedTo(e As NavigationEventArgs)
            MyBase.OnNavigatedTo(e)
            ViewModel = DirectCast(e.Parameter, EnchantmentViewModel)
            ViewModel.EditImage = ItemImageEdit
            Await ViewModel.LoadImageAsync()
        End Sub

    End Class

End Namespace
