Imports DungeonItems.ViewModels

Namespace Global.DungeonItems.Views

    Public NotInheritable Class PerkPage
        Inherits Page

        Public Property ViewModel As PerkViewModel

        Protected Overrides Sub OnNavigatedTo(e As NavigationEventArgs)
            MyBase.OnNavigatedTo(e)
            ViewModel = DirectCast(e.Parameter, PerkViewModel)
        End Sub

    End Class

End Namespace
